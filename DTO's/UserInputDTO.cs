using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class UserInputDTO
    {
        [Required]
        public string UserName { get; set; }
    
        [Required]
        [RegularExpression(@"^(superAdmin|admin|doctor)$", ErrorMessage = "Role must be either 'admin' , 'superAdmin' or 'doctor'.")]

        public string Role { get; set; }
        public string Phone { get; set; }
    }
}






