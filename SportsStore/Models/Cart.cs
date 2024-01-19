namespace SportsStore.Models;

public class Cart
{
    public List<CartLine> Lines { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        CartLine? line = Lines.Where(l => l.Product.ProductID == product.ProductID).FirstOrDefault();

        if (line == null)
        {
            Lines.Add(new CartLine { Product = product, Quantity = quantity});
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public void RemoveLine(Product product)
    {
        Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);
    }

    public decimal ComputeTotalValue()
    {
        return Lines.Sum(l => l.Product.Price * l.Quantity);
    }

    public void Clear() => Lines.Clear();
}

public class CartLine 
{
    public int CartLineID { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}
