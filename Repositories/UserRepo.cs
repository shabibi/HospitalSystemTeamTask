using HospitalSystemTeamTask.Models;
using System.Collections.Generic;
using System.Linq;

namespace HospitalSystemTeamTask.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get user by ID
        public User GetUserById(int uid)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.UID == uid);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Add new user
        public void AddUser(User user)
        {
            try
            {
                // Hash the password before saving
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Update user status
        public void UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Get user by email and password
        public User GetUserByEmail(string email)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

       
        public bool IsValidRole(string roleName)
        {
            var validRoles = new List<string> { "Patient", "Admin", "Doctor" };
            return validRoles.Contains(roleName);
        }

        //for  adding Doctor:
        public bool EmailExists(string email)
        {
            try
            {
                return _context.Users.Any(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void UpdatePassword(int uid, string newPassword)
        {
            try
            {
                var user = GetUserById(uid);
                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public IEnumerable<User> GetUserByRole(string roleName)
        {
            try
            {
               return _context.Users.Where(u => u.Role == roleName).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public User GetUserByName(string userName)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
    }
}
