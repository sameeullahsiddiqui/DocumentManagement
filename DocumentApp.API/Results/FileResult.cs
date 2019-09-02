using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DocumentApp.API.Results
{
    public class FileResult : IHttpActionResult
    {
        public FileResult(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            FilePath = filePath;
        }

        public string FilePath { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(File.OpenRead(FilePath));
            var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(FilePath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return Task.FromResult(response);
        }
    }
}