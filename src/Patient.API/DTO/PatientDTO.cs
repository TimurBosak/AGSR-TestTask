using AutoMapper;
using Patient.API.Mapping;

namespace Patient.API.DTO
{
    public class PatientDTO : IMapWith<Domain.Models.Patient>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public GenderDTO Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Patient, PatientDTO>()
                .ForMember(patientDto => patientDto.Name, opt => opt.MapFrom(patient => patient.Name))
                .ForMember(patientDto => patientDto.Surname, opt => opt.MapFrom(patient => patient.Surname))
                .ForMember(patientDto => patientDto.Gender, opt => opt.MapFrom(patient => patient.Gender))
                .ForMember(patientDto => patientDto.BirthDate, opt => opt.MapFrom(patient => patient.BirthDate))
                .ForMember(patientDto => patientDto.Active, opt => opt.MapFrom(patient => patient.Active))
                .ReverseMap();
        }
    }

    public enum GenderDTO
    {
        male,
        female,
        other,
        unknown
    }
}
