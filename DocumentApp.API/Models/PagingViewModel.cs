namespace DocumentApp.API.Models
{
    public class PagingModel
    {
        const int maxPageSize = -1;
        private int _pageSize;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}