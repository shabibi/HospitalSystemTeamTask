using HospitalSystemTeamTask.Models;
using System.Collections.Generic;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IUserRepo
    {
        
        User GetUserById(int uid);

        
        void AddUser(User user);

        void UpdateUserStatus(int uid, bool isActive);

        
        User GetUser(string email, string password);

        bool IsValidRole(string roleName);
        void UpdateUser(User user);
        bool EmailExists(string email);
        void UpdatePassword(int uid, string newPassword);
    }
}
