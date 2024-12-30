using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class ClinicInput
    {
        [Required]
        public int DepID { get; set; }

        [Required]
        public int AssignDoctor { get; set; }

        [Required]
        public int BID { get; set; } 

        [Required]
       
        public string ClincName { get; set; }  

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; } 

        [Required]
        public TimeSpan StartTime { get; set; } 

        [Required]
        public TimeSpan EndTime { get; set; } 

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0.")]
        public decimal Cost { get; set; } 

        public bool IsActive { get; set; }
    }
}