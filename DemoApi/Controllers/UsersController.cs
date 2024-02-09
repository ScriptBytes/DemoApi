using DemoApi.Core;
using DemoApi.Data;
using DemoApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DefaultDbContext context;
    private readonly UserService userService;

    public UsersController(DefaultDbContext context, UserService userService)
    {
        this.context = context;
        this.userService = userService;
    }

    [HttpGet(Name = "AllUsers")]
    public async Task<ActionResult> Get(
        string name = "",
        DateTime? birthStart = null,
        DateTime? birthEnd = null,
        int page = 1,
        int pageSize = 50
        )
    {
        var query = context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => u.Name == name);
        }

        if (birthStart.HasValue)
        {
            birthStart = DateTime.SpecifyKind(birthStart.Value, DateTimeKind.Utc);
            query = query.Where(u => u.DateOfBirth >= birthStart);
        }
        
        if (birthEnd.HasValue)
        {
            birthEnd = DateTime.SpecifyKind(birthEnd.Value, DateTimeKind.Utc);
            query = query.Where(u => u.DateOfBirth <= birthEnd);
        }

        var results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(results);
    }
    
    [HttpGet("{id:int}",Name = "GetUser")]
    public async Task<ActionResult> GetUser(int id)
    {
        var user = await context.Users
            .Select(u => new
            {
                Id = u.Id,
                Name = u.Name,
                Posts = u.Posts
            })
            .FirstOrDefaultAsync(u => u.Id == id);

        return user == null ? NotFound() : Ok(user);
    }
    
    [HttpPut("{id:int}", Name = "UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] User user)
    {
        var existing = await context.Users.FindAsync(user.Id);
        
        if (existing == null)
        {
            return NotFound();
        }
        
        existing.Name = user.Name;
        existing.DateOfBirth = user.DateOfBirth;
        existing.Email = user.Email;
        
        context.ChangeTracker.DetectChanges();
        await context.SaveChangesAsync();
        
        return Ok(existing);
        
        
        // Just for reference since I didn't make actual endpoints for these, the code for adding and deleting:
        // context.AddAsync(user);
        // await context.SaveChangesAsync();

        // context.Remove(existing);
        // await context.SaveChangesAsync()
    }
    
    [HttpGet("roles", Name = "UserRoles")]
    public async Task<ActionResult> GetRoles(string email)
    {
        var roles = await userService.UserRoles(email);

        return Ok(roles);
    }
}