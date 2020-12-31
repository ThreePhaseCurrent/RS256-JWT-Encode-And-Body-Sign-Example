using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using RS256JWTEncodeExample.Enums;

namespace RS256JWTEncodeExample.Extentions
{
    public static class EnumExtension
    {
        public static string ToDescriptionString(this ApiRequest.ContentTypes val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
