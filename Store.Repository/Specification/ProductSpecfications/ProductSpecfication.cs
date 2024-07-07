namespace Store.Repository.Specification.ProductSpecfications
{
    public class ProductSpecfication
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SortBy { get; set; }
        public int PageIndex { get; set; } = 1;
        public const int MAXPAGESIZE = 50;
        
        private int _pageSize = 6;
        public int PigeSize 
        { 
            get => _pageSize;
            set => _pageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value; 
        }

        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }

    }
}
