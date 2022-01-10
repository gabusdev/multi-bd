using todos.Models;
using todos.Services;
using Microsoft.AspNetCore.Mvc;

namespace todos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _context;
        public UserController(UserService service)
        {
            _context = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll() =>
            await _context.GetAllAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _context.GetAsync(id);

            if (user is null)
                return NotFound();

            return user;
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            var existingUser = await _context.GetAsync(id);
            if (existingUser is null)
                return NotFound();
            user.Id = existingUser.Id;
            await _context.Update(user);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            await _context.AddAsync(user);
            return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _context.GetAsync(id);

            if (user is null)
                return NotFound();

            await _context.DeleteAsync(id);

            return NoContent();
        }
    }
}
