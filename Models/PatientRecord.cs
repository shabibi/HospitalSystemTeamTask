using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystemTeamTask.Models
{
    public class PatientRecord
    {
        [Key]
        public int RID  { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PID { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Branch")]
        public int 	BID  { get; set; }
        public Branch Branch { get; set; }

        [ForeignKey("Doctor")]
        public int DID { get; set; }
        public Doctor Doctor { get; set; }

        public DateOnly VisitDate { get; set; }

        public TimeSpan VisitTime { get; set; }

        public string Inspection {  get; set; }
        public string Treatment  { get; set; }

        public Decimal Cost { get; set; }

    }
}
