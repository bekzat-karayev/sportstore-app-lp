
namespace SportsStore.Models
{
    /*  The repository implementation just maps the Products property defined by the IStoreRepository interface onto the
    Products property defined by the StoreDbContext class. The Products property in the context class returns
    a DbSet<Product> object, which implements the IQueryable<T> interface and makes it easy to implement
    the repository interface when using Entity Framework Core.
    */
    public class EFStoreRepository : IStoreRepository
    {
        public StoreDbContext context;

        public EFStoreRepository(StoreDbContext storeDbContext)
        {
            context = storeDbContext;
        }

        public IQueryable<Product> Products => context.Products;
    }
}
