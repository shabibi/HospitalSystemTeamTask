using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class ClinicInput
    {
        [Required]
        public int DepID { get; set; } // Department ID

        [Required]
        public int AssignDoctor { get; set; } 

        [Required]
        public int BID { get; set; } // Branch ID

        [Required]
       
        public string ClincName { get; set; } // 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; } // Clinic Capacity

        [Required]
        public TimeSpan StartTime { get; set; } // Clinic Start Time

        [Required]
        public TimeSpan EndTime { get; set; } // Clinic End Time

        [Required]
        [Range(1, 30, ErrorMessage = "Slot duration must be between 1 and 120 minutes.")]
        public int SlotDuration { get; set; } // Duration of each slot in minutes

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0.")]
        public decimal Cost { get; set; } 

        public bool IsActive { get; set; }
    }
}