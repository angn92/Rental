using System;

namespace Rental.Core.Validation
{
    public static class ValidationParameter
    {
        /// <summary>
        /// Check that given parameter is null, empty or whitespace.
        /// </summary>
        /// <param name="arg"></param>
        public static void FailIfNullOrEmpty(string arg)
        {
            if (String.IsNullOrWhiteSpace(arg))
                throw new Exception($"Parameter {nameof(arg)} can not be null or empty.");
        }

        public static void FailIfNull(object arg)
        {
            if (arg is null)
                throw new Exception($"Object {nameof(arg)} can not be null.");
        }
    }
}
