using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [ForeignKey("Clinic")]
        public int CID { get; set; }
        public Clinic Clinic { get; set; }


        [ForeignKey("Patient")]
        public int? PID  { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
       
      
        public bool Staus  { get; set; }

        public DateTime Date {  get; set; }

        public DateTime? BookingDate  { get; set; }

    }
}
