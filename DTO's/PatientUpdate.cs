
using System.ComponentModel.DataAnnotations;
namespace HospitalSystemTeamTask.DTO_s
{
    public class PatientUpdate
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, including at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}
