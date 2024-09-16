using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public partial class DBContext : DbContext
    {
        public DBContext() { }
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
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
        public virtual DbSet<Award> Awards { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBConnect"));
            // run 'dotnet ef migrations add "Initial" in terminal to create a migration
            // remove migration: dotnet ef migrations remove
            // Add to sql server: dotnet ef database update
            //if (!optionsBuilder.IsConfigured)
            //{
            //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            //    //Data Source=(local);Database=Charity_Management_System;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;
            //    optionsBuilder.UseSqlServer("Server=DESKTOP-7VJGJ8V;Database=Charity_Management_System;TrustServerCertificate=true;Trusted_Connection=True;Encrypt=false;");
            //}
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If you have other entity configurations or relationships
            // model them here as well.

            // ---------------------
            // Primary Key
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

            modelBuilder.Entity<Award>()
                .HasKey(a => new { a.awardID });

            // ---------------------
            // Foreign Key

            // User - Award
            modelBuilder.Entity<User>()
                .HasOne(u => u.award)
                .WithOne(a => a.user)
                .HasForeignKey<User>(u => u.awardID);

            // User - Request
            modelBuilder.Entity<Request>()
                .HasOne(u => u.user)
                .WithOne(r => r.request)
                .HasForeignKey<Request>(r => r.userID);

            // Project - Request
            modelBuilder.Entity<Project>()
                .HasOne(p => p.request)
                .WithOne(r => r.project)
                .HasForeignKey<Project>(r => r.requestID);

            // Project - ProjectMember
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.project)
                .WithOne(p => p.projectMember)
                .HasForeignKey<ProjectMember>(pm => pm.projectID);

            // Project - ProjectResource
            modelBuilder.Entity<ProjectResource>()
                .HasOne(pr => pr.project)
                .WithOne(p => p.projectResource)
                .HasForeignKey<ProjectResource>(pr => pr.projectID);

            // UserToOrganizationTransactionHistory - User
            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasOne(utoth => utoth.user)
                .WithOne(u => u.userToOrganizationTransactionHistory)
                .HasForeignKey<UserToOrganizationTransactionHistory>(utoth => utoth.userID);

            // OrganizationMember - User
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.user)
                .WithOne(u => u.organizationMember)
                .HasForeignKey<OrganizationMember>(om => om.userID);

            // Project - Organization
            modelBuilder.Entity<Project>()
                .HasOne(p => p.organization)
                .WithOne(o => o.project)
                .HasForeignKey<Project>(p => p.organizationID);

            // OrganizationMember - Organization
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.organization)
                .WithOne(o => o.organizationMember)
                .HasForeignKey<OrganizationMember>(om => om.organizationID);

            // OrganizationResource - Organization
            modelBuilder.Entity<OrganizationResource>()
                .HasOne(or => or.organization)
                .WithOne(o => o.organizationResource)
                .HasForeignKey<OrganizationResource>(or => or.organizationID);

            // OrganizationToProjectTransactionHistory - Organization
            modelBuilder.Entity<OrganizationToProjectTransactionHistory>()
                .HasOne(otpth => otpth.organization)
                .WithOne(o => o.organizationToProjectTransactionHistory)
                .HasForeignKey<OrganizationToProjectTransactionHistory>(otpth => otpth.organizationID);

            // UserToOrganizationTransactionHistory - Organization
            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasOne(utoth => utoth.organization)
                .WithOne(o => o.userToOrganizationTransactionHistory)
                .HasForeignKey<UserToOrganizationTransactionHistory>(utoth => utoth.organizationID);

            // -----------------
            // OnDelete()

            // OrganizationToProjectTransactionHistory - Project
            modelBuilder.Entity<OrganizationToProjectTransactionHistory>()
                .HasOne(otpth => otpth.project)
                .WithOne(o => o.organizationToProjectTransactionHistory)
                .HasForeignKey<Project>(otpth => otpth.projectID)
                .OnDelete(DeleteBehavior.NoAction);

            // UserToProjectTransactionHistory - User
            modelBuilder.Entity<UserToProjectTransactionHistory>()
                .HasOne(utpth => utpth.user)
                .WithOne(o => o.userToProjectTransactionHistory)
                .HasForeignKey<User>(utpth => utpth.userID)
                .OnDelete(DeleteBehavior.NoAction);

            // User - ProjectMember
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.user)
                .WithOne(o => o.projectMember)
                .HasForeignKey<User>(pm => pm.userID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
