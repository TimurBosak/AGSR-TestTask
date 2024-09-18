using System.Linq.Expressions;
using PatientModel = Patient.Domain.Models.Patient;

namespace Patient.Services.Interfaces

{
    public interface IPatientService
    {
        public Task<PatientModel> GetPatientByIdAsync(Guid patientId);

        public Task<IReadOnlyCollection<PatientModel>> GetAllPatientsAsync();

        public Task<IReadOnlyCollection<PatientModel>> GetPatiensByConditionAsync(Expression<Func<PatientModel, bool>> predicate);

        public Task CreatePatientAsync(PatientModel patient);

        public Task CreateMultiplePatientsAsync(IEnumerable<PatientModel> patients);

        public Task UpdatePatientAsync(PatientModel patient);

        public Task DeletePatientAsync(Guid patientId);
    }
}
