using System;
using System.IO;
using System.Text.RegularExpressions;


namespace UI
{
    public class Validator : Validator<FormatException> { }

    public class Validator<TException> where TException : Exception, new ()
    {
        public bool IPAddressValidation(object value, string propertyName = null)
        {
            const string pattern = @"([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})$";
            if (Regex.IsMatch((string)value, pattern))
                return true;
            throw GetException(propertyName, "xxx.xxx.xxx.xxx");
        }

        public bool FilepathValidation(object value, string propertyName = null)
        {
            var valueStr = (string)value;
            const string pattern = @"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.(?i)(txt|doc)$";
            if (Regex.IsMatch(valueStr, pattern))
                return true;
            if (valueStr.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                return true;
            throw GetException(propertyName, @"c:\files\output.txt");
        }

        public bool CommaSepratedIntValidation(object value, string propertyName = null)
        {
            const string pattern = @"(\d+)(;\s*\d+)*";
            if (Regex.IsMatch((string)value, pattern))
                return true;
            throw GetException(propertyName, "21;80;8080");
        }

        private static TException GetException(string propertyName, string format)
        {
            var exception = (TException)Activator.CreateInstance(typeof(TException), 
                GetExceptionMessage(propertyName, format));
            exception.Source = propertyName;
            return exception;
        }

        private static string GetExceptionMessage(string propertyName, string format)
        {
            return propertyName + "have a wrong format. Format: " + format;
        }
    }
}
