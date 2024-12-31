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

        public IEnumerable<PatientRecord> GetRecordsByDoctorId(int doctorId)
        {
            return _repository.GetAll()
                .Where(record => record.DID == doctorId)
                .ToList();
        }

        public IEnumerable<PatientRecord> GetRecordsByBranchId(int branchId)
        {
            return _repository.GetAll()
                .Where(record => record.BID == branchId)
                .ToList();
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
