
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using System.Security.Cryptography;


namespace HospitalSystemTeamTask.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepo _DoctorRepo;
        private readonly IUserService _UserService;


        public DoctorService(IDoctorRepo DoctorRepo, IUserService userService)
        {
            _DoctorRepo = DoctorRepo;
            _UserService = userService;

        }
        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _DoctorRepo.GetAllDoctors();

        }
        public Doctor GetDoctorById(int uid)
        {
            var doctor = _DoctorRepo.GetDoctorById(uid);
            if (doctor == null)
                throw new KeyNotFoundException($"User with ID {uid} not found.");
            return doctor;
        }

        public Doctor GetDoctorByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty.");
            }

            return _DoctorRepo.GetDoctorByEmail(email);
        }

        public bool EmailExists(string email)
        {
            return _DoctorRepo.EmailExists(email);
        }

        public Doctor GetDoctorByName(string docName)
        {
            if (string.IsNullOrEmpty(docName))
            {
                throw new ArgumentException("Doctor name cannot be null or empty.");
            }

            return _DoctorRepo.GetDoctorByName(docName);
        }

        public DoctorOutPutDTO GetDoctorData(string? docName, int? Did)
        {
            Doctor doctor = null;

            // Validate that at least one parameter is provided
            if (string.IsNullOrWhiteSpace(docName) && !Did.HasValue)
                throw new ArgumentException("Either docName or doctor ID must be provided.");

            // Retrieve doctor based on docName 
            if (!string.IsNullOrEmpty(docName))
                doctor = GetDoctorByName(docName);

            // Retrieve doctor based on Did
            if (Did.HasValue)
                doctor = GetDoctorById(Did.Value);


            if (doctor == null)
                throw new KeyNotFoundException($"doctor not found.");

            var outputData = new DoctorOutPutDTO
            {
                UID = doctor.DID,
                CurrentBrunch = doctor.CurrentBrunch,
                Level = doctor.Level,
                Degree = doctor.Degree,
                WorkingYear = doctor.WorkingYear,
                JoiningDate = doctor.JoiningDate // Get the current date

            };

            return (outputData);
        }

        public void AddDoctor(DoctorOutPutDTO input)
        {
            if (input == null)
                throw new ArgumentException("Doctor information is missing.");

            if (input.UID <= 0)
                throw new ArgumentException("Invalid doctor ID.");

            // Retrieve the user by ID
            var user = _UserService.GetUserById(input.UID);
            if (user == null)
                throw new ArgumentException($"No user found with ID {input.UID}.");

            // Ensure the user's role is 'doctor'
            if (!user.Role.Equals("doctor", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("The provided user ID does not belong to a doctor.");

            // Create the Doctor entity and associate it with the User
            var doctor = new Doctor
            {
                DID = user.UID, // Link User to Doctor
                CurrentBrunch = input.CurrentBrunch,
                DepId = input.DepId,
                Level = input.Level,
                Degree = input.Degree,
                WorkingYear = input.WorkingYear,
                JoiningDate = DateOnly.FromDateTime(DateTime.Now), // Automatically set the current date
                CID = input.CID,
            };



            // Save the doctor
            _DoctorRepo.AddDoctor(doctor);



        }


        public IEnumerable<DoctorOutPutDTO> GetDoctorsByBranchName(string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
                throw new ArgumentException("Branch name is required.");

            // Fetch doctors from the repository
            var doctors = _DoctorRepo.GetDoctorByBranchName(branchName);

            // Transform to DTOs
            var doctorDtos = doctors.Select(doctor => new DoctorOutPutDTO
            {
                UID = doctor.DID,
                CurrentBrunch = doctor.CurrentBrunch,
                Level = doctor.Level,
                Degree = doctor.Degree,
                WorkingYear = doctor.WorkingYear,
                JoiningDate = doctor.JoiningDate,
                DepId = doctor.DepId
            });

            return doctorDtos;
        }

        public IEnumerable<DoctorOutPutDTO> GetDoctorsByDepartmentName(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Department name is required.");

            // Retrieve doctors from the repository
            var doctors = _DoctorRepo.GetDoctorsByDepartmentName(departmentName);

            if (doctors == null || !doctors.Any())
                throw new KeyNotFoundException($"No doctors found for department '{departmentName}'.");

            // Transform to DTOs
            var doctorDtos = doctors.Select(doctor => new DoctorOutPutDTO
            {
                UID = doctor.DID,
                CurrentBrunch = doctor.CurrentBrunch,
                Level = doctor.Level,
                Degree = doctor.Degree,
                WorkingYear = doctor.WorkingYear,
                JoiningDate = doctor.JoiningDate,
                DepId = doctor.DepId
            });

            return doctorDtos;
        }

        public void UpdateDoctorDetails(int UID, int DID, DoctorUpdateDTO input)
        {
            if (input == null)
            {
                throw new ArgumentException("Doctor update details are required.");
            }

            // Get the user associated with the doctor
            var existingUser = _UserService.GetUserById(UID);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("Doctor user not found.");
            }

            // Update user details
            existingUser.Password = input.Password;
            existingUser.Phone = input.Phone;
            _UserService.UpdateUser(existingUser);

            // Get doctor entity
            var existingDoctor = _DoctorRepo.GetDoctorById(DID);
            if (existingDoctor == null)
            {
                throw new KeyNotFoundException("Doctor entity not found.");
            }

            // Update doctor details
            existingDoctor.CurrentBrunch = input.CurrentBrunch;
            existingDoctor.Level = input.Level;
            existingDoctor.Degree = input.Degree;
            existingDoctor.DepId = input.DepId;
            existingDoctor.WorkingYear = input.WorkingYear;
            existingDoctor.CID = input.CID;

            _DoctorRepo.UpdateDoctor(existingDoctor);
        }

        public void UpdateDoctor(Doctor doctor)
        {

            var existingDoctor = _DoctorRepo.GetDoctorById(doctor.DID);
            if (existingDoctor == null)
                throw new KeyNotFoundException($"Product with ID {doctor.DID} not found.");

            _DoctorRepo.UpdateDoctor(doctor);

        }


    }
}


    
