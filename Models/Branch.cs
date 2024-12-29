using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.Models
{
    public class Branch
    {
        [Key]
        public int BID  { get; set; }

        [Required]
        public string BranchName  { get; set; }

        [Required]
        public string Location  { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<BranchDepartment> BranchDepartments { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<PatientRecord> PatientRecords { get; set; }


    }
}
