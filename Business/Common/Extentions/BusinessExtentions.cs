using Business.Business;
using Business.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Common.Extentions
{
    public static class BusinessExtentions
    {
        

        public static bool IsNotPositive(this int number)
        {
            return number <= 0;
        }
        public static bool IsPositive(this int number)
        {
            return number > 0;
        }
        public static bool IsNullOrEmptyOrWhitespace(this string number)
        {
            if (string.IsNullOrWhiteSpace(number) || number == string.Empty )
                return true;
            return false;
        }


    }
}
