
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalSystemTeamTask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("RegisterSuperAdmin")]
        public IActionResult RegisterSuperAdmin(UserInputDTO InputUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (InputUser == null)
                    return BadRequest("User data is required");

                // Normalize role by trimming spaces and comparing case-insensitively
                var normalizedRole = InputUser.Role?.Trim();

                // Add the user
                _userService.AddSuperAdmin(InputUser);

                return Ok("Super Admin registered successfully.");
            }
            catch (Exception ex)
            {
                // Log and return the error
                return StatusCode(500, $"An error occurred while adding the superAdmin. {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterNewStaff")]
        public IActionResult RegisterNewStaff(UserInputDTO InputUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (InputUser == null)
                    return BadRequest("User data is required");

                // Normalize role by trimming spaces and comparing case-insensitively
                var normalizedRole = InputUser.Role?.Trim();

                // Add the user
                _userService.AddStaff(InputUser);

                return Ok("New staff registered successfully.");
            }
            catch (Exception ex)
            {
                // Log and return the error
                return StatusCode(500, $"An error occurred while adding new staff. {ex.Message}");
            }
        }
        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login(string email, string password)
        {
            try
            {
                var user = _userService.GetUSer(email, password);
                string token = GenerateJwtToken(user.UID.ToString(), user.UserName, user.Role);
                return Ok(token);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while login. {(ex.Message)}");
            }

        }


        [HttpGet("GetUserById/{UserID}")]
        public IActionResult GetUserById(int UserID)
        {
            try
            {
                var user = _userService.GetUserById(UserID);
                return Ok(user);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving user. {(ex.Message)}");
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost("AddDoctor")]
        //public IActionResult AddDoctor(UserInputDTO inputDoctor)
        //{
        //    try
        //    {
        //        if (inputDoctor == null)
        //            return BadRequest("Doctor data is required.");

        //        var doctor = new User
        //        {
        //            UserName = inputDoctor.UserName,
        //            Email = inputDoctor.Email,
        //            Password = inputDoctor.Password, // Temporary password
        //            Role = "Doctor",
        //            IsActive = true
        //        };

        //        _userService.AddDoctor(doctor);

        //        return Ok("Doctor added successfully. Share the email and temporary password with the doctor.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while adding the doctor: {ex.Message}");
        //    }
        //}
        [Authorize]
        [HttpPut("UpdatePassword")]
        public IActionResult UpdatePassword(UpdatePasswordDTO passwordDto)
        {
            try
            {
                if (string.IsNullOrEmpty(passwordDto.NewPassword))
                    return BadRequest("New password is required.");

                _userService.UpdatePassword(passwordDto.UserId, passwordDto.NewPassword);

                return Ok("Password updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the password: {ex.Message}");
            }
        }




        [NonAction]
        public string GenerateJwtToken(string userId, string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
