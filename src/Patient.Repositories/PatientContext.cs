using Microsoft.EntityFrameworkCore;

namespace Patient.Repositories
{
    public class PatientContext : DbContext
    {
        public PatientContext(DbContextOptions<PatientContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Models.Patient>()
                .Property(p => p.BirthDate)
                .IsRequired();
        }
    }
}
