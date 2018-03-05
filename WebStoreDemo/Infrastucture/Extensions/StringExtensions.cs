using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStoreDemo.Infrastucture.Extensions
{
    public static class StringExtensions
    {
        public static decimal GenerateDecimal(this string source, int totalDigits = 4, int precision = 2)
        {
            int total = 0;
            foreach (char c in source)
            {
                var i = (int) c;
                total += i;
                
            }

            if (total == 0)
                return 0;

            var totalAsString = total.ToString();
            totalAsString = totalAsString.PadLeft(4, '0');
            totalAsString = totalAsString.Substring(totalAsString.Length - totalDigits);
            totalAsString = totalAsString.Insert(totalAsString.Length - precision, ".");
            return  decimal.Parse(totalAsString);
             



        }
    }
}
