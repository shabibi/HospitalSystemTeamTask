using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IClinicRepocs
    {
        public IEnumerable<Clinic> GetAllClinic();
        void AddClinic(Clinic clinic);
        Clinic GetClinicById(int Cid);
        Clinic GetClinicByName(string ClinicName);
        IEnumerable<Clinic> GetClinicsByBranchName(string branchName);
    }
}
