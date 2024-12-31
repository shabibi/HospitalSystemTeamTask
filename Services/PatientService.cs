
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using System.Security.Cryptography;


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

        public void UpdatePatientDetails( int UID, PatientUpdate patientInput)
        {
            //get patient data from user table
            var existingUser = _userService.GetUserById(UID);

            //update patient data (only accept to update user name and phone number)
            existingUser.Password = patientInput.Password;
            existingUser.Phone = patientInput.Phone;

            if (existingUser == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            _userService.UpdateUser(existingUser);
        }


        public void AddPatient(PatientInputDTO patientInput)
        {
            if (patientInput == null )
            {
                throw new ArgumentException("Patient or User information is missing.");
            }
            var pass = "Pass1234";
           var hashedPasswor = HashingPassword.Hshing(patientInput.Password);
          
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


