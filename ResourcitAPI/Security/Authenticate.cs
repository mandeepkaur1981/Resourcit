using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcitAPI.Security
{
    public static class Authenticate
    {
        public static bool ValidateKey(HttpRequest request, string configKey)
        {
            StringValues key = request.Headers["Key"];
            if (configKey == key)
                return true;
            return false;
        }
    }
}
