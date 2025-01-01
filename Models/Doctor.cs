using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.Models
{
    public class Doctor
    {
        [Key]
        [ForeignKey("User")]
        public int DID { get; set; }
        public User User { get; set; }

        [ForeignKey("Department")]
        public int DepId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Clinic")]
        public int? CID { get; set; } 
        public Clinic Clinic { get; set; } 

        [ForeignKey("Branch")]
        public int CurrentBrunch { get; set; }
        public Branch Branch { get; set; }

        public string Level  { get; set; }

        public string Degree { get; set; }

        public int 	WorkingYear {  get; set; }  
        public DateOnly JoiningDate  { get; set; }

        
        public virtual ICollection<PatientRecord> PatientRecords { get; set; }

    }
}
