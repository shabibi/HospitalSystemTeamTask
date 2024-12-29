
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;


namespace HospitalSystemTeamTask.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepo _PatientRepo;
        private readonly IUserService _userService;

        public PatientService(IPatientRepo PatientRepo, IUserService userService)
        {
            _PatientRepo = PatientRepo;
            _userService = userService;
        }



        public IEnumerable<Patient> GetAllPatients()
        {
            return _PatientRepo.GetAllPatients();

        }

        public Patient GetPatientById(int Pid)
        {
            var patient = _PatientRepo.GetPatientsById(Pid);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID {Pid} not found.");
            return patient;
        }

        public void UpdatePatientDetails(Patient updatedPatient)
        {
            // Fetch the existing patient
            var existingPatient = _PatientRepo.GetPatientsById(updatedPatient.PID);

            if (existingPatient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            // Update patient-specific fields
            existingPatient.Age = updatedPatient.Age;
            existingPatient.Gender = updatedPatient.Gender;

            // Update user-specific fields if provided
            if (updatedPatient.User != null)
            {
                if (!string.IsNullOrEmpty(updatedPatient.User.UserName))
                {
                    existingPatient.User.UserName = updatedPatient.User.UserName;
                }

                if (!string.IsNullOrEmpty(updatedPatient.User.Email))
                {
                    existingPatient.User.Email = updatedPatient.User.Email;
                }

                if (!string.IsNullOrEmpty(updatedPatient.User.Password))
                {
                    existingPatient.User.Password = BCrypt.Net.BCrypt.HashPassword(updatedPatient.User.Password);
                }
            }

            // Save changes via the repository
            _PatientRepo.UpdatePatient(existingPatient);
        }


        public void AddPatient(PatientInputDTO patientInput)
        {
            if (patientInput == null )
            {
                throw new ArgumentException("Patient or User information is missing.");
            }

            //// Validate User fields
            //if (string.IsNullOrEmpty(patientInput.User.UserName) || string.IsNullOrEmpty(patient.User.Email) || string.IsNullOrEmpty(patient.User.Password))
            //{
            //    throw new ArgumentException("UserName, Email, and Password are required for the User.");
            //}

            // Hash the user's password

           var hashedPasswor = BCrypt.Net.BCrypt.HashPassword(patientInput.Password);
            // Validate Patient fields
            //if (string.IsNullOrEmpty(patient.Gender) || patient.Age <= 0)
            //{
            //    throw new ArgumentException("Gender and valid Age are required for the Patient.");
            //}

            var user = new User { 
                UserName = patientInput.UserName, 
                Password = hashedPasswor,
                Email = patientInput.Email, 
                Role = "patient" ,
                IsActive = true,
                Phone = patientInput.Phone
            };
            _userService.AddUser(user);
            // Delegate to repository
           
            var patient = new Patient {PID= user.UID, Age = patientInput.Age, Gender = patientInput.Gender };
            _PatientRepo.AddPatient(patient);
        }

    }


}


