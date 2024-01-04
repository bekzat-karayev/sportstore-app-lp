namespace SportsStore.Models.ViewModels
{
    /*  To support the PageLinkTagHelper class we need to pass information about the number of pages available, 
    the current page and the total number of products in the repository. The PagingInfo ViewModel will serve
    this exact purpose.
    */
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
