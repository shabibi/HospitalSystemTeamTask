using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystemTeamTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientRecordsController : ControllerBase
    {
        private readonly IPatientRecordService _service;

        public PatientRecordsController(IPatientRecordService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var records = _service.GetAllRecords()
                .Select(record => new PatientRecordDto
                {
                    RID = record.RID,
                    PID = record.PID,
                    PatientName = record.Patient?.User?.UserName, 
                    BID = record.BID,
                    BranchName = record.Branch?.BranchName, 
                    DID = record.DID,
                    DoctorName = record.Doctor?.User?.UserName, 
                    VisitDate = record.VisitDate,
                    VisitTime = record.VisitTime,
                    Inspection = record.Inspection,
                    Treatment = record.Treatment,
                    Cost = record.Cost
                });

            return Ok(records);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePatientRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var record = new PatientRecord
                {
                    PID = dto.PID,
                    BID = dto.BID,
                    DID = dto.DID,
                    VisitDate = dto.VisitDate,
                    VisitTime = dto.VisitTime,
                    Inspection = dto.Inspection,
                    Treatment = dto.Treatment,
                    Cost = dto.Cost
                };

                _service.CreateRecord(record);

                
                return CreatedAtAction(nameof(GetAll), new { id = record.RID }, new { message = "Patient record added successfully!", record });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the patient record.", details = ex.Message });
            }
        }
    }
}
