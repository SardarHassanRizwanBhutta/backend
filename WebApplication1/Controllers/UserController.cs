using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("users")]
    [ApiController] // This class now responds to web api requests
    public class UserController : ControllerBase
    {
        
        private readonly UserService _userService;

        public UserController(DatabaseContext context, UserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
           var users = await _userService.GetUsers();
           return Ok(users);
       }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _userService.GetUserById(id);
        // Explicitly checking if user is not null, because it does not convert the obj into truthy or falsy 
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Put request requires to send entire updated entity not partial updates 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
             if (id != user.Id)
            {
                return BadRequest();
            }

            var result = await _userService.GetUserById(id);

            if(result == null)
            {
                return NotFound();
            }

            result.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            result.Email = user.Email; 

            result.RoleId = user.RoleId;

            await _userService.UpdateUser(result);

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _userService.GetUserById(id); //EF uses in-memory snapshots to track changes to our entities
            // if we have entity cache in our snapshot we can save one extra round trip to database for Find or FindAsync methods
            if (user == null)
            {
                return NotFound();
            }
            await _userService.DeleteUser(user);
            return NoContent();
        }

    }
}