using Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Sajaya_Task.Models
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Errors = new ModelErrorCollection();
        }
        public HttpStatusCode Status { get; set; }
        public string StatusText { get; set; }
        public ModelErrorCollection Errors { get; set; }
        public object Data { get; set; }
        public ErrorEnum? ErrorCode { get; set; }
        public string ErrorCodeText { get; set; }
    }
}
