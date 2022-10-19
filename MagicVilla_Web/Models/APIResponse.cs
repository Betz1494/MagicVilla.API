﻿using System.Net;

namespace MagicVilla_Web.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }
        public string Message { get; set; } 
        public object Result { get; set; }

    }
}
