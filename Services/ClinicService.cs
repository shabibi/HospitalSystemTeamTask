using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

using HospitalSystemTeamTask.Repositories;


namespace HospitalSystemTeamTask.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepocs _clinicRepo;
        private readonly IDoctorService _doctorService;

        private readonly IBranchDepartmentService _branchDepartmentService;



        public ClinicService(IClinicRepocs clinicRepo,  IDoctorService doctorService, IBranchDepartmentService branchDepartmentService)
        {
            _clinicRepo = clinicRepo;
   
            _doctorService = doctorService;
            _branchDepartmentService = branchDepartmentService;
        }
        public IEnumerable<Clinic> GetAllClinic()
        {
            return _clinicRepo.GetAllClinic();
        }


        public void AddClinic(ClinicInput input)
        {
            if (input == null)
            {
                throw new ArgumentException("Clinic details are required.", nameof(input));
            }

            ValidateClinicInput(input);

            var doctor = GetAndValidateDoctor(input.AssignDoctor);

            // Calculate slot time
            TimeSpan totalDuration = input.EndTime - input.StartTime;
            int slotTime = (int)(totalDuration.TotalMinutes / input.Capacity);

            // Create and add the clinic
            var clinic = new Clinic
            {
                ClincName = input.ClincName.ToLower(),
                Capacity = input.Capacity,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                SlotDuration = slotTime,
                Cost = input.Cost,
                IsActive = input.IsActive,
                DepID = doctor.DepId,
                BID = doctor.CurrentBrunch,
                AssignDoctor = input.AssignDoctor
            };

            _clinicRepo.AddClinic(clinic);

            UpdateBranchDepartmentCapacity(doctor.DepId, doctor.CurrentBrunch, input.Capacity);

            // Assign clinic ID to the doctor
            doctor.CID = clinic.CID;
            _doctorService.UpdateDoctor(doctor);
        }

        private void ValidateClinicInput(ClinicInput input)
        {
            if (input.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than 0.");
            }

            if (input.EndTime <= input.StartTime)
            {
                throw new ArgumentException("End time must be later than start time.");
            }
        }

        private Doctor GetAndValidateDoctor(int doctorId)
        {
            var doctor = _doctorService.GetDoctorById(doctorId);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {doctorId} not found.");
            }

            if (doctor.CID != null)
            {
                throw new InvalidOperationException($"Doctor with ID {doctorId} is already assigned to Clinic ID {doctor.CID}.");
            }

            return doctor;
        }

        private void UpdateBranchDepartmentCapacity(int depId, int branchId, int capacity)
        {
            var branchDepartment = _branchDepartmentService.GetBranchDep(depId, branchId);
            branchDepartment.DepartmentCapacity += capacity;
            _branchDepartmentService.UpdateBranchDepartment(branchDepartment);
        }


        public Clinic GetClinicById(int clinicId)
        {
            // Validate input
            if (clinicId <= 0)
            {
                throw new ArgumentException("Invalid clinic ID. Clinic ID must be greater than 0.");
            }

            // Retrieve clinic by ID
            var clinic = _clinicRepo.GetClinicById(clinicId);

            // Handle clinic not found
            if (clinic == null)
            {
                throw new KeyNotFoundException($"Clinic with ID {clinicId} not found.");
            }

            return clinic;
        }

        public decimal GetPrice(int clinicId)
        {
            var clinic = GetClinicById(clinicId);
            return clinic.Cost;
        }

        //public Clinic GetClinicByDoctor (int doctorId)
        //{
            
        //}
        public Clinic GetClinicByName(string clinicName)
        {
            var clinic = _clinicRepo.GetClinicByName(clinicName.ToLower());
            if (clinic == null)
                throw new KeyNotFoundException($"clinic with name {clinicName} not found.");
            return clinic;
        }

        public IEnumerable<Clinic> GetClinicsByBranchName(string branchName)
        {
            if (string.IsNullOrEmpty(branchName))
            {
                throw new ArgumentException("Branch name is required.");
            }

            var clinics = _clinicRepo.GetClinicsByBranchName(branchName.ToLower());
            if (!clinics.Any())
            {
                throw new KeyNotFoundException($"No clinics found for branch: {branchName}");
            }

            return clinics;
        }

        public IEnumerable<Clinic> GetClinicsByDepartmentId(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0.");
            }

            var clinics = _clinicRepo.GetClinicsByDepartmentID(departmentId);
            if (!clinics.Any())
            {
                throw new KeyNotFoundException($"No clinics found for Department ID: {departmentId}");
            }

            return clinics;
        }

        public void UpdateClinicDetails( int CID,ClinicInput input)
        {
            var existingClinic = _clinicRepo.GetClinicById(CID);
            TimeSpan totalDuration = input.EndTime - input.StartTime;
            if (input.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than 0.");
            }

            int slotTime = (int)(totalDuration.TotalMinutes / input.Capacity);
            var doctor = _doctorService.GetDoctorById(input.AssignDoctor);

            // Map updated properties
            existingClinic.ClincName = input.ClincName;
            existingClinic.Capacity = input.Capacity;
            existingClinic.StartTime = input.StartTime;
            existingClinic.EndTime = input.EndTime;
            existingClinic.SlotDuration = slotTime;
            existingClinic.Cost = input.Cost;
            existingClinic.IsActive = input.IsActive;
            existingClinic.DepID = doctor.DepId;
            existingClinic.BID = doctor.CurrentBrunch;
            existingClinic.AssignDoctor = input.AssignDoctor;
            

            // Persist changes
            _clinicRepo.UpdateClinic(existingClinic);
        }

        public void SetClinicStatus(int clinicId, bool isActive)
        {
            if (clinicId <= 0)
            {
                throw new ArgumentException("Invalid Clinic ID.");
            }

            // Fetch the clinic
            var clinic = _clinicRepo.GetClinicById(clinicId);
            if (clinic == null)
            {
                throw new KeyNotFoundException($"Clinic with ID {clinicId} not found.");
            }

            // Update the IsActive flag
            clinic.IsActive = isActive;

            // Persist changes
            _clinicRepo.UpdateClinic(clinic);
        }

        public string GetClinicName(int cid)
        {
            return _clinicRepo.GetClinicName(cid);
        }
    }

}

