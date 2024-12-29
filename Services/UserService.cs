using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;

namespace HospitalSystemTeamTask.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
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

            _userRepo.UpdateUserStatus(uid, false); // Deactivate the user
        }

        // Get user by ID
        public User GetUserById(int uid)
        {
            var user = _userRepo.GetUserById(uid);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");
            return user;
        }

        
        public void UpdateUser(User user)
        {
            var existingUser = _userRepo.GetUserById(user.UID);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.UID} not found.");

            _userRepo.UpdateUser(user);
        }

        public User GetUSer(string email, string password)
        {
            var user = _userRepo.GetUser(email, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            return user;
        }

        //adding Doctor:
        public void AddDoctor(User doctor)
        {
            if (doctor.Role != "Doctor")
                throw new InvalidOperationException("Invalid role. Only doctors can be added via this method.");

            if (_userRepo.EmailExists(doctor.Email))
                throw new InvalidOperationException("Email already exists.");

            _userRepo.AddUser(doctor);
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

    }
}
