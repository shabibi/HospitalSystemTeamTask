using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;

namespace HospitalSystemTeamTask.Services
{
    public class PatientRecordService : IPatientRecordService
    {
        private readonly IPatientRecordRepository _repository;

        public PatientRecordService(IPatientRecordRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<PatientRecord> GetAllRecords()
        {
            return _repository.GetAll();
        }

        public PatientRecord GetRecordById(int id)
        {
            return _repository.GetById(id);
        }

        public void CreateRecord(PatientRecord record)
        {
            _repository.Add(record);
        }

        public void UpdateRecord(PatientRecord record)
        {
            _repository.UpdateRecord(record);
        }

        public void DeleteRecord(PatientRecord record)
        {
            _repository.DeleteRecord(record);
        }
    }
}
