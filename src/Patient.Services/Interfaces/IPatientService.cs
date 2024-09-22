using PatientModel = Patient.Domain.Models.Patient;

namespace Patient.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PatientModel> GetPatientByIdAsync(Guid patientId);

        Task<IReadOnlyCollection<PatientModel>> GetAllPatientsAsync();

        Task CreatePatientAsync(PatientModel patient);

        Task CreateMultiplePatientsAsync(IEnumerable<PatientModel> patients);

        Task UpdatePatientAsync(Guid patientId, PatientModel updatedModel);

        Task DeletePatientAsync(Guid patientId);

        Task<IReadOnlyCollection<Domain.Models.Patient>> GetPatientsByDateFilterAsync(DateRangeFilter filter);
    }
}
