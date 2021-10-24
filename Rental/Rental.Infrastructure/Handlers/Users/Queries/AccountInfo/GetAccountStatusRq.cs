﻿using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusRq : IQuery
    {
        public string Username { get; set; }
        public string IdSession { get; set; }
    }
}