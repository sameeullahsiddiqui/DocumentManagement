using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.API.ViewModels
{
    public class RackPagingViewModel:PagingModel
    {
        public string RackNumber { get; set; }
        public string BlockNumber { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }
    }
}
