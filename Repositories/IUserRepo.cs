using HospitalSystemTeamTask.Models;
using System.Collections.Generic;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IUserRepo
    {
        
        User GetUserById(int uid);

        
        void AddUser(User user);

        User GetUserByEmail(string email);

        bool IsValidRole(string roleName);
        void UpdateUser(User user);
        bool EmailExists(string email);
        void UpdatePassword(int uid, string newPassword);
        IEnumerable<User> GetUserByRole(string roleName);
        User GetUserByName(string userName);
        string GetUserName(int uid);
    }
}
