using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IDoctorService
    {
        IEnumerable<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int uid);
        Doctor GetDoctorByEmail(string email);
        bool EmailExists(string email);
        Doctor GetDoctorByName(string docName);
        DoctorOutPutDTO GetDoctorData(string? docName, int? Did);
        void AddDoctor(DoctorOutPutDTO input);
        IEnumerable<DoctorOutPutDTO> GetDoctorsByBranchName(string branchName);
        IEnumerable<DoctorOutPutDTO> GetDoctorsByDepartmentName(string departmentName);
        public void UpdateDoctorDetails(int DID, DoctorUpdateDTO input);
        void UpdateDoctor(Doctor doctor);

    }
}
