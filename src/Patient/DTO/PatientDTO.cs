namespace Patient.API.DTO
{
    public class PatientDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }
    }
}
