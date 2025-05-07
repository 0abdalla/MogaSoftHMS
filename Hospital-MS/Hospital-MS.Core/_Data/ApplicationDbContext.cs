using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Models.HR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hospital_MS.Core._Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<NurseActivity> NurseActivities { get; set; }
        public DbSet<NurseShift> NurseShifts { get; set; }
        public DbSet<DoctorRating> DoctorPerformances { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PatientAttachment> PatientAttachments { get; set; }
        public DbSet<PatientMedicalHistory> PatientMedicalHistories { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<StaffAttachments> StaffAttachments { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<MedicalServiceSchedule> MedicalServiceSchedules { get; set; }

        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobLevel> JobLevels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
           .SelectMany(t => t.GetForeignKeys())
           .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entityEntry in entries)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(e => e.CreatedById).CurrentValue = currentUserId;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                    entityEntry.Property(e => e.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
