using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IUserService
    {
     
        void AddUser(User user);

        
        void DeactivateUser(int uid);

       
        User GetUserById(int uid);
        User GetUserByName(string userName);

        void UpdateUser(User user);
        string AuthenticateUser(string email, string password);
        void UpdatePassword(int uid, string newPassword);
        void AddSuperAdmin(UserInputDTO InputUser);
        bool EmailExists(string email);
        void AddStaff(UserInputDTO InputUser);
        UserOutputDTO GetUserData(string? userName, int? uid);
        IEnumerable<UserOutputDTO> GetUserByRole(string roleName);
    }
}
