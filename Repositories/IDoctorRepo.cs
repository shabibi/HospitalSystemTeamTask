using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IDoctorRepo
    {
        IEnumerable<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int Did);
        Doctor GetDoctorByEmail(string email);
        bool EmailExists(string email);

        Doctor GetDoctorByName(string docName);
        void AddDoctor(Doctor doctor);
        IEnumerable<Doctor> GetDoctorByBranchName(string branchName);
        IEnumerable<Doctor> GetDoctorsByDepartmentName(string departmentName);
        void UpdateDoctor(Doctor doctor);

    }
}
