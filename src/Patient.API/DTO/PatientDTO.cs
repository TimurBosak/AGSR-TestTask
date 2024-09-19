namespace Patient.API.DTO
{
    public class PatientDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public GenderDTO Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }
    }

    public enum GenderDTO
    {
        male,
        female,
        other,
        unknown
    }
}
