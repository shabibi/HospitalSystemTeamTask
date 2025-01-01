﻿using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;

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

        public PatientRecord GetRecordById(int id)
        {
            return _repository.GetById(id);
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
