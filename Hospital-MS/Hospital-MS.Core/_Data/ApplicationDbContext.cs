﻿using Hospital_MS.Core.Extensions;
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
        public DbSet<Page> Pages { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Treasury> Treasuries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<DispensePermission> DispensePermissions { get; set; }
        public DbSet<StoreCount> StoreCounts { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AccountTree> AccountTrees { get; set; }
        public DbSet<CostCenterTree> CostCenterTree { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<PenaltyType> PenaltyTypes { get; set; }
        public DbSet<ContractDetail> ContractDetails { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<EmployeeAdvance> EmployeeAdvances { get; set; }
        public DbSet<AdvanceType> AdvanceTypes { get; set; }
        public DbSet<ReceiptPermission> ReceiptPermissions { get; set; }
        public DbSet<ReceiptPermissionItem> ReceiptPermissionItems { get; set; }
        public DbSet<MaterialIssuePermission> MaterialIssuePermissions { get; set; }
        public DbSet<MaterialIssueItem> MaterialIssueItems { get; set; }
        public DbSet<AttendanceSalary> AttendaceSalaries { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        public DbSet<DailyRestriction> DailyRestrictions { get; set; }
        public DbSet<DailyRestrictionDetail> DailyRestrictionDetails { get; set; }
        public DbSet<RestrictionType> RestrictionTypes { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<AdditionNotice> AdditionNotices { get; set; }
        public DbSet<DebitNotice> DebitNotices { get; set; }
        public DbSet<MainGroup> MainGroups { get; set; }
        public DbSet<RadiologyBodyType> RadiologyBodyTypes { get; set; }

        public DbSet<TreasuryOperation> TreasuryOperations { get; set; }
        public DbSet<TreasuryMovement> TreasuryMovements { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public DbSet<MedicalServiceDetail> MedicalServiceDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
           .SelectMany(t => t.GetForeignKeys())
           .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Staff>()
                 .HasOne(s => s.CreatedBy)
                 .WithMany()
                 .HasForeignKey(s => s.CreatedById)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
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
