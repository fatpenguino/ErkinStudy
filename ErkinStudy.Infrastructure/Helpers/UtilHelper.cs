using System;
using System.Collections.Generic;
using System.Text;

namespace ErkinStudy.Infrastructure.Helpers
{
    public static class UtilHelper
    {
        public static string RemoveInputMaskFromPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Replace("+", "").Replace("(", "").Replace(")","").Replace("-", "");
        }
    }
}
