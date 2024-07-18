
using Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Matrix.Utilities
{
    public class ResponseMiddleWare
    {
        private readonly RequestDelegate _next;
        public ResponseMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var accessToken = context.Request.Headers[CommonNames.Authorization];

            await _next(context);
        }
    }
}
