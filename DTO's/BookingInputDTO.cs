using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class BookingInputDTO
    {
        [Required]
        public int PID { get; set; }
        [Required]
        public int CID { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public bool Staus { get; set; } = true;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }

    }
}
