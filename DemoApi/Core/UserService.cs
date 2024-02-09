using Microsoft.Extensions.Caching.Memory;

namespace DemoApi.Core;

public class UserService
{
    private readonly IMemoryCache cache;

    public UserService(IMemoryCache cache)
    {
        this.cache = cache;
    }

    // non-working demo just to show how to remove the cache
    public void SetRoles(string email, List<string> roles)
    {
        // go update the database
        cache.Remove("UserRoles" + email);
    }
    
    public async Task<List<string>> UserRoles(string email)
    {
        var roles = cache.Get<List<string>>("UserRoles" + email);

        if (roles != null)
        {
            Console.WriteLine("Roles found in cache.");
            return roles;
        }
        
        Console.WriteLine("Roles not found in cache. Fetching from database.");
        roles = await DbCallGetRoles(email);

        cache.Set("UserRoles" + email, roles, TimeSpan.FromHours(5));
        Console.WriteLine("Roles added to cache.");

        return roles;
    }
    
    async Task<List<string>> DbCallGetRoles(string email)
    {
        var emailRoles = new Dictionary<string, List<string>>()
        {
            { "test@fake.com", ["Admin"] },
            { "test2@fake.com", ["User", "Publisher"] },
            { "example@sample.com", ["Editor", "Contributor"] },
            { "user3@domain.com", ["Viewer"] },
            { "admin@example.com", ["Admin", "Editor", "User"] }
        };

        // Simulate DB call delay
        await Task.Delay(200);

        return emailRoles.TryGetValue(email, out var roles) ? roles : new List<string>();
    }
}