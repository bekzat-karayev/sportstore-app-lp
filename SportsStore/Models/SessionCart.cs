using System.Text.Json.Serialization;
using SportsStore.infrastructure;

namespace SportsStore.Models;

/*  The SessionCart class subclasses the Cart class and overrides the `AddItem`, `RemoveLine`, and `Clear` methods 
so they call the base implementations and then store the updated state in the session using the extension methods 
on the ISession interface. 
    The static GetCart method is a factory for creating SessionCart objects and providing them with an ISession object 
so they can store themselves.
    Getting hold of the ISession object is a little complicated. I obtain an instance of the IHttpContextAccessor service, 
which provides me with access to an HttpContext object that, in turn, provides me with the ISession. 
This indirect approach is required because the session isn’t provided as a regular service.
*/
public class SessionCart : Cart
{
    public static Cart GetCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
        SessionCart cart = session?.GetJson<SessionCart>("cart") ?? new();
        cart.Session = session;

        return cart;
    }

    [JsonIgnore]
    public ISession? Session { get; set; }

    public override void AddItem(Product product, int quantity)
    {
        base.AddItem(product, quantity);
        Session?.SetJson("Cart", this);
    }

    public override void RemoveLine(Product product)
    {
        base.RemoveLine(product);
        Session?.SetJson("Cart", this);
    }

    public override void Clear()
    {
        base.Clear();
        Session?.Remove("Cart");
    }
}
