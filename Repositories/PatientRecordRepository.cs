using HospitalSystemTeamTask.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HospitalSystemTeamTask.Repositories
{
    public class PatientRecordRepository : IPatientRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PatientRecord> GetAll()
        {
            return _context.PatientRecords.Include(pr => pr.Patient).Include(pr => pr.Doctor).Include(pr => pr.Branch).ToList();
        }

        public PatientRecord GetById(int id)
        {
            return _context.PatientRecords.Include(pr => pr.Patient).Include(pr => pr.Doctor).Include(pr => pr.Branch).FirstOrDefault(pr => pr.RID == id);
        }
        public void Add(PatientRecord record)
        {
            _context.PatientRecords.Add(record);
            _context.SaveChanges();
        }
        public void UpdateRecord(PatientRecord record)
        {
            _context.PatientRecords.Update(record);
            _context.SaveChanges();
        }

        public void DeleteRecord(PatientRecord record)
        {
            _context.PatientRecords.Remove(record);
            _context.SaveChanges();
        }

    }
}
