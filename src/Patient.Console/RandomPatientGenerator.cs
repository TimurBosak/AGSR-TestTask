namespace Patient.Console
{
    public class RandomPatientGenerator
    {
        private static readonly string[] names = { "Антон", "Андрей", "Тимур", "Евгений", "Александра", "Виктория", "Марина", "Анастасия" };
        private static readonly string[] surnames = { "Босак", "Прокопенко", "Коваль", "Можайко", "Шиманович", "Будейко" };
        private readonly Random generator = new();
        private readonly RandomDateTime dateGenerator = new ();

        public PatientDTO GenerateRandomPatient()
        {
            var patient = new PatientDTO()
            {
                name = GetRandomName(),
                surname = GetRandomSurname(),
                active = generator.Next(0, 2) == 1,
                gender = (Gender)generator.Next(1, 5),
                birthDate = dateGenerator.Next(),
            };

            return patient;
        }

        private string GetRandomName()
        {
            return names[generator.Next(names.Length)];
        }

        private string GetRandomSurname()
        {
            return surnames[generator.Next(surnames.Length)];
        }
    }
}
