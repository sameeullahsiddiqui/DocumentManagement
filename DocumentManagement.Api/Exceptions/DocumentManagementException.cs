using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DocumentManagement.API.Exceptions
{
    [Serializable]
    public class DocumentManagementException : Exception
    {
        public DocumentManagementException(HttpStatusCode statusCode, string errorCode, string errorDescription) : base($"{errorCode}::{errorDescription}")
        {
            StatusCode = statusCode;
        }

        public DocumentManagementException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}