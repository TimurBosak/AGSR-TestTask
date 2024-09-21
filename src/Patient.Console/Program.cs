// See https://aka.ms/new-console-template for more information
using Patient.Console;
using System.Net.Http.Json;

Console.WriteLine("To create 100 patients click 1!");
Console.WriteLine("To exit console app click 2");

while (true)
{
    var answer = Console.ReadKey();
    if (answer.Key == ConsoleKey.D1)
    {
        var patientsPayload = new List<PatientDTO>();
        var patientGenerator = new RandomPatientGenerator();
        var random = new Random();
        for (int i = 0; i < 100; i++)
        {
            var patient = patientGenerator.GenerateRandomPatient();

            patientsPayload.Add(patient);
        }

        var client = new HttpClient();
        if (IsRunningInDocker())
        {
            client.BaseAddress = new Uri("http://patientapi:80/");
        }
        else
        {
            client.BaseAddress = new Uri("https://localhost:7158/");
        }
        var response = await client.PostAsJsonAsync("api/Patient/CreateRangePatients", patientsPayload);

        if (response.IsSuccessStatusCode)
        {
            var sourceOfAdding = IsRunningInDocker() ? "from Docker" : "from localhost";
            Console.WriteLine($"patients added: {sourceOfAdding}");
        }
    }
    else if (answer.Key == ConsoleKey.D2)
    {
        Console.Clear();
        break;
    }
}

Console.WriteLine("Program finished");

static bool IsRunningInDocker()
{
    return File.Exists("/proc/1/cgroup") && File.ReadAllText("/proc/1/cgroup").Contains("docker");
}
