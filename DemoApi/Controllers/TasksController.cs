using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    [HttpGet(Name = "WhenAllVsParallel")]
    public async Task<IActionResult> Test1()
    {
        var ids = Enumerable.Range(1, 10).ToList();
        
        await WhenAll(ids);
        
        await ParallelForeach(ids);
            

        return Ok(new { Message = "Test1 completed" });
    }
        
    private async Task Process(int id)
    {
        await Task.Delay(2000);
        Console.WriteLine($"Processed id: {id}");
    }
    
    // Good option for batches of I/O that WON'T throttle/limit the destination (api, db, etc).
    private async Task WhenAll(List<int> ids)
    {
        Console.WriteLine("Before WhenAll: " + DateTime.Now);
        await Task.WhenAll(ids.Select(id => Process(id)));
        Console.WriteLine("After WhenAll: " + DateTime.Now);
    }

    // Good option for processing that COULD throttle/limit the destination
    // Also good for light/medium CPU bound in small batches
    private async Task ParallelForeach(List<int> ids)
    {
        // Parallel.ForEach
        Console.WriteLine("Before ParallelForeach: " + DateTime.Now);
        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = 2
        };
            
        await Parallel.ForEachAsync(ids, options, async (id, token) =>
        {
            await Process(id);
        });
        Console.WriteLine("After ParallelForeach: " + DateTime.Now);
    }
}