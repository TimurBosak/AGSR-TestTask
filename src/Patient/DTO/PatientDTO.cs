namespace Patient.API.DTO
{
    public class PatientDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }
    }

    public enum Gender
    {
        male,
        female,
        other,
        unknown
    }
}
