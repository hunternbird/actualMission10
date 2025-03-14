using actualMission10.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// [Route("api/[controller]")]
// [ApiController]
// public class BowlersController : ControllerBase
// {
//     private readonly BowlingDbContext _context;
//
//     public BowlersController(BowlingDbContext context)
//     {
//         _context = context;
//     }
//
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<Bowler>>> GetBowlers()
//     {
//         return await _context.Bowlers
//             .Where(b => _context.Teams.Any(t => t.TeamId == b.TeamId && (t.TeamName == "Marlins" || t.TeamName == "Sharks")))
//             .ToListAsync();
//             
//             
//     }
// }

[Route("api/[controller]")]
[ApiController]
public class BowlersController : ControllerBase
{
    private readonly BowlingDbContext _context;

    public BowlersController(BowlingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBowlers()
    {
        var bowlers = await _context.Bowlers
            .Include(b => b.Team)
            .Where(b => b.Team.TeamName == "Marlins" || b.Team.TeamName == "Sharks")
            .Select(b => new 
            {
                Name = $"{b.BowlerFirstName} {b.BowlerMiddleInit} {b.BowlerLastName}".Trim(),
                Team = b.Team.TeamName,
                Address = b.BowlerAddress,
                City = b.BowlerCity,
                State = b.BowlerState,
                Zip = b.BowlerZip,
                Phone = b.BowlerPhoneNumber
            })
            .ToListAsync();

        return Ok(bowlers);
    }
}
