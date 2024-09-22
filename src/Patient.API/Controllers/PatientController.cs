using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Patient.API.DTO;
using Patient.Services;
using Patient.Services.Interfaces;
using System.Net;

namespace Patient.API.Controllers
{
    /// <summary>
    /// Controller for crud operations over patient model.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _maper;


        public PatientController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _maper = mapper;
        }

        /// <summary>
        /// Creates a patient.
        /// </summary>
        /// <param name="patientDTO">Patient DTO for creating new patient</param>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient(PatientDTO patientDTO)
        {
            var patient = _maper.Map<Domain.Models.Patient>(patientDTO);

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
        public async Task<IActionResult> UpdatePatient(Guid id, string newName, string newSurname, bool newActive, DateTime newBirthDate, GenderDTO newGender)
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("GetPatientById/{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            PatientDTO patientDto;
            try
            {
                patientDto = _maper.Map<PatientDTO>(await _patientService.GetPatientByIdAsync(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(patientDto);
        }

        /// <summary>
        /// Delete patinet by id.
        /// </summary>
        /// <param name="id">Id of patient.</param>
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("DeletePatientById/{id}")]
        public async Task<IActionResult> DeletePatientById(Guid id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Get's all existing recipients
        /// </summary>
        [ProducesResponseType(typeof(IReadOnlyCollection<PatientDTO>), (int)HttpStatusCode.OK)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patientsDto = _maper.Map<List<PatientDTO>>(await _patientService.GetAllPatientsAsync());

            return Ok(patientsDto);
        }

        /// <summary>
        /// Get's patients based on various date filters 
        /// </summary>
        /// <param name="filterDto">List of string params which parssed to operator+date/date</param>
        [ProducesResponseType(typeof(IReadOnlyCollection<PatientDTO>), (int)HttpStatusCode.OK)]
        [HttpGet("GetPatientsByDate")]
        public async Task<IActionResult> GetPatientsByDate([FromQuery] DateRangeFilterDTO filterDto)
        {
            var filter = new DateRangeFilter
            {
                DateFilters = filterDto.DateFilters.Select(f => f).ToList()
            };

            var patientsDto = _maper.Map<List<PatientDTO>>(await _patientService.GetPatientsByDateFilterAsync(filter));

            return Ok(patientsDto);
        }

        /// <summary>
        /// Create range of patients
        /// </summary>
        /// <param name="patients">Collection of DTOs of patients to create</param>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost("CreateRangePatients")]
        public async Task<IActionResult> AddRangePatients(IEnumerable<PatientDTO> patients)
        {
            var patientsRangeToAdd = new List<Domain.Models.Patient>();

            foreach(var patientDto in patients)
            {
                var patient = _maper.Map<Domain.Models.Patient>(patientDto);
                patientsRangeToAdd.Add(patient);
            }

            await _patientService.CreateMultiplePatientsAsync(patientsRangeToAdd);

            return Ok();
        }
    }
}
