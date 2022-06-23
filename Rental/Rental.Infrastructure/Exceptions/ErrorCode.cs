namespace Rental.Infrastructure.Exceptions
{
    public  class ErrorCode
    {
        public static string IncorrectArgument => "incorrect_argument";
        public static string EnumMapError => "could_not_be_mapped";

        /// <summary>
        /// User account error
        /// </summary>
        public static string UsernameExist => "username_exist";
        public static string UserNotExist => "user_not_exist";
        public static string AccountBlocked => "account_is_blocked";
        public static string AccountNotActive => "account_is_not_active";

        /// <summary>
        /// Email error
        /// </summary>
        public static string EmailInUse => "email_in_use";
        public static string InvalidEmail => "invalid_email";

        /// <summary>
        /// Phone error 
        /// </summary>
        public static string InvalidPhoneNumber => "invalid_phone_number";

        /// <summary>
        /// Session error
        /// </summary>
        public static string SessionDoesNotExist => "session_not_exist";
        public static string SessioNotActive => "session_not_active";
        public static string SessionExpired => "session_expired";
        public static string SessionNotAuthorized => "sesion_not_authorized";

        /// <summary>
        /// Password error 
        /// </summary>
        public static string PasswordNotExist => "password_not_exist";

        public static string PasswordIncorrect => "new_password_is_identical_like_old_password";

        /// <summary>
        /// Product exist 
        /// </summary>
        public static string ProductExist => "product_exist";
    }
}
