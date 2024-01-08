namespace SportsStore.Models.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

    public PagingInfo PagingInfo { get; set; } = new();
    // Adding this property allows to add functionality of navigating products by category
    public string? CurrentCategory { get; set; }
}
