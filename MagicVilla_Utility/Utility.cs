﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicVilla_Utility
{
    public static class Utility
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }

        public static string SessionToken = "JWTToken";
    }
}
