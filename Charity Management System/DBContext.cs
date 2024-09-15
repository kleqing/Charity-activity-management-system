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
        public virtual DbSet<OrganizationToProjectTransactionHistory> OrganizationToProjectTransactionHistories { get; set; }
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

            // User - Award
            modelBuilder.Entity<User>()
                .HasOne(u => u.award)
                .WithOne(a => a.user)
                .HasForeignKey<User>(u => u.awardID);

            // User - Request


            // Project - Project Member
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.projectID, pm.userID });  // Composite key for many-to-many relation

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.project)
                .WithMany(p => p.projectMember)  // Add a collection navigation property in `Project` for members if missing
                .HasForeignKey(pm => pm.projectID);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.user)
                .WithMany(u => u.projectMember)  // Add a collection navigation property in `User` for projectMembers if missing
                .HasForeignKey(pm => pm.userID);



            // Organization Resource - Organization
            //modelBuilder.Entity<OrganizationResource>()
            //    .HasKey(or => new { or.resourceID });

            //modelBuilder.Entity<Organization>()
            //    .HasOne(o => o.organizationResource)
            //    .WithOne(or => or.organization)
            //    .HasForeignKey<OrganizationResource>(or => or.organizationID);

            //// Project - Organization
            //modelBuilder.Entity<Organization>()
            //    .HasOne(o => o.project)
            //    .WithOne(p => p.organization)
            //    .HasForeignKey<Project>(p => p.organizationID);

            //modelBuilder.Entity<Project>()
            //    .HasOne(o => o.organization)
            //    .WithOne(oi => oi.project)
            //    .HasForeignKey<Organization>(oi => oi.organizationID);

            //// Organization Member - Organization
            //modelBuilder.Entity<Organization>()
            //    .HasOne(o => o.organizationMember)
            //    .WithOne(om => om.organization)
            //    .HasForeignKey<OrganizationMember>(om => om.organizationID);

            //modelBuilder.Entity<OrganizationMember>()
            //    .HasOne(o => o.organization)
            //    .WithOne(oi => oi.organizationMember)
            //    .HasForeignKey<Organization>(oi => oi.organizationID);

            //// User To Project Transaction History - Organization
            //modelBuilder.Entity<UserToOrganizationTransactionHistory>()
            //    .HasOne(otpth => otpth.organization)
            //    .WithMany(o => o.userToOrganizationTransactionHistory)
            //    .HasForeignKey(otpth => otpth.organizationID);

            //modelBuilder.Entity<Organization>()
            //    .HasOne(o => o.userToOrganizationTransactionHistory)
            //    .WithOne(utoth => utoth.organization)
            //    .HasForeignKey<UserToOrganizationTransactionHistory>(utoth => utoth.organizationID);

        }
    }
}
