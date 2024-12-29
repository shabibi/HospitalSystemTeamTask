using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IUserService
    {
     
        void AddUser(User user);

        
        void DeactivateUser(int uid);

       
        User GetUserById(int uid);

        
        void UpdateUser(User user);
        User GetUSer(string email, string password);
        void AddDoctor(User doctor);
        void UpdatePassword(int uid, string newPassword);
        void AddSuperAdmin(UserInputDTO InputUser);
        bool EmailExists(string email);
    }
}
