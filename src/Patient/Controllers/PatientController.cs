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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatients([FromQuery] DateRangeFilterDTO filter)
        {
            var query = _patientService.GetPatientsQuery().AsQueryable();

            foreach (var dateFilter in filter.DateFilters)
            {
                if (IsOperator(dateFilter))
                {
                    var operatorPart = dateFilter.Substring(0, 2).ToLower();
                    var datePart = dateFilter.Substring(2);

                    if (DateTime.TryParse(datePart, out var dateValue))
                    {
                        switch (operatorPart)
                        {
                            case "eq": // Equal
                                query = query.Where(p => p.BirthDate.Date == dateValue.Date);
                                break;
                            case "ne": // Not equal
                                query = query.Where(p => p.BirthDate.Date != dateValue.Date);
                                break;
                            case "lt": // Less than
                                query = query.Where(p => p.BirthDate < dateValue);
                                break;
                            case "gt": // Greater than
                                query = query.Where(p => p.BirthDate > dateValue);
                                break;
                            case "ge": // Greater than or equal
                                query = query.Where(p => p.BirthDate >= dateValue);
                                break;
                            case "le": // Less than or equal
                                query = query.Where(p => p.BirthDate <= dateValue);
                                break;
                            case "sa": // Starts after (exclusive)
                                query = query.Where(p => p.BirthDate > dateValue);
                                break;
                            case "eb": // Ends before (exclusive)
                                query = query.Where(p => p.BirthDate < dateValue);
                                break;
                            case "ap": // Approximately matches (exact date)
                                query = query.Where(p => p.BirthDate.Date == dateValue.Date);
                                break;
                        }
                    }
                }
                else if (dateFilter.Length == 4 && int.TryParse(dateFilter, out var year))
                {
                    var startOfYear = new DateTime(year, 1, 1);
                    var endOfYear = new DateTime(year, 12, 31, 23, 59, 59);

                    query = query.Where(p => p.BirthDate >= startOfYear && p.BirthDate <= endOfYear);
                }
                else if (dateFilter.Length == 7)
                {
                    var parts = dateFilter.Split('-');
                    if (parts.Length == 2 && int.TryParse(parts[0], out year) && int.TryParse(parts[1], out var month))
                    {
                        var startOfMonth = new DateTime(year, month, 1);
                        var endOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

                        query = query.Where(p => p.BirthDate >= startOfMonth && p.BirthDate <= endOfMonth);
                    }
                }
            }

            var patients = query.ToList();
            return Ok(patients);
        }


        private bool IsOperator(string dateFilter)
        {
            return dateFilter.Length >= 2 && char.IsLetter(dateFilter[0]) && char.IsLetter(dateFilter[1]);
        }
    }
}
