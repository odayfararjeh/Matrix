
using Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sajaya_Task.Models;
using System;
using System.Net;

namespace Sajaya_Task.Helper
{
    public class IdentityHelper
    {
        public static ResponseModel GenerateResponseModel(ErrorEnum? stateEnum, ModelErrorCollection errors, object response, HttpStatusCode status, Exception ex = null)
        {
            ErrorEnum? statusCode = null;
            if (stateEnum != null)
            {
                statusCode = (ErrorEnum)stateEnum;
            }

            var responseModel = new ResponseModel
            {
                Data = response,
                Status = status,
                StatusText = status.ToString(),
                Errors = errors,
                ErrorCode = statusCode,
                ErrorCodeText = statusCode?.ToString()
            };

            return responseModel;
        }
    }
}
