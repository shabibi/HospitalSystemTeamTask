using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IClinicRepocs
    {
        public IEnumerable<Clinic> GetAllClinic();
        void AddClinic(Clinic clinic);
        Clinic GetClinicById(int Cid);
    }
}
