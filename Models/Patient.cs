using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalSystemTeamTask.Models
{
    public class Patient
    {
        [Key]
        [ForeignKey("User")]
        public int PID  { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        public virtual ICollection<PatientRecord> PatientRecords { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
