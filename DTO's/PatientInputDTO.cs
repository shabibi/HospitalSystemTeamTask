using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalSystemTeamTask.DTO_s
{
    public class PatientInputDTO
    {
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|outlook\.com)$",
        ErrorMessage = "Email must be in the format 'string@gmail.com' or 'string@outlook.com'.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter," +
            " one lowercase letter, one digit")]
        public string Password { get; set; }

        [Required]
        [Range (0, 120 , ErrorMessage ="Age must be grater than 0 ")]
        public int Age { get; set; }
        [Required]
        [RegularExpression("M|F", ErrorMessage = "Gender must be M or F.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Phone number must be exactly 8 digits.")]
        public string Phone {  get; set; }
    }
}
