using CineCritique.DAL.DTOS;
using CineCritique.domain.Models;
using CineCritique.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineCritique.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _userService;

        public UserController(IUser userService)
        {
            _userService = userService;
        }

        // إضافة مستخدم
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User cannot be null.");
            }
            if (string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Password cannot be null or empty.");
            }


            // تحويل UserDto إلى User
            var user = new User
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                Role = userDto.Role, //admin, topuser, regular
                IsActive = userDto.IsActive,
                PasswordHash = userDto.Password
            };

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // تحديث مستخدم
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // تحديث بيانات المستخدم باستخدام UserDto
            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            user.Role = userDto.Role;
            user.IsActive = userDto.IsActive;

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        // عرض جميع المستخدمين
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            // تحويل المستخدمين إلى UserDto
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                Password=user.PasswordHash
                
            }).ToList();

            return Ok(userDtos);
        }

        // عرض تفاصيل مستخدم
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // تحويل المستخدم إلى UserDto
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                Password=user.PasswordHash

            };

            return Ok(userDto);
        }

        // حذف مستخدم
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
