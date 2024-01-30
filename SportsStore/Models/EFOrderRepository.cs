
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public class EFOrderRepository : IOrderRepository
{
    private StoreDbContext context;

    public EFOrderRepository(StoreDbContext storeDbContext)
    {
        context = storeDbContext;
    }

    /*  I used the Include and ThenInclude methods to specify that when an Order object is read from the database, the collection 
    associated with the Lines property should also be loaded along with each Product object associated with each collection 
    object. This ensures that I receive all the data objects that I need without having to perform separate queries
    and then assemble the data myself.
    */
    public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);
    
    public void SaveOrder(Order order)
    {
        /*  An additional step is also required when I store an Order object in the database. When the user’s cart data is 
        de-serialized from the session store, new objects are created that are not known to Entity Framework Core, which then tries 
        to write all the objects into the database. 
            For the Product objects associated with an Order, this means that Entity Framework Core tries to write objects 
        that have already been stored, which causes an error. To avoid this problem, I notify Entity Framework Core that the 
        objects exist and shouldn’t be stored in the database unless they are modified using AttachRange method.
            This ensures that Entity Framework Core won’t try to write the de-serialized Product objects that are
        associated with the Order object.
        */
        context.AttachRange(order.Lines.Select(l => l.Product));
        if (order.OrderID == 0)
        {
            context.Orders.Add(order);
        }

        context.SaveChanges();
    }
}
