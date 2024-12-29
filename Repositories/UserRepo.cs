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
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Update user active status
        public void UpdateUserStatus(int uid, bool isActive)
        {
            try
            {
                var user = GetUserById(uid);
                if (user != null)
                {
                    user.IsActive = isActive;
                    _context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public void UpdateUser(User user)
        {
            try
            {
                // Only hash the password if it is updated
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }


        // Get user by email and password
        public User GetUser(string email, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                // Verify password
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }

                return null; // Invalid credentials
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

    }
}
