using System;
using System.Net;

namespace DocumentApp.API.Exceptions
{
    [Serializable]
    public class DocumentAppException : Exception
    {
        public DocumentAppException(HttpStatusCode statusCode, string errorCode, string errorDescription) : base($"{errorCode}::{errorDescription}")
        {
            StatusCode = statusCode;
        }

        public DocumentAppException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}