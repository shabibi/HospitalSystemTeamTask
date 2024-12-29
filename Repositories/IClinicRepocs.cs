using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IClinicRepocs
    {
        public IEnumerable<Clinic> GetAllClinic();
    }
}
