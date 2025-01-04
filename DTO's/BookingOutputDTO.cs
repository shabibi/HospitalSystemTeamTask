namespace HospitalSystemTeamTask.DTO_s
{
    public class BookingOutputDTO
    {
        public int CID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Staus { get; set; }
        public int? PID { get; set; }
        public DateTime? BookingDate { get; set; }

    }
}
