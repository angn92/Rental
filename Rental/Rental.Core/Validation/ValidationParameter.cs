using System;

namespace Rental.Core.Validation
{
    public static class ValidationParameter
    {
        /// <summary>
        /// Check that given parameter is null, empty or whitespace.
        /// </summary>
        /// <param name="arg"></param>
        public static void FailIfNullOrEmpty(string arg, string parameter = null)
        {
            if (String.IsNullOrWhiteSpace(arg))
                throw new ArgumentNullException(parameter);
        }

        public static void FailIfNull(object arg)
        {
            if (arg == null)
                throw new ArgumentNullException($"Object {nameof(arg)} can not be null.");
        }
    }
}
