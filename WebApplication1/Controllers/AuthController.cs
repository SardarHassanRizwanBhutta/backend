using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
namespace WebApplication1.Controllers
{
    // AuthRequestDTO a subset of User model - dto, view model, input model
    public class AuthRequest { 
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}

    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly JwtService _jwtService;

        private readonly UserService _userService;

        public AuthController(DatabaseContext context, JwtService jwtService, UserService userService)
        {
            _context = context;
            _jwtService = jwtService;
            _userService = userService;
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Task - represents an asynchronous operations
        // IActionResult - does not enforces a return type can only be used for returning response codes
        // ActionResult - does enforces a return type can be used for returning response codes - The action expected return
        // type is inferred from <ActionResult<T>>
        // Automatically serializes the object to json
        [HttpPost("sign-up")]
        // <ActionResult>
        public async Task<IActionResult> SignUp(User user)
        {
            var result = await _userService.GetUserByEmail(user.Email);
            if(result != null) {
                return BadRequest(new {message="Email already taken", data = (object) null});
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            result = await _userService.AddUser(user);
            // return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            // return Ok(user);
            return Ok(new {message="Success", data = result});
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<User>> SignIn(AuthRequest user)
        {
            var result = await _userService.GetUserByEmail(user.Email);
            if(result == null || !BCrypt.Net.BCrypt.Verify(user.Password, result.Password))
            {
                return BadRequest(new {message="Invalid Username or password", data = (object) null});
            }
            Console.WriteLine($"User: {result.Email}, Role: {result.Role?.Name}");
            var token = _jwtService.GenerateToken(result);
            return Ok(new {message="Success", data= token});
        }
    }
}