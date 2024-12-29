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
            _userRepo.AddUser(user);
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
