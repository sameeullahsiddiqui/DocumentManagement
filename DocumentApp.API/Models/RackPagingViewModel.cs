namespace DocumentApp.API.Models
{
    public class RackPagingViewModel : PagingModel
    {
        internal int pageNumber;
        internal int pageSize;

        public string FileName { get;  set; }
        public string FolderName { get;  set; }
        public string BlockNumber { get;  set; }
        public string RackNumber { get;  set; }
    }
}