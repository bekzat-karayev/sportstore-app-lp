using System.Text.Json;

namespace SportsStore.infrastructure;

/*  The session state feature in ASP.NET Core stores only int, string, and byte[] values. 
    Since I want to store a Cart object, I need to define extension methods to the ISession interface, 
which provides access to the session state data to serialize Cart objects into JSON and convert them back.  
*/
public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(key));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        string? sessionData = session.GetString(key) ?? string.Empty;

        return session == null ? default : JsonSerializer.Deserialize<T>(sessionData);
    }
}
