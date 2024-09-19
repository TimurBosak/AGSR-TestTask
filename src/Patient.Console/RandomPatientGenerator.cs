namespace Patient.Console
{
    public class RandomPatientGenerator
    {
        private static string[] names = { "Антон", "Андрей", "Тимур", "Евгений", "Александра", "Виктория", "Марина", "Анастасия" };
        private static string[] surnames = { "Босак", "Прокопенко", "Коваль", "Можайко", "Шиманович", "Будейко" };
        private readonly Random gen = new Random();
        private readonly RandomDateTime dateGenerator = new RandomDateTime();

        public PatientDTO GenerateRandomPatient()
        {
            var patient = new PatientDTO()
            {
                name = GetRandomName(),
                surname = GetRandomSurname(),
                active = gen.Next(0, 2) == 1, // Random boolean
                gender = (Gender)gen.Next(1, 5), // Random enum value from 1 to 4
                birthDate = dateGenerator.Next(),
            };

            return patient;
        }

        private string GetRandomName()
        {
            return names[gen.Next(names.Length)];
        }

        private string GetRandomSurname()
        {
            return surnames[gen.Next(surnames.Length)];
        }
    }
}
