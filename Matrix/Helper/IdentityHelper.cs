﻿
using Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Matrix.Models;
using System;
using System.Net;

namespace Matrix.Helper
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
