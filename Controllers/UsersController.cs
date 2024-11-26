using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EpsBackend.Data;
using EpsBackend.Models;
using EpsBackend.Utils;
using System.Diagnostics;
using Microsoft.Identity.Client;

namespace EpsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EpsBackendContext _context;

        public UsersController(EpsBackendContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        public class LoginUserBody
        {
            public required string Email {  get; set; }
            public required string Password { get; set; }
        }

        [HttpGet("login")]
        public async Task<ActionResult<User>> Login(LoginUserBody user)
        {
            // Check user exists

            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("There is missing information");
            }

            var newUser = await _context.User.FirstOrDefaultAsync(u =>  u.Email == user.Email);
            if (newUser == null)
            {
                return NotFound("Could not find user");
            }

            // Check supplied password against hash in database
            
            if (BCrypt.Net.BCrypt.Verify(user.Password, newUser.Password) == false)
            {
                return Unauthorized("Password invalid");
            }


            HttpContext.Response.Cookies.Append("Acookie", "Tasty", new CookieOptions
            {

            });

            return Ok(newUser);
            

        } 

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            string hashedPassword = PasswordHash.HashedPassword(user.Password);
            user.Password = hashedPassword;


            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
