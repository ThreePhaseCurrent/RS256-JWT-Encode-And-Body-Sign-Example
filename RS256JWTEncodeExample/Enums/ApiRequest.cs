using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RS256JWTEncodeExample.Enums
{
    public static class ApiRequest
    {
        public enum ContentTypes
        {
            [Description("application/json")]
            JSON = 0,
            [Description("application/text")]
            TEXT = 1
        }
    }
}
