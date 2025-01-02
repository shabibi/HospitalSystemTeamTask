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


        [HttpGet("GetRecords")]
        public IActionResult GetRecords(int? RecordId, int? patientId, int? doctorId, int? branchId)
        {
            try
            {
                // Ensure only one parameter is provided
                int providedParametersCount = new[] { RecordId, patientId, doctorId, branchId }.Count(p => p.HasValue);
                if (providedParametersCount > 1)
                {
                    throw new ArgumentException("Only one parameter can be provided at a time.");
                }
                // Extract JWT token and claims
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                int userId =int.Parse( JwtHelper.GetClaimValue(token, "sub"));

                // Validate patient access based on user role
                if (userRole.Equals("patient", StringComparison.OrdinalIgnoreCase) && patientId.HasValue && userId != patientId)
                {
                    return Forbid("You are not authorized to access other patients' records." );
                }
                // Validate doctor access based on user role
                if (userRole.Equals("Doctor", StringComparison.OrdinalIgnoreCase) && doctorId.HasValue && userId != doctorId)
                {
                    return Forbid("You are not authorized to access other doctors' records." );
                }

                //Retrieve patient records by doctor ID
                var records = _service.GetRecords(RecordId, patientId,  doctorId, branchId);

                // Check if no records were found
                if (records == null || !records.Any())
                {
                    return NotFound(new { message = "No records found matching the specified criteria." });
                }

                return Ok(records);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving patient records.", details = ex.Message });
            }
        }



        [HttpPatch("UpdateRecord")]
        public IActionResult UpdateRecord(int RecordID, string? treatment, string? inspection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string token = JwtHelper.ExtractToken(Request);
                var userRole = JwtHelper.GetClaimValue(token, "unique_name");
                int userId = int.Parse(JwtHelper.GetClaimValue(token, "sub"));

                
               _service.UpdateRecord(RecordID,treatment,inspection,userId);

                return Ok(new { message = "Patient record updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the patient record.", details = ex.Message });
            }
        }

        //[HttpDelete("{id}")]
        //public IActionResult DeleteRecord(int id)
        //{
        //    try
        //    {
        //        // Get the record to delete
        //        var recordToDelete = _service.GetRecordById(id);
        //        if (recordToDelete == null)
        //        {
        //            return NotFound(new { message = "Patient record not found." });
        //        }

        //        // Call the service to delete the record
        //        _service.DeleteRecord(recordToDelete);

        //        return Ok(new { message = "Patient record deleted successfully!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return a generic error message
        //        return StatusCode(500, new { message = "An error occurred while deleting the patient record.", details = ex.Message });
        //    }
        //}
    }
}
