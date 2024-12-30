
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
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
            // Validate the email input
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email is required." });

            // Validate the password input
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { message = "Password is required." });

            try
            {
                // Attempt to authenticate the user and generate a token
                string token = _userService.AuthenticateUser(email, password);
                return Ok(token );

            }
            catch (InvalidOperationException ex)
            {
                // Return an HTTP 400 response if the user account is inactive or invalid
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Return an HTTP 401 response if authentication fails due to invalid credentials
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while login. {(ex.Message)}");
            }

        }

        [Authorize]
        [HttpPut("DeactivateUser")]
        public IActionResult DeactivateUser(int userId)
        {
            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check if the user's role allows them to perform this action
                if (userRole == null && userRole != "admin" && userRole != "superAdmin")
                    return BadRequest("You are not authorized to perform this action.");

                // Validate the user ID
                if (userId < 0)
                    return BadRequest("Invalid input");

                _userService.DeactivateUser(userId);
                return Ok(new { message = "User deactivated successfully." });

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while adding the product: {ex.Message}");
            }
        }
        
        [HttpGet("GetUser")]
        public IActionResult GetUser(int? UserID, string ? UserName)
        {
            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                var userId =int.Parse( JwtHelper.GetClaimValue(token, "sub"));

                // Check if the user's role allows them to perform this action
                if (userRole == null && userRole != "admin" && userRole != "superAdmin" && userRole != "doctor")
                    return BadRequest("You are not authorized to perform this action.");

                // Validate the user ID
                if (UserID < 0)
                    return BadRequest("Invalid input");

                // Validate input: At least one parameter must be provided
                if (!UserID.HasValue && string.IsNullOrWhiteSpace(UserName))
                    return BadRequest(new { message = "Invalid input. Provide either UserID or UserName." });

                // Validate input: Ensure only one field is used for search
                if (UserID.HasValue && !string.IsNullOrWhiteSpace(UserName))
                    return BadRequest(new { message = "Invalid input. Provide only one field (UserID or UserName) to search." });

                var user = _userService.GetUserData(UserName,UserID);


                if (userRole == "patient" && userId != user.UID)
                    return BadRequest("You are not authorized to get data of other patients .");

                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 if the user is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving user. {(ex.Message)}");
            }
        }

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


    }
}
