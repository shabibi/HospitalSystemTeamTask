using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IClinicService
    {
        IEnumerable<Clinic> GetAllClinic();
        void AddClinic(ClinicInput input);
        Clinic GetClinicById(int clinicId);
        Clinic GetClinicByName(string clinicName);
        IEnumerable<Clinic> GetClinicsByBranchName(string branchName);
        IEnumerable<Clinic> GetClinicsByDepartmentId(int departmentId);
        void UpdateClinicDetails(int CID, ClinicInput input);
    }
}
