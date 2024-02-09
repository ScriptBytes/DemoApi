namespace DemoApi.Core;

public class UserService
{
    public async Task<List<string>> UserRoles(string email)
    {
        return await DbCallGetRoles(email);
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