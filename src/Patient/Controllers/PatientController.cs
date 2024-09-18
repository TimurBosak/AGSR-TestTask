using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patient.API.DTO;
using Patient.Services.Interfaces;
using System.Net;

namespace Patient.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;


        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient(PatientDTO patientDTO)
        {
            var patient = new Domain.Models.Patient
            {
                Name = patientDTO.Name,
                Surname = patientDTO.Surname,
                Active = patientDTO.Active,
                BirthDate = patientDTO.BirthDate,
            };

            await _patientService.CreatePatientAsync(patient);

            return Ok();
        }

        /// <summary>Updates the patient.</summary>
        /// <param name="patientDTO">Patient DTO to update existing patient.</param>
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(PatientDTO patientDTO)
        {
            var updatedPatient = new Domain.Models.Patient
            {
                Name = patientDTO.Name,
                Surname = patientDTO.Surname,
                Active = patientDTO.Active,
                BirthDate = patientDTO.BirthDate,
            };

            try
            {
                await _patientService.UpdatePatientAsync(patientDTO.Id, updatedPatient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Gets a patient by its ID.
        /// </summary>
        /// <param name="id">The id of patient.</param>
        [ProducesResponseType(typeof(PatientDTO), (int)HttpStatusCode.OK)]
        [HttpGet("GetPatientById/{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return Ok(patient);
        }
    }
}
