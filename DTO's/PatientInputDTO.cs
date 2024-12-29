using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class PatientInputDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        [Required]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be Male, Female.")]
        public string Gender { get; set; }
    }
}
