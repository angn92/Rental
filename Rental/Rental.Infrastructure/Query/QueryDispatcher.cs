using Autofac;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _context;

        public QueryDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new Exception("Error");

            var handler = await _context.Resolve<IQueryHandler<TQuery, TResult>>().HandleAsync(query);
            return handler;
        }
    }
}
