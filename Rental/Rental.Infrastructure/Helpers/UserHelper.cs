﻿using Rental.Core.Domain;
using Rental.Core.Enum;
using System;

namespace Rental.Infrastructure.Helpers
{
    public interface IUserHelper
    {
        string CheckAccountStatus(User user);
    }

    public class UserHelper : IUserHelper
    {
        public string CheckAccountStatus(User user)
        {
            string status = null;

            switch (user.Status)
            {
                case AccountStatus.Active:
                    status = AccountStatus.Active.ToString();
                    break;
                case AccountStatus.Blocked:
                    status = AccountStatus.Blocked.ToString();
                    break;
                case AccountStatus.NotActive:
                    status = AccountStatus.NotActive.ToString();
                    break;
            }
            return status;
        }
    }
}
