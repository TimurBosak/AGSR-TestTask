namespace Patient.Domain.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }
    }
}