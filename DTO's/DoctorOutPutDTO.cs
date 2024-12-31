using System.Text.Json.Serialization;

namespace HospitalSystemTeamTask.DTO_s
{
    public class DoctorOutPutDTO
    {
        public int UID { get; set; }
    
        public int CurrentBrunch { get; set; }
        public string Level { get; set; }

        public string Degree { get; set; }

        public int WorkingYear { get; set; }
        [JsonIgnore]
        public DateOnly JoiningDate { get; set; }
        public int DepId { get; set; }
    }
}
