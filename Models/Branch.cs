using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<BranchDepartment> BranchDepartments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Doctor> Doctors { get; set; }
        [JsonIgnore]
        public virtual ICollection<Clinic> Clinics { get; set; }
        [JsonIgnore]
        public virtual ICollection<PatientRecord> PatientRecords { get; set; }


    }
}
