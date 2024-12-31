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

    }
}
