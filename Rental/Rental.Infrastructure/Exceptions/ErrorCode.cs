﻿namespace Rental.Infrastructure.Exceptions
{
    public  class ErrorCode
    {
        public static string UsernameExist => "username_exist";
        public static string EmailInUse => "email_in_use";
        public static string InvalidEmail => "invalid_email";
        public static string InvalidPhoneNumber => "invalid_phone_number";
        public static string AccountBlocked => "account_is_blocked";
        public static string AccountNotActive => "account_is_not_active";
        public static string ProducyUnavailable => "product_unavailable";
        public static string InvalidCredential => "invalid_credential";
        public static string UserNotExist => "user_not_exist";
        public static string IncorrectArgument => "incorrect_argument";
        public static string SessionDoesNotExist => "session_not_exist";
        public static string SessioNotActive => "session_not_active";
        public static string SessionExpired => "session_expired";
        public static string PasswordNotExist => "password_not_exist";
    }
}
