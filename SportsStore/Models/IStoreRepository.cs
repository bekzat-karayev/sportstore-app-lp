namespace SportsStore.Models
{
    /*  The repository pattern is one of the most widely used, and it provides a consistent way to access 
    the features presented by the database context class. Not everyone finds a repository useful, but my experience is 
    that it can reduce duplication and ensures that operations on the database are performed consistently.
        This interface uses IQueryable<T> to allow a caller to obtain a sequence of Product objects. 
    The IQueryable<T> interface is derived from the more familiar IEnumerable<T> interface and represents a
    collection of objects that can be queried, such as those managed by a database.
    */
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }

        void SaveProduct(Product p);
        void CreateProduct(Product p);
        void DeleteProduct(Product p);
    }
}
