using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class UserInputDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@([a-zA-Z0-9.-]+\.)?(gmail|outlook|edu)$",
    ErrorMessage = "Email must be in the format 'string@gmail', 'string@outlook', or 'string@edu'.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
    ErrorMessage = "Password must be at least 8 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}






