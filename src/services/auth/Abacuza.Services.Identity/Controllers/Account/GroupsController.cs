using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Abacuza.Services.Identity.Data;
using Abacuza.Services.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abacuza.Services.Identity.Controllers.Account
{
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            return Ok(await _context.Groups.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(string id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            
            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] dynamic model)
        {
            var json = (JsonElement)model;
            string name;
            if (json.TryGetProperty("name", out var nameElement))
            {
                name = nameElement.GetString();
            }
            else
            {
                return BadRequest("Group name is not specified.");
            }
            
            var group = new AbacuzaAppGroup(name);
            if (json.TryGetProperty("description", out var descriptionElement))
            {
                group.Description = descriptionElement.GetString();
            }

            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupById", new { id = group.Id }, group.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupById(string id)
        {
            var hasUsersInGroup = _context.UserGroups.Any(x => x.GroupId == id);
            if (hasUsersInGroup)
            {
                return BadRequest("There is at least one user in the group, remove all users and try again.");
            }

            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id); 
            _context.Groups.Remove(group);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignUserToGroup([FromBody] dynamic model)
        {
            var json = (JsonElement)model;
            string userId, groupId;
            if (json.TryGetProperty("userId", out var userIdElement))
            {
                userId = userIdElement.GetString();
            }
            else
            {
                return BadRequest("User Id is not specified.");
            }

            if (json.TryGetProperty("groupId", out var groupIdElement))
            {
                groupId = groupIdElement.GetString();
            }
            else
            {
                return BadRequest("Group Id is not specified.");
            }

            if (!_context.Users.Any(u => u.Id == userId))
            {
                return BadRequest("Specified User Id doesn't exist.");
            }

            if (!_context.Groups.Any(g => g.Id == groupId))
            {
                return BadRequest("Specified Group Id doesn't exist.");
            }

            var userGroup = new AbacuzaAppUserGroup
            {
                UserId = userId,
                GroupId = groupId
            };

            await _context.UserGroups.AddAsync(userGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("unassign")]
        public async Task<IActionResult> UnassignUserFromGroup([FromBody] dynamic model)
        {
            var json = (JsonElement)model;
            string userId, groupId;
            if (json.TryGetProperty("userId", out var userIdElement))
            {
                userId = userIdElement.GetString();
            }
            else
            {
                return BadRequest("User Id is not specified.");
            }

            if (json.TryGetProperty("groupId", out var groupIdElement))
            {
                groupId = groupIdElement.GetString();
            }
            else
            {
                return BadRequest("Group Id is not specified.");
            }

            var userGroup =
                await _context.UserGroups.FirstOrDefaultAsync(ug => ug.GroupId == groupId && ug.UserId == userId);
            if (userGroup != null)
            {
                _context.UserGroups.Remove(userGroup);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
        
        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> GetGroupsByUserId(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound($"User with specified Id ({userId}) doesn't exist.");
            }

            var query = from groups in _context.Groups
                join userGroups in _context.UserGroups on groups.Id equals userGroups.GroupId
                where userGroups.UserId == userId
                select groups;
            
            return Ok(query);
        }
    }
}