namespace HospitalSystemTeamTask.DTO_s
{
    public class PatientRecordDto
    {
        public int RID { get; set; }
        public int PID { get; set; }
        public string PatientName { get; set; }
        public int BID { get; set; }
        public string BranchName { get; set; }
        public int DID { get; set; }
        public string DoctorName { get; set; }
        public DateOnly VisitDate { get; set; }
        public TimeSpan VisitTime { get; set; }
        public string Inspection { get; set; }
        public string Treatment { get; set; }
        public decimal Cost { get; set; }
    }
}
