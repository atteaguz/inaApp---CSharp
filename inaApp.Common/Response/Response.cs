using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Response
{
    public class Response<T>
    {
        public T Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
