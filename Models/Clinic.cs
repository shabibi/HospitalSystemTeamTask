using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.Models
{
    public class Clinic
    {
        [Key]
        public int CID { get; set; }
        
        [ForeignKey("Department")]
        public int 	DepID { get; set; }
        public Department Department { get; set; }

       
        [ForeignKey("Doctor")]
        public int AssignDoctor { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Branch")]
        public int 	BID  { get; set; }
        public Branch Branch { get; set; }

        [Required]
        public int Capacity { get; set; }
        [Required]
        public TimeSpan StartTime  { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public int SlotDuration  { get; set; }
        public decimal Cost { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
