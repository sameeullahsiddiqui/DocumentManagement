using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.API.ViewModels
{
    public class DocumentsTypePagingViewModel : PagingModel
    {
        public string DocumentType { get; set; }
    }
}
