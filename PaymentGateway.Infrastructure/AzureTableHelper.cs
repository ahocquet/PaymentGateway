using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace PaymentGateway.Infrastructure
{
    public class AzureTableHelper<T> where T : ITableEntity, new()
    {
        private const string PartitionKey = "PartitionKey";
        private const string RowKey       = "RowKey";

        public AzureTableHelper(int maximumParallelism = 1, int maximumBatchOperations = 100)
        {
            MaximumParallelism     = maximumParallelism;
            MaximumBatchOperations = maximumBatchOperations;
        }

        private int MaximumParallelism     { get; }
        private int MaximumBatchOperations { get; }

        public async Task<TableBatchResult[]> InsertOrReplaceInBatch(IEnumerable<T> entities, CloudTable table)
        {
            void OperationDispatcher(TableBatchOperation batchOperation, ITableEntity entity)
            {
                batchOperation.InsertOrReplace(entity);
            }

            var result = await ExecuteBatchOperation(entities, table, OperationDispatcher);

            return result;
        }

        public async Task<TableBatchResult[]> DeletePartitionFromTableAsync(string partitionKey, CloudTable table)
        {
            void OperationDispatcher(TableBatchOperation batchOperation, ITableEntity entity)
            {
                batchOperation.Delete(entity);
            }

            var entities = await ReadPartitionAsync(partitionKey, table);
            var result   = await ExecuteBatchOperation(entities, table, OperationDispatcher);

            return result;
        }

        public Task<TableResult> RetrieveAsync(string partitionKey, string rowKey, CloudTable table)
        {
            return table.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
        }

        public async Task DeleteByRowKey(CloudTable table, string rowKey)
        {
            var filter   = TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, rowKey);
            var query    = new TableQuery<T>().Where(filter);
            var entities = await InternalExecuteQuery(table, query);

            if (entities == null)
            {
                return;
            }

            await DeleteAsync(entities.ToArray(), table);
        }

        /**
             * This method reads a whole partition from a table.
             */
        public async Task<IEnumerable<T>> ReadPartitionAsync(string partitionKey, CloudTable table)
        {
            var filter   = TableQuery.GenerateFilterCondition(PartitionKey, QueryComparisons.Equal, partitionKey);
            var query    = new TableQuery<T>().Where(filter);
            var entities = await InternalExecuteQuery(table, query);

            return entities;
        }

        public async Task<IEnumerable<T>> GetByRowKeyAsync(string rowKey, CloudTable table)
        {
            var filter   = TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, rowKey);
            var query    = new TableQuery<T>().Where(filter);
            var entities = await InternalExecuteQuery(table, query);

            return entities;
        }

        /**
             * This method dumps all data from a table.
             */
        public async Task<IEnumerable<T>> ReadAllAsync(CloudTable table)
        {
            var query    = new TableQuery<T>();
            var entities = await InternalExecuteQuery(table, query);

            return entities;
        }

        public async Task DeleteAsync(T[] dataSrc, CloudTable table)
        {
            var batchTasks = new List<Task<TableResult>>();
            foreach (var data in dataSrc)
            {
                var task = table.ExecuteAsync(TableOperation.Delete(data));
                batchTasks.Add(task);

                if (batchTasks.Count == MaximumParallelism)
                {
                    await Task.WhenAll(batchTasks);
                    batchTasks.Clear();
                }
            }

            await Task.WhenAll(batchTasks);
        }

        public Task DeleteAsync(T dataSrc, CloudTable table)
        {
            return DeleteAsync(new[] {dataSrc}, table);
        }

        public Task<TableResult> DeleteIfExists(CloudTable table, string partitionKey, string rowKey)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var entity = new DynamicTableEntity(partitionKey, rowKey) {ETag = "*"};
            var op     = TableOperation.Delete(entity);

            return DeleteIfExistsInternal(table, op);
        }

        public Task InsertOrReplace(CloudTable table, ITableEntity projection)
        {
            return table.ExecuteAsync(TableOperation.InsertOrReplace(projection));
        }

        public Task InsertOrMerge(CloudTable table, ITableEntity projection)
        {
            return table.ExecuteAsync(TableOperation.InsertOrMerge(projection));
        }

        private async Task<List<T>> InternalExecuteQuery(CloudTable table, TableQuery<T> query)
        {
            TableContinuationToken token    = null;
            var                    entities = new List<T>();
            do
            {
                var page = await table.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(page.Results);
                token = page.ContinuationToken;
            } while (token != null);

            return entities;
        }

        private Task<TableBatchResult[]> ExecuteBatchOperation(IEnumerable<T> entities, CloudTable table, Action<TableBatchOperation, ITableEntity> operationDispatcher)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            return ExecuteBatchOperationInternalAsync(entities, table, operationDispatcher);
        }

        private async Task<TableBatchResult[]> ExecuteBatchOperationInternalAsync(IEnumerable<T> entities, CloudTable table,
                                                                                  Action<TableBatchOperation, ITableEntity> operationDispatcher)

        {
            await table.CreateIfNotExistsAsync();

            var batchTasks = new List<Task<TableBatchResult>>();
            var enumerable = entities as T[] ?? entities.ToArray();

            for (var i = 0; i < enumerable.Length; i += MaximumBatchOperations)
            {
                var batchItems = enumerable.Skip(i)
                                           .Take(MaximumBatchOperations)
                                           .ToList();

                var batchOperation = new TableBatchOperation();
                foreach (var item in batchItems)
                {
                    operationDispatcher(batchOperation, item);
                }

                var task = table.ExecuteBatchAsync(batchOperation);
                batchTasks.Add(task);

                if (batchTasks.Count == MaximumParallelism)
                {
                    await Task.WhenAll(batchTasks);
                    batchTasks.Clear();
                }
            }

            var result = await Task.WhenAll(batchTasks);
            return result;
        }

        private async Task<TableResult> DeleteIfExistsInternal(CloudTable table, TableOperation operation)
        {
            try
            {
                var result = await table.ExecuteAsync(operation);
                return result;
            }
            catch (Exception ex)
            {
                var statusCode = GetStorageStatusCode(ex);
                return new TableResult {HttpStatusCode = statusCode};
            }
        }

        private int GetStorageStatusCode(Exception ex)
        {
            return ex is StorageException se && se.RequestInformation != null ? se.RequestInformation.HttpStatusCode : 500;
        }
    }
}
