using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class UpdatePasswordDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
       ErrorMessage = "Password must be at least 8 characters long, including at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string NewPassword { get; set; }
      
       
        public string CurrentPassword { get; set; }
    }
}
