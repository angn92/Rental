namespace Rental.Infrastructure.Exceptions
{
    public  class ErrorCode
    {
        #region General errors
        public static string IncorrectArgument => "incorrect_argument";
        public static string EnumMapError => "could_not_be_mapped";
        public static string EmailInUse => "email_in_use";
        public static string InvalidEmail => "invalid_email";
        public static string InvalidPhoneNumber => "invalid_phone_number";
        #endregion


        #region Customer errors
        public static string UsernameExist => "username_exist";
        public static string UserNotExist => "customer_not_exist";
        public static string AccountBlocked => "account_is_blocked";
        public static string AccountNotActive => "account_is_not_active";
        #endregion


        #region Session errors
        public static string SessionDoesNotExist => "session_not_exist";
        public static string SessioNotActive => "session_not_active";
        public static string SessionExpired => "session_expired";
        public static string SessionNotAuthorized => "sesion_not_authorized";
        public static string WrongSessionState => "wrong_session_satte";
        #endregion


        #region Password errors
        public static string PasswordNotExist => "password_not_exist";
        public static string PasswordIncorrect => "new_password_is_identical_like_old_password";
        #endregion


        #region Product errors
        public static string ProductExist => "product_exist";
        public static string ProductNotExist => "product_not_exist";
        public static string ProductNotAvailable => "product_not_available";
        public static string ProductIsNotReserved => "product_is_not_reserved";
        #endregion


        #region Order errors
        public static string OrderNotExist => "order_not_exist";
        public static string OrderWasFinishedOrCancelled => "order_was_finished_or_cancelled";
        #endregion
    }
}
