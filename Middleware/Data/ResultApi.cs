using Middleware.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Middleware.Data
{
    /// <summary>
    /// Result work server.
    /// </summary>
    public class ResultApi
    {
        public object Result { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
