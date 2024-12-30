
using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using System.Security.Cryptography;


namespace HospitalSystemTeamTask.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepo _DoctorRepo;


        public DoctorService(IDoctorRepo DoctorRepo)
        {
            _DoctorRepo = DoctorRepo;

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



    } }