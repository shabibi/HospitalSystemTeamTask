using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalSystemTeamTask.Models
{
    public class Clinic
    {
        [Key]
        public int CID { get; set; }
        [JsonIgnore]
        [ForeignKey("Department")]
        public int 	DepID { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }

        [ForeignKey("Doctor")]
        public int AssignDoctor { get; set; }
        [JsonIgnore]
        public Doctor Doctor { get; set; }

        [ForeignKey("Branch")]
        public int 	BID  { get; set; }
        [JsonIgnore]
        public Branch Branch { get; set; }


        public string ClincName { get; set; } 

        [Required]
        public int Capacity { get; set; }
        [Required]
        public TimeSpan StartTime  { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public int SlotDuration  { get; set; }
        public decimal Cost { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
