using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalSystemTeamTask.Models
{
    public class User
    {
        [Key]
        public int UID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        public string Password { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Doctor> Doctors { get;set; }
    }
}
