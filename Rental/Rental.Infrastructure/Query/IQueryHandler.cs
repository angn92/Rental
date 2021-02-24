using System.Threading.Tasks;

namespace Rental.Infrastructure.Query
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery 
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
