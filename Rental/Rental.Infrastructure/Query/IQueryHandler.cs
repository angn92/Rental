using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Query
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
    {
        ValueTask<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
