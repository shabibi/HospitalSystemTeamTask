using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalSystemTeamTask.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepo userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        // Add user
        public void AddUser(User user)
        {
            //hashing Password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _userRepo.AddUser(user);
        }

        public void AddSuperAdmin(UserInputDTO InputUser)
        {
            if (InputUser.Role != "superAdmin")
                throw new ArgumentException("Invalid role. Only 'superAdmin' role is allowed.", nameof(InputUser.Role));
            //check if there is any active supper admin
            var existingSuperAdmins = _userRepo.GetUserByRole(InputUser.Role);
                if (existingSuperAdmins != null && existingSuperAdmins.Any(u => u.IsActive))
                {
                    throw new InvalidOperationException("Only one active super admin is allowed in the system.");
                }
                else
                {
                    String defaultPassword = "Super1234";
                    string generatedEmail =$"{InputUser.UserName}@gmail.com";
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword);
                    var newSupperAdmin = new User
                    {
                        UserName = InputUser.UserName,
                        Email = generatedEmail,
                        Phone = InputUser.Phone,
                        Password = hashedPassword,
                        Role = InputUser.Role,
                        IsActive = true
                    };
                    _userRepo.AddUser(newSupperAdmin);
                }
                   
        }
         
        
        //Add hospital stuff (admin or doctor ) 
        public void AddStaff(UserInputDTO InputUser)
        {
            if (InputUser.Role.ToLower() != "doctor" && InputUser.Role.ToLower() != "admin")
                throw new ArgumentException("Invalid role. Only 'doctor and admin' role is allowed.", nameof(InputUser.Role));

            String defaultPassword = "Staff1234";

            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string generatedEmail = $"{InputUser.UserName}{randomNumber}@gmail.com";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword);
            var newStaff = new User
            {
                UserName = InputUser.UserName,
                Email = generatedEmail,
                Phone = InputUser.Phone,
                Password = hashedPassword,
                Role = InputUser.Role,
                IsActive = true
            };
            _userRepo.AddUser(newStaff);

        }

        // Deactivate user
        public void DeactivateUser(int uid)
        {
            var user = _userRepo.GetUserById(uid);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");

            if(!user.IsActive)
                throw new ArgumentException("User already not active.");
            
            user.IsActive = false;
            _userRepo.UpdateUser(user); // Deactivate the user
        }

        // Get user by ID
        public User GetUserById(int uid)
        {
            var user = _userRepo.GetUserById(uid);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");
            return user;
        }

        public User GetUserByName(string userName)
        {
            var user = _userRepo.GetUserByName(userName);
            if (user == null)
                throw new KeyNotFoundException($"User with Name {userName} not found.");
            return user;
        }
        
        public UserOutputDTO GetUserData(string ? userName, int ? uid)
        {
            User user = null;

            // Validate that at least one parameter is provided
            if (string.IsNullOrWhiteSpace(userName) && !uid.HasValue)
                throw new ArgumentException("Either username or user ID must be provided.");

            // Retrieve user based on username 
            if (!string.IsNullOrEmpty(userName))
                 user = GetUserByName(userName);

            // Retrieve user based on UID
            if (uid.HasValue)
                user = GetUserById(uid.Value);


            if(user == null)
                throw new KeyNotFoundException($"User not found.");

            var outputData = new UserOutputDTO
            {
                UID = user.UID,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return (outputData);
        }
        public void UpdateUser(User user)
        {
            var existingUser = _userRepo.GetUserById(user.UID);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.UID} not found.");

            _userRepo.UpdateUser(user);
        }

        public string AuthenticateUser(string email, string password)
        {
            // Retrieve the user from the repository using the provided email
            var user = _userRepo.GetUserByEmail(email);

            // Check if a user with the given email exists
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email");

            // Verify the provided password against the stored hashed password
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new ArgumentException("Incorrect Password");

            // Ensure the user's account is active
            if (!user.IsActive)
                throw new ArgumentException("This user account is inactive");

            return GenerateJwtToken(user.UID.ToString(), user.UserName, user.Role);
        }

        public void UpdatePassword(int uid, string newPassword)
        {
            var user = _userRepo.GetUserById(uid);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");

            _userRepo.UpdatePassword(uid, newPassword);
        }

        public bool EmailExists(string email)
        {
            return _userRepo.EmailExists(email);
        }

        public string GenerateJwtToken(string userId, string username , string role)
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
