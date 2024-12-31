namespace HospitalSystemTeamTask.DTO_s
{
    public class UpdatePatientRecordDto
    {
        public int PatientRecordID { get; set; }
        public int PID { get; set; }
        public int BID { get; set; }
        public int DID { get; set; }
        public DateOnly VisitDate { get; set; }
        public TimeSpan VisitTime { get; set; }
        public string Inspection { get; set; }
        public string Treatment { get; set; }
        public decimal Cost { get; set; }
    }
}
