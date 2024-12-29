using System.ComponentModel.DataAnnotations;

namespace HospitalSystemTeamTask.Models
{
    public class Department
    {
        [Key]
        public int 	DepID { get; set; }

        [Required]
        public string DepartmentName  { get; set; }

       
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<BranchDepartment> BranchDepartments { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection <Clinic> Clinics { get; set; }

    }
}
