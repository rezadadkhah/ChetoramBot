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
            if (number > 0)
                return false;
            return true;
        }
        public static bool IsNullOrEmptyOrWhitespace(this string number)
        {
            if (string.IsNullOrWhiteSpace(number) || number == string.Empty )
                return true;
            return false;
        }


    }
}
