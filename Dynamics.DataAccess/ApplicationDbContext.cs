using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationMember> OrganizationMember { get; set; }
        public virtual DbSet<OrganizationResource> OrganizationResources { get; set; }
        public virtual DbSet<OrganizationToProjectTransactionHistory> OrganizationToProjectTransactionHistory { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToOrganizationTransactionHistory> UserToOrganizationTransactionHistories { get; set; }
        public virtual DbSet<UserToProjectTransactionHistory> UserToProjectTransactionHistories { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<ProjectResource> ProjectResources { get; set; }
        public virtual DbSet<Request> Requests { get; set; }

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
                .HasKey(a => new { a.awardID });

            modelBuilder.Entity<OrganizationResource>()
                .HasKey(or => new { or.resourceID });

            modelBuilder.Entity<Organization>()
                .HasKey(o => new { o.organizationID });

            modelBuilder.Entity<OrganizationToProjectTransactionHistory>()
                .HasKey(o => new { o.transactionID });

            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasKey(u => new { u.transactionID });

            modelBuilder.Entity<UserToProjectTransactionHistory>()
                .HasKey(u => new { u.transactionID });

            modelBuilder.Entity<ProjectResource>()
                .HasKey(pr => new { pr.resourceID });

            modelBuilder.Entity<Project>()
                .HasKey(p => new { p.projectID });

            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.projectID, pm.userID });

            modelBuilder.Entity<OrganizationMember>()
                .HasKey(om => new { om.organizationID, om.userID });

            modelBuilder.Entity<Request>()
                .HasKey(r => new { r.requestID });

            modelBuilder.Entity<User>()
                .HasKey(u => new { u.userID });

            //    // ---------------------
            //    // Foreign Key
            // Award to User
            modelBuilder.Entity<Award>()
                .HasOne(a => a.User)
                .WithMany(u => u.Award)
                .HasForeignKey(a => a.userID);

            // OrganizationResource to Organization
            modelBuilder.Entity<OrganizationResource>()
                .HasOne(or => or.Organization)
                .WithMany(o => o.OrganizationResource)
                .HasForeignKey(or => or.organizationID);

            // Project to Organization
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Organization)
                .WithMany(o => o.Project)
                .HasForeignKey(p => p.organizationID);

            // Project to Request
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Request)
                .WithOne(r => r.Project)
                .HasForeignKey<Project>(p => p.requestID);

            // ProjectResource to Project
            modelBuilder.Entity<ProjectResource>()
                .HasOne(pr => pr.Project)
                .WithMany(p => p.ProjectResource)
                .HasForeignKey(pr => pr.projectID);

            // Request to User
            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany(u => u.Request)
                .HasForeignKey(r => r.userID);

            // ProjectMember to User
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMember)
                .HasForeignKey(pm => pm.userID);

            // ProjectMember to Project
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMember)
                .HasForeignKey(pm => pm.projectID);

            // OrganizationMember to User
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.User)
                .WithMany(u => u.OrganizationMember)
                .HasForeignKey(om => om.userID);

            // OrganizationMember to Organization
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.Organization)
                .WithMany(o => o.OrganizationMember)
                .HasForeignKey(om => om.organizationID);

            // UserToOrganizationTransactionHistory to User
            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserToOrganizationTransactions)
                .HasForeignKey(ut => ut.userID);

            // UserToOrganizationTransactionHistory to Organization
            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasOne(ut => ut.Organization)
                .WithMany(o => o.UserToOrganizationTransactions)
                .HasForeignKey(ut => ut.organizationID);

            // UserToProjectTransactionHistory to User
            modelBuilder.Entity<UserToProjectTransactionHistory>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserToProjectTransactions)
                .HasForeignKey(ut => ut.userID);

            // UserToProjectTransactionHistory to Project
            modelBuilder.Entity<UserToProjectTransactionHistory>()
                .HasOne(ut => ut.Project)
                .WithMany(p => p.UserToProjectTransactions)
                .HasForeignKey(ut => ut.projectID);

            // OrganizationToProjectTransactionHistory to Organization
            modelBuilder.Entity<OrganizationToProjectTransactionHistory>()
                .HasOne(ot => ot.Organization)
                .WithMany(o => o.OrganizationToProjectTransactions)
                .HasForeignKey(ot => ot.organizationID);

            // OrganizationToProjectTransactionHistory  to Project
            modelBuilder.Entity<Project>()
                .HasMany(ot => ot.OrganizationToProjectTransactions)
                .WithOne(p => p.Project)
                .HasForeignKey(ot => ot.projectID)
                .OnDelete(DeleteBehavior.NoAction);

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
