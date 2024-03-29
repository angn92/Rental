﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public static string InvlaidInputParameters => "invalid_input_parameters";
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
        public static string NoProducts => "no_any_products";
        #endregion


        #region Order errors
        public static string OrderNotExist => "order_not_exist";
        public static string OrderWasFinishedOrCancelled => "order_was_finished_or_cancelled";
        public static string ClientHasNoActiveOrders => "client_has_no_active_orders";
        #endregion

        #region Category error
        public static string CategoryNotExist => "category_not_exist";
        public static string ListOfCategoriesIsEmpty => "list_of_categories_is_empty";
        public static string CategoryExist => "category_exist";
        #endregion

        #region Dictionary error
        public static string DictionaryNotFound => "dictionary_not_found";
        #endregion
    }
}
