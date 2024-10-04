using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationMember> OrganizationMember { get; set; }
        public virtual DbSet<OrganizationResource> OrganizationResources { get; set; }
        public virtual DbSet<OrganizationToProjectHistory> OrganizationToProjectTransactionHistory { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToOrganizationHistory> UserToOrganizationTransactionHistories { get; set; }
        public virtual DbSet<UserToProjectHistory> UserToProjectTransactionHistories { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<ProjectResource> ProjectResources { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable sensitive data logging
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If you have other entity configurations or relationships
            // model them here as well.

            // ---------------------
            // Primary Key

            modelBuilder.Entity<Award>()
                .HasKey(a => new { a.AwardID });

            modelBuilder.Entity<OrganizationResource>()
                .HasKey(or => new { or.ResourceID });

            modelBuilder.Entity<Organization>()
                .HasKey(o => new { o.OrganizationID });

            modelBuilder.Entity<OrganizationToProjectHistory>()
                .HasKey(o => new { o.TransactionID });

            modelBuilder.Entity<UserToOrganizationHistory>()
                .HasKey(u => new { u.TransactionID });

            modelBuilder.Entity<UserToProjectHistory>()
                .HasKey(u => new { u.TransactionID });

            modelBuilder.Entity<ProjectResource>()
                .HasKey(pr => new { pr.ResourceID });

            modelBuilder.Entity<Project>()
                .HasKey(p => new { p.ProjectID });

            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectID, pm.UserID });

            modelBuilder.Entity<OrganizationMember>()
                .HasKey(om => new { om.OrganizationID, om.UserID });

            modelBuilder.Entity<Request>()
                .HasKey(r => new { r.RequestID });

            modelBuilder.Entity<User>()
                .HasKey(u => new { u.UserID });

            modelBuilder.Entity<Report>()
                .HasKey(u => new { u.ReportID });


            //    // ---------------------
            //    // Foreign Key
            // Award to User
            modelBuilder.Entity<Award>()
                .HasOne(a => a.User)
                .WithMany(u => u.Award)
                .HasForeignKey(a => a.UserID);

            // OrganizationResource to Organization
            modelBuilder.Entity<OrganizationResource>()
                .HasOne(or => or.Organization)
                .WithMany(o => o.OrganizationResource)
                .HasForeignKey(or => or.OrganizationID);

            // Project to Organization
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Organization)
                .WithMany(o => o.Project)
                .HasForeignKey(p => p.OrganizationID);

            // Project to Request
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Request)
                .WithOne(r => r.Project)
                .HasForeignKey<Project>(p => p.RequestID);

            // ProjectResource to Project
            modelBuilder.Entity<ProjectResource>()
                .HasOne(pr => pr.Project)
                .WithMany(p => p.ProjectResource)
                .HasForeignKey(pr => pr.ProjectID);

            // Request to User
            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany(u => u.Request)
                .HasForeignKey(r => r.UserID);

            // ProjectMember to User
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMember)
                .HasForeignKey(pm => pm.UserID);

            // ProjectMember to Project
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMember)
                .HasForeignKey(pm => pm.ProjectID);

            // OrganizationMember to User
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.User)
                .WithMany(u => u.OrganizationMember)
                .HasForeignKey(om => om.UserID);

            // OrganizationMember to Organization
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.Organization)
                .WithMany(o => o.OrganizationMember)
                .HasForeignKey(om => om.OrganizationID);

            // UserToOrganizationTransactionHistory to User
            modelBuilder.Entity<UserToOrganizationHistory>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserToOrganizationTransactions)
                .HasForeignKey(ut => ut.UserID);

            // UserToOrganizationTransactionHistory to Organization
            modelBuilder.Entity<UserToOrganizationHistory>()
                .HasOne(ut => ut.Organization)
                .WithMany(o => o.UserToOrganizationTransactions)
                .HasForeignKey(ut => ut.OrganizationID);

            // UserToProjectTransactionHistory to User
            modelBuilder.Entity<UserToProjectHistory>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserToProjectTransactions)
                .HasForeignKey(ut => ut.UserID);

            // UserToProjectTransactionHistory to Project
            modelBuilder.Entity<UserToProjectHistory>()
                .HasOne(ut => ut.Project)
                .WithMany(p => p.UserToProjectTransactions)
                .HasForeignKey(ut => ut.ProjectID);

            // OrganizationToProjectTransactionHistory to Organization
            modelBuilder.Entity<OrganizationToProjectHistory>()
                .HasOne(ot => ot.Organization)
                .WithMany(o => o.OrganizationToProjectTransactions)
                .HasForeignKey(ot => ot.OrganizationID);

            // OrganizationToProjectTransactionHistory  to Project
            modelBuilder.Entity<Project>()
                .HasMany(ot => ot.OrganizationToProjectTransactions)
                .WithOne(p => p.Project)
                .HasForeignKey(ot => ot.ProjectID)
                .OnDelete(DeleteBehavior.NoAction);

            // History to Project
            modelBuilder.Entity<History>()
                .HasOne(h => h.Project)
                .WithMany(p => p.History)
                .HasForeignKey(h => h.ProjectID);

            // Report to User
            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Report)
                .HasForeignKey(r => r.ReporterID);
            //    // -----------------
            //    // OnDelete()


            //    // ---------------------
            //    // Auto increment

            //    // User (Test-only)
            //    modelBuilder.Entity<User>()
            //        .Property(u => u.userID)
            //        .ValueGeneratedOnAdd();  // Auto-increment
        }
    }
}
