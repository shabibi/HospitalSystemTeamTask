﻿
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using HospitalSystemTeamTask;

namespace HospitalSystemTeamTask.Repositories
{
    public class PatientRepo : IPatientRepo
    {
        public ApplicationDbContext _context;
        public PatientRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get All Patients
        public IEnumerable<Patient> GetAllPatients()
        {
            try
            {
                return _context.Patients.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        //Get Patients by id
        public Patient GetPatientsById(int Pid)
        {
            try
            {
                return _context.Patients.FirstOrDefault(u => u.PID == Pid);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }


    }
}
