﻿using System.Threading.Tasks;

namespace Rental.Infrastructure.Query
{
    public interface IQueryDispatcher 
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}
