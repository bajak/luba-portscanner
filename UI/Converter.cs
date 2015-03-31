using System;
using System.Runtime.CompilerServices;

namespace UI
{
    public class Converter
    {
        public static Int32[] SemicolonSeperatedStringToInt32Array(string value)
        {
            string[] splitedValue;
            if (value.Contains(","))
                splitedValue = value.Split(',');
            else if (value.Contains(";"))
                splitedValue = value.Split(';');
            else
                splitedValue = value.Split(' ');

            var intArrayValue = new int[splitedValue.Length];

            for (var i = 0; i < splitedValue.Length; i++)
                if (splitedValue[i] != string.Empty)
                    intArrayValue[i] = Convert.ToInt32(splitedValue[i].Replace(" ", ""));

            return intArrayValue;
        }

        public static string Int32ArrayToSemicolonSeperatedString(Int32[] value)
        {
            var portsStr = string.Empty;
            for (var i = 0; i < value.Length; i++)
            {
                portsStr += value[i];
                if (i != value.Length - 1)
                    portsStr += ", ";
            }
            return portsStr;
        }
    }
}
