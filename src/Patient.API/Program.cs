using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patient.Repositories;
using Patient.Repositories.Implementations;
using Patient.Repositories.Interfaces;
using Patient.Services;
using Patient.Services.Interfaces;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory);

builder.Services.AddDbContext<PatientContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<PatientContext>>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<PatientContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database already existing");
    }
}

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    o.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
