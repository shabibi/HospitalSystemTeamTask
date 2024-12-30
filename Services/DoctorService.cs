
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


        } }