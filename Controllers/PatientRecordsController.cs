using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Helper;
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


        [HttpPost("CreatePatientRecord")]
        public IActionResult Create([FromBody] PatientRecordInputDTO inputRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string token = JwtHelper.ExtractToken(Request);
                int doctorId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check if the user's role allows them to perform this action
                if (userRole == null || (userRole != "doctor"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }
                _service.CreateRecord(inputRecord, doctorId);


                return CreatedAtAction(nameof(GetAll), new { message = "Patient record added successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the patient record.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                // Get the patient record by ID
                var record = _service.GetRecordById(id);

                if (record == null)
                {
                    return NotFound(new { message = "Patient record not found." });
                }

                // Map PatientRecord to PatientRecordDto
                //var recordDto = new PatientRecordDto
                //{
                //    PatientRecordID = record.RID,
                //    PID = record.PID,
                //    PatientName = record.Patient?.User?.UserName,
                //    BID = record.BID,
                //    BranchName = record.Branch?.BranchName,
                //    DID = record.DID,
                //    DoctorName = record.Doctor?.User?.UserName,
                //    VisitDate = record.VisitDate,
                //    VisitTime = record.VisitTime,
                //    Inspection = record.Inspection,
                //    Treatment = record.Treatment,
                //    Cost = record.Cost
                //};

                // Return the record if found
                return Ok();
            }
            catch (Exception ex)
            {
                // Return a generic error message if something goes wrong
                return StatusCode(500, new { message = "An error occurred while retrieving the patient record.", details = ex.Message });
            }
        }



        [HttpGet("GetAllRecords")]
        public IActionResult GetAll()
        {
            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");

                // Check if the user's role allows them to perform this action
                if (userRole == null || (userRole != "admin" && userRole != "supperAdmin"))
                {
                    return BadRequest(new { message = "You are not authorized to perform this action." });
                }
                return Ok(_service.GetAllRecords());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving products. {(ex.Message)}");

            }
        }


        [HttpGet("by-doctor/{doctorId}")]
        public IActionResult GetByDoctorId(int doctorId)
        {
            try
            {
                // Retrieve patient records by doctor ID
                //var records = _service.GetRecordsByDoctorId(doctorId)
                    //.Select(record => new PatientRecordDto
                    //{
                    //    PatientRecordID = record.RID,
                    //    PID = record.PID,
                    //    PatientName = record.Patient?.User?.UserName,
                    //    BID = record.BID,
                    //    BranchName = record.Branch?.BranchName,
                    //    DID = record.DID,
                    //    DoctorName = record.Doctor?.User?.UserName,
                    //    VisitDate = record.VisitDate,
                    //    VisitTime = record.VisitTime,
                    //    Inspection = record.Inspection,
                    //    Treatment = record.Treatment,
                    //    Cost = record.Cost
                    //});

                //if (!records.Any())
                //{
                //    return NotFound(new { message = "No patient records found for the specified doctor." });
                //}

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving patient records.", details = ex.Message });
            }
        }


        [HttpGet("by-branch/{branchId}")]
        public IActionResult GetByBranchId(int branchId)
        {
            try
            {
                // Retrieve patient records by branch ID
                //var records = _service.GetRecordsByBranchId(branchId)
                    //.Select(record => new PatientRecordDto
                    //{
                    //    PatientRecordID = record.RID,
                    //    PID = record.PID,
                    //    PatientName = record.Patient?.User?.UserName,
                    //    BID = record.BID,
                    //    BranchName = record.Branch?.BranchName,
                    //    DID = record.DID,
                    //    DoctorName = record.Doctor?.User?.UserName,
                    //    VisitDate = record.VisitDate,
                    //    VisitTime = record.VisitTime,
                    //    Inspection = record.Inspection,
                    //    Treatment = record.Treatment,
                    //    Cost = record.Cost
                    //});

                //if (!records.Any())
                //{
                //    return NotFound(new { message = "No patient records found for the specified branch." });
                //}

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving patient records.", details = ex.Message });
            }
        }

        [HttpPatch("{id}")]
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
