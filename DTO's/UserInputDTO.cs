using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class UserInputDTO
    {
        [Required]
        public string UserName { get; set; }
    
        [Required]
        [RegularExpression(@"^(supperAdmin|admin|doctor|patient)$", ErrorMessage = "Role must be either 'admin' or 'user'.")]

        public string Role { get; set; }
        public string Phone { get; set; }
    }
}






