using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Cosmos.Table;
using PaymentGateway.Domain.Values;
using PaymentGateway.Infrastructure;
using PaymentGateway.Read.Models;
using PaymentGateway.Read.Repositories.Entities;
using PaymentGateway.SharedKernel.Constants;

namespace PaymentGateway.Read.Repositories
{
    /// <summary>
    /// Repository implementation for <see cref="PaymentView"/>.<br/>
    /// Primary Key : Payment Id<br/>
    /// Row Key : Payment Id
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMapper                         _mapper;
        private readonly CloudTable                      _table;
        private readonly AzureTableHelper<PaymentEntity> _azTableHelper;

        public PaymentRepository(IDictionary<string, CloudTable> tables, IMapper mapper)
        {
            _mapper        = mapper;
            _table         = tables[ContainerName.PaymentProjection];
            _azTableHelper = new AzureTableHelper<PaymentEntity>();
        }

        public async Task<PaymentView> GetById(PaymentId id)
        {
            var tableResult = await _azTableHelper.RetrieveAsync(id.IdAsString(), id.IdAsString(), _table);
            var paymentView = _mapper.Map<PaymentView>(tableResult.Result);
            return paymentView;
        }

        public async Task Save(PaymentView payment)
        {
            var entity = _mapper.Map<PaymentEntity>(payment);
            await _azTableHelper.InsertOrMerge(_table, entity);
        }
    }
}
