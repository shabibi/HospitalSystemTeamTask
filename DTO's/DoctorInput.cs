using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class DoctorInput
    {
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|outlook\.com)$",
        ErrorMessage = "Email must be in the format 'string@gmail.com' or 'string@outlook.com'.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, including at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public int WorkingYear { get; set; }
        [Required]
        public DateOnly JoiningDate { get; set; }
        [Required]
        public int CurrentBrunch { get; set; }
        [Required]
        public int DepID { get; set; }

    }
}
