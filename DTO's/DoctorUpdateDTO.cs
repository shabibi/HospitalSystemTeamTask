using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class DoctorUpdateDTO
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, including at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required]
        public int DepId { get; set; }
        [Required]
        public int CurrentBrunch { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public int WorkingYear { get; set; }
        public int CID { get; set; }


    }
}
