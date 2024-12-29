using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystemTeamTask.Models
{
    [PrimaryKey(nameof(BID), nameof(DepID))]
    public class BranchDepartment
    {
        [ForeignKey("Branch")]
        public int BID { get; set; }
        public Branch Branch { get; set; }

        [ForeignKey("Department")]
        public int DepID { get; set; }
        public Department Department { get; set; }

        public int DepartmentCapacity { get; set; } = 0;
    }
}
