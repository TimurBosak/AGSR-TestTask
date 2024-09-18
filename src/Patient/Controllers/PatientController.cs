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

        /// <summary>
        /// Creates a patient.
        /// </summary>
        /// <param name="patientDTO">Patient DTO for creating new patient</param>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient(PatientDTO patientDTO)
        {
            var patient = new Domain.Models.Patient
            {
                Name = patientDTO.Name,
                Surname = patientDTO.Surname,
                Active = patientDTO.Active,
                Gender = (Domain.Enums.Gender)patientDTO.Gender,
                BirthDate = patientDTO.BirthDate,
            };

            await _patientService.CreatePatientAsync(patient);

            return Ok();
        }

        /// <summary>Updates the patient.</summary>
        /// <param name="id">Patient id using to find db patient.</param>
        /// <param name="newName">New patient name.</param>
        /// <param name="newSurname">New patient surname.</param>
        /// <param name="newActive">New patient active status.</param>
        /// <param name="newBirthDate">New patient birthdate.</param>
        /// <param name="newGender">New patient gender.</param>
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(Guid id, string newName, string newSurname, bool newActive, DateTime newBirthDate, Gender newGender)
        {
            var updatedPatient = new Domain.Models.Patient
            {
                Name = newName,
                Surname = newSurname,
                Active = newActive,
                BirthDate = newBirthDate,
                Gender = (Domain.Enums.Gender)newGender
            };

            try
            {
                await _patientService.UpdatePatientAsync(id, updatedPatient);
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
            Domain.Models.Patient patient;
            try
            {
                patient = await _patientService.GetPatientByIdAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(patient);
        }
    }
}
