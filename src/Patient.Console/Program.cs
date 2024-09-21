// See https://aka.ms/new-console-template for more information
using Patient.Console;
using System.Net.Http.Json;

while (true)
{
    Console.WriteLine("To create 100 patients click 1!");
    Console.WriteLine("To exit console app click 2");

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
        client.BaseAddress = new Uri("http://patientapi:80/");
        Console.WriteLine(client.BaseAddress);
        var response = await client.PostAsJsonAsync("api/Patient/CreateRangePatients", patientsPayload);

        if (response.IsSuccessStatusCode)
        {
            Console.Clear();
            Console.WriteLine("patients added");
        }
    }
    else if (answer.Key == ConsoleKey.D2)
    {
        Console.Clear();
        break;
    }
}

Console.WriteLine("Program finished");
