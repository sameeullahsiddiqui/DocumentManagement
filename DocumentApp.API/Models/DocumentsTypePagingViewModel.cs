using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentApp.API.Models
{
    public class DocumentsTypePagingViewModel : PagingModel
    {
        public string DocumentTypeName { get; set; }
    }
}