namespace HospitalSystemTeamTask.DTO_s
{
    public class PatientRecordOutput
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string BranchName { get; set; }
        public string ClinicName { get; set; }

        public DateOnly VisitDate {  get; set; }
        public TimeSpan VisitTime { get; set; }
        public string Inspection { get; set; }
        public string Treatment { get; set; }
        public decimal Price { get; set; }


    }
}
