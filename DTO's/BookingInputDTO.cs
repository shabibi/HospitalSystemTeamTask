using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.DTO_s
{
    public class BookingInputDTO
    {
        [Required]
        public int CID { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
      
        [Required]
        public DateTime Date { get; set; }
   
    }
}
