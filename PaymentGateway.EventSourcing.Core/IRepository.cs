using System.Threading.Tasks;
using PaymentGateway.EventSourcing.Core.Aggregate;
using PaymentGateway.EventSourcing.Core.Exception;

namespace PaymentGateway.EventSourcing.Core
{
    /// <summary>
    /// Represents an asynchronous, virtual collection of <typeparamref name="TAggregateRoot"/>.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root in this collection.</typeparam>
    /// <typeparam name="TAggregateId">The type identifier for <typeparamref name="TAggregateRoot"/></typeparam>
    public interface IRepository<TAggregateRoot, in TAggregateId> where TAggregateId : IAggregateId
    {
        /// <summary>
        /// Gets the aggregate root entity associated with the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <returns>An instance of <typeparamref name="TAggregateRoot"/>.</returns>
        /// <exception cref="AggregateNotFoundException">Thrown when an aggregate is not found.</exception>
        Task<TAggregateRoot> Get(TAggregateId identifier);

        /// <summary>
        /// Persists the uncommitted events from the aggregate root entity.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root entity.</param>
        Task Save(TAggregateRoot aggregateRoot);
    }
}
