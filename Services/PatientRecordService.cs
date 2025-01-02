using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using System.Numerics;

namespace HospitalSystemTeamTask.Services
{
    public class PatientRecordService : IPatientRecordService
    {
        private readonly IPatientRecordRepository _repository;
        private readonly IDoctorService _doctorService;
        private readonly IClinicService _clinicService;
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;

        public PatientRecordService(IPatientRecordRepository repository, IDoctorService doctorService,
            IClinicService clinicService, IUserService userService, IBranchService branchService)
        {
            _repository = repository;
            _doctorService = doctorService;
            _clinicService = clinicService;
            _userService = userService;
            _branchService = branchService;
        }

        public IEnumerable<PatientRecordOutput> GetAllRecords()
        {
            var records = _repository.GetAll();
            List<PatientRecordOutput> output = new List<PatientRecordOutput>();
            Doctor doctor = null;
             
            
            foreach (var record in records)
            {
                doctor = _doctorService.GetDoctorById(record.DID);
                output.Add(new PatientRecordOutput
                {
                    RecordId = record.RID,
                    PatientId = record.PID,
                    PatientName = _userService.GetUserName(record.PID),
                    DoctorName = _userService.GetUserName(record.DID),
                    BranchName = _branchService.GetBranchName(record.BID),
                    ClinicName = _clinicService.GetClinicName(doctor.CID.Value),
                    VisitDate = record.VisitDate,
                    VisitTime = record.VisitTime,
                    Inspection = record.Inspection,
                    Treatment = record.Treatment,
                    Price = record.Cost,
                });
               
            }
            return output;
        }

        public void CreateRecord(PatientRecordInputDTO record, int doctorId)
        {
            var doctor = _doctorService.GetDoctorById(doctorId);
            if (doctor == null)
                throw new ArgumentException("Doctor not found.");

            if (!doctor.CID.HasValue)
                throw new ArgumentException("Doctor is not associated with a clinic.");

            var branch = doctor.CurrentBrunch;
            var cost = _clinicService.GetPrice(doctor.CID.Value);

            var newRecord = new PatientRecord
            {
                PID = record.PtientID,
                DID = doctorId,
                VisitDate = DateOnly.FromDateTime(DateTime.Now),
                VisitTime = TimeSpan.FromTicks(DateTime.Now.TimeOfDay.Ticks),
                Cost = cost,
                BID = branch,
                Treatment = record.Treatment,
                Inspection = record.Inspection
            };

            // Validate inputs before saving
            if (newRecord.PID <= 0 || newRecord.BID <= 0 || newRecord.DID <= 0)
                throw new ArgumentException("Invalid IDs provided.");

            _repository.Add(newRecord);
        }

        //Get records depends on given parameter
        public IEnumerable<PatientRecordOutput> GetRecords(int ? RecordId, int? patientId, int? doctorId, int? branchId)
        {
            if (!patientId.HasValue && !doctorId.HasValue && !branchId.HasValue && !RecordId.HasValue)
                throw new ArgumentException("At least one filter parameter (PatientId, DoctorId, or BranchId) must be provided.");

            // Filter records based on provided parameters
            var filteredRecords = _repository.GetAll().Where(record =>
                (!patientId.HasValue || record.PID == patientId.Value) &&
                (!doctorId.HasValue || record.DID == doctorId.Value) &&
                (!branchId.HasValue || record.BID == branchId.Value)&&
                (!RecordId.HasValue || record.RID == RecordId.Value)
            ).ToList();

            if (!filteredRecords.Any())
                throw new InvalidOperationException("No records found matching the specified criteria.");

          
            // Generate output with additional details
            List<PatientRecordOutput> output = new List<PatientRecordOutput>();
            Doctor doctor = null;


            foreach (var record in filteredRecords)
            {
                doctor = _doctorService.GetDoctorById(record.DID);
                output.Add(new PatientRecordOutput
                {
                    RecordId = record.RID,
                    PatientId = record.PID,
                    PatientName = _userService.GetUserName(record.PID),
                    DoctorName = _userService.GetUserName(record.DID),
                    BranchName = _branchService.GetBranchName(record.BID),
                    ClinicName = _clinicService.GetClinicName(doctor.CID.Value),
                    VisitDate = record.VisitDate,
                    VisitTime = record.VisitTime,
                    Inspection = record.Inspection,
                    Treatment = record.Treatment,
                    Price = record.Cost,
                });

            }
            return output;
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
