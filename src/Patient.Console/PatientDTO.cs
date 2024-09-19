namespace Patient.Console
{
    public class PatientDTO
    {
        public string name { get; set; }

        public string surname { get; set; }

        public Gender gender { get; set; }

        public DateTime birthDate { get; set; }

        public bool active { get; set; }
    }

    public enum Gender
    {
        male,
        female,
        other,
        unknown
    }
}
