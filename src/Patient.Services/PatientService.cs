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
                throw new KeyNotFoundException();
            }

            return patient;
        }

        public async Task<IReadOnlyCollection<Domain.Models.Patient>> GetAllPatientsAsync()
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();

            var patients = await patientRepository.GetAllAsync();

            return patients;
        }

        public async Task<IReadOnlyCollection<Domain.Models.Patient>> GetPatiensByConditionAsync(Expression<Func<Domain.Models.Patient, bool>> predicate)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();

            var patients = await patientRepository.GetWhereAsync(predicate);

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

        public async Task UpdatePatientAsync(Domain.Models.Patient patient)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();
            var databasePatient = await patientRepository.GetByIdAsync(patient.Id);

            if (databasePatient == null)
            {
                throw new KeyNotFoundException();
            }

            patientRepository.Update(patient);

            await _uow.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            var patientRepository = _uow.GetRepository<Domain.Models.Patient>();
            var databasePatient = await patientRepository.GetByIdAsync(patientId);

            if (databasePatient == null)
            {
                throw new KeyNotFoundException();
            }

            patientRepository.Remove(databasePatient);

            await _uow.SaveChangesAsync();
        }
    }
}
