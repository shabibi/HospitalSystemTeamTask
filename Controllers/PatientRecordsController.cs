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

        [HttpPut("{id}")]
        public IActionResult UpdateRecord(int id, [FromBody] UpdatePatientRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if the record exists
                var existingRecord = _service.GetRecordById(id);
                if (existingRecord == null)
                {
                    return NotFound(new { message = "Patient record not found." });
                }

                // Update the existing entity with the new data
                existingRecord.PID = dto.PID;
                existingRecord.BID = dto.BID;
                existingRecord.DID = dto.DID;
                existingRecord.VisitDate = dto.VisitDate;
                existingRecord.VisitTime = dto.VisitTime;
                existingRecord.Inspection = dto.Inspection;
                existingRecord.Treatment = dto.Treatment;
                existingRecord.Cost = dto.Cost;

                // Call the service to update the record
                _service.UpdateRecord(existingRecord);

                return Ok(new { message = "Patient record updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the patient record.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRecord(int id)
        {
            try
            {
                // Get the record to delete
                var recordToDelete = _service.GetRecordById(id);
                if (recordToDelete == null)
                {
                    return NotFound(new { message = "Patient record not found." });
                }

                // Call the service to delete the record
                _service.DeleteRecord(recordToDelete);

                return Ok(new { message = "Patient record deleted successfully!" });
            }
            catch (Exception ex)
            {
                // Return a generic error message
                return StatusCode(500, new { message = "An error occurred while deleting the patient record.", details = ex.Message });
            }
        }
    }
}
