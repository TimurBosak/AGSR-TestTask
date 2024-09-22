using Microsoft.EntityFrameworkCore;
using Patient.Repositories.Interfaces;
using Patient.Services.Interfaces;
using System.Linq.Expressions;

namespace Patient.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _uow;


        public PatientService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<Domain.Models.Patient> GetPatientByIdAsync(Guid patientId)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();

            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient with such id is not existing");
            }

            return patient;
        }

        public async Task<IReadOnlyCollection<Domain.Models.Patient>> GetAllPatientsAsync()
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();

            var patients = await patientRepository.GetAllAsync();

            return patients;
        }

        public async Task CreatePatientAsync(Domain.Models.Patient patient)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();

            patientRepository.Add(patient);

            await _uow.SaveChangesAsync();
        }

        public async Task CreateMultiplePatientsAsync(IEnumerable<Domain.Models.Patient> patients)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();

            patientRepository.AddRange(patients);

            await _uow.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Guid patientId, Domain.Models.Patient updatedModel)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();
            var databasePatient = await patientRepository.GetByIdAsync(patientId);

            if (databasePatient == null)
            {
                throw new KeyNotFoundException("Patient with such id is not existing");
            }

            databasePatient.Name = updatedModel.Name;
            databasePatient.Surname = updatedModel.Surname;
            databasePatient.BirthDate = updatedModel.BirthDate;
            databasePatient.Active = updatedModel.Active;

            patientRepository.Update(databasePatient);

            await _uow.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();
            var databasePatient = await patientRepository.GetByIdAsync(patientId);

            if (databasePatient == null)
            {
                throw new KeyNotFoundException("Patient with such id is not existing");
            }

            patientRepository.Remove(databasePatient);

            await _uow.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Domain.Models.Patient>> GetPatientsByDateFilterAsync(DateRangeFilter filter)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();
            var query = patientRepository.GetQuery();
            var edgeCaseQuery = patientRepository.GetQuery();

            var startAfterList = new List<DateTime>();
            var endBeforeList = new List<DateTime>();

            foreach (var dateFilter in filter.DateFilters)
            {
                if (IsOperator(dateFilter))
                {
                    var operatorPart = dateFilter.Substring(0, 2).ToLower();
                    var datePart = dateFilter.Substring(2);
                    DateTime dateValue;

                    if (datePart.Length == 4 && int.TryParse(datePart, out var year))
                    {
                        dateValue = new DateTime(year, 1, 1);
                        ApplyDateFilter(ref query, operatorPart, dateValue, startAfterList, endBeforeList);
                    }
                    else if (datePart.Length == 7 && datePart.Contains('-'))
                    {
                        var parts = datePart.Split('-');
                        if (parts.Length == 2 && int.TryParse(parts[0], out year) && int.TryParse(parts[1], out var month) && month >= 1 && month <= 12)
                        {
                            dateValue = new DateTime(year, month, 1);
                            ApplyDateFilter(ref query, operatorPart, dateValue, startAfterList, endBeforeList);
                        }
                    }
                    else if (DateTime.TryParse(datePart, out dateValue))
                    {
                        ApplyDateFilter(ref query, operatorPart, dateValue, startAfterList, endBeforeList);
                    }
                }
                else if (dateFilter.Length == 4 && int.TryParse(dateFilter, out var year))
                {
                    var startOfYear = new DateTime(year, 1, 1);
                    var endOfYear = new DateTime(year, 12, 31, 23, 59, 59);
                    query = query.Where(p => p.BirthDate >= startOfYear && p.BirthDate <= endOfYear);
                }
                else if (dateFilter.Length == 7 && dateFilter.Contains('-'))
                {
                    var parts = dateFilter.Split('-');
                    if (parts.Length == 2 && int.TryParse(parts[0], out year) && int.TryParse(parts[1], out var month) && month >= 1 && month <= 12)
                    {
                        var startOfMonth = new DateTime(year, month, 1);
                        var endOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
                        query = query.Where(p => p.BirthDate >= startOfMonth && p.BirthDate <= endOfMonth);
                    }
                }
            }

            // If we have any SA or EB in our params update query to respect this
            if (startAfterList.Count > 0)
            {
                query = edgeCaseQuery.Where(p => p.BirthDate > startAfterList.Max());
            }

            if (endBeforeList.Count > 0)
            {
                query = edgeCaseQuery.Where(p => p.BirthDate < endBeforeList.Min());
            }

            var patients = await query.ToListAsync();
            return patients;
        }


        private static bool IsOperator(string dateFilter)
        {
            return dateFilter.Length >= 2 && char.IsLetter(dateFilter[0]) && char.IsLetter(dateFilter[1]);
        }

        private static void ApplyDateFilter(ref IQueryable<Domain.Models.Patient> query, string operatorPart, DateTime dateValue, List<DateTime> startAfterList, List<DateTime> endBeforeList)
        {
            switch (operatorPart)
            {
                case "sa":
                    startAfterList.Add(dateValue);
                    break;
                case "eb":
                    endBeforeList.Add(dateValue);
                    break;
                case "gt":
                    query = query.Where(p => p.BirthDate > dateValue);
                    break;
                case "lt":
                    query = query.Where(p => p.BirthDate < dateValue);
                    break;
                case "ge":
                    query = query.Where(p => p.BirthDate >= dateValue);
                    break;
                case "le":
                    query = query.Where(p => p.BirthDate <= dateValue);
                    break;
                case "ap":
                case "eq":
                    query = query.Where(p => p.BirthDate.Date == dateValue.Date);
                    break;
                case "ne":
                    query = query.Where(p => p.BirthDate.Date != dateValue.Date);
                    break;
            }
        }
    }
}
