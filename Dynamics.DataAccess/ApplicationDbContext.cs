using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationMember> OrganizationMember { get; set; }
        public virtual DbSet<OrganizationResource> OrganizationResources { get; set; }
        public virtual DbSet<OrganizationToProjectHistory> OrganizationToProjectTransactionHistory { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToOrganizationTransactionHistory> UserToOrganizationTransactionHistories { get; set; }
        public virtual DbSet<UserToProjectTransactionHistory> UserToProjectTransactionHistories { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<ProjectResource> ProjectResources { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<History> Histories { get; set; }

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
            modelBuilder.Entity<Report>()
               .HasKey(a => new { a.ReportID });

            modelBuilder.Entity<Award>()
                .HasKey(a => new { a.AwardID });

            modelBuilder.Entity<OrganizationResource>()
                .HasKey(or => new { or.ResourceID });

            modelBuilder.Entity<Organization>()
                .HasKey(o => new { o.OrganizationID });

            modelBuilder.Entity<OrganizationToProjectHistory>()
                .HasKey(o => new { o.TransactionID });

            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasKey(u => new { u.TransactionID });

            modelBuilder.Entity<UserToProjectTransactionHistory>()
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

            //    // ---------------------
            //    // Foreign Key

            //Report to User: one user can have many reports
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.ReportsMade)
                .HasForeignKey(r => r.ReporterID)
                .OnDelete(DeleteBehavior.NoAction);

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
            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserToOrganizationTransactions)
                .HasForeignKey(ut => ut.UserID);

            // UserToOrganizationTransactionHistory to Organization
            modelBuilder.Entity<UserToOrganizationTransactionHistory>()
                .HasOne(ut => ut.Organization)
                .WithMany(o => o.UserToOrganizationTransactions)
                .HasForeignKey(ut => ut.OrganizationID);

            // UserToProjectTransactionHistory to User
            modelBuilder.Entity<UserToProjectTransactionHistory>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserToProjectTransactions)
                .HasForeignKey(ut => ut.UserID);

            // UserToProjectTransactionHistory to Project
            modelBuilder.Entity<UserToProjectTransactionHistory>()
                .HasOne(ut => ut.Project)
                .WithMany(p => p.UserToProjectTransactions)
                .HasForeignKey(ut => ut.ProjectID);
            // UserToProjectTransactionHistory to ProjectResource
            modelBuilder.Entity<UserToProjectTransactionHistory>()
               .HasOne(ut => ut.Resource)
               .WithMany(p => p.UserToProjectTransactionHistories)
               .HasForeignKey(ut => ut.ResourceID)
                 .OnDelete(DeleteBehavior.NoAction); 


            // OrganizationToProjectTransactionHistory to Organization
            modelBuilder.Entity<OrganizationToProjectHistory>()
                .HasOne(ot => ot.Organization)
                .WithMany(o => o.OrganizationToProjectTransactions)
                .HasForeignKey(ot => ot.OrganizationID);
            // OrganizationToProjectTransactionHistory to ProjectResource
            modelBuilder.Entity<OrganizationToProjectHistory>()
             .HasOne(ut => ut.Resource)
             .WithMany(p => p.OrganizationToProjectHistories)
             .HasForeignKey(ut => ut.ResourceID)
               .OnDelete(DeleteBehavior.NoAction); 


            // OrganizationToProjectTransactionHistory  to Project
            modelBuilder.Entity<Project>()
                .HasMany(ot => ot.OrganizationToProjectTransactions)
                .WithOne(p => p.Project)
                .HasForeignKey(ot => ot.ProjectID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectResource>()
               .HasMany(ot => ot.UserToProjectTransactionHistories)
               .WithOne(p => p.Resource)
               .HasForeignKey(ot => ot.ResourceID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectResource>()
             .HasMany(ot => ot.OrganizationToProjectHistories)
             .WithOne(p => p.Resource)
             .HasForeignKey(ot => ot.ResourceID)
             .OnDelete(DeleteBehavior.NoAction);

            // History to Project
            modelBuilder.Entity<History>()
                .HasOne(h => h.Project)
                .WithMany(p => p.History)
                .HasForeignKey(h => h.ProjectID);
            //    // -----------------
            //    // OnDelete()


            //    // ---------------------
            //    // Auto increment

            //    // User (Test-only)
            //    modelBuilder.Entity<User>()
            //        .Property(u => u.userID)
            //        .ValueGeneratedOnAdd();  // Auto-increment

            //seed organization
            modelBuilder.Entity<Organization>().HasData(
          new Organization()
          {
              OrganizationID = Guid.Parse("4641b799-7fba-4d20-a78a-4d68db162e98"),
              OrganizationName = "Organization 1",
              OrganizationDescription = "llalalala"
          },
           new Organization()
           {
               OrganizationID = Guid.Parse("c2f983db-d9bb-4214-ae40-eab93cc2de72"),
               OrganizationName = "Organization 2",
               OrganizationDescription = "llalalala222"
           }
      );
            //seed request
            modelBuilder.Entity<Request>().HasData(
          new Request()
          {
              RequestID = Guid.Parse("ad111cb1-39d5-42e2-a2f5-e943898e59e6"),
              UserID = Guid.Parse("FBD9F087-D6D2-4A27-A763-F1951DA04361"),
              Title = "Request 1",
              Content = "Content of request 1"
          },
           new Request()
           {
               RequestID = Guid.Parse("4398bfb2-5a6d-4daf-a8be-52ef92f455a9"),
               UserID = Guid.Parse("FBD9F087-D6D2-4A27-A763-F1951DA04361"),
               Title = "Request 2",
               Content = "Content of request 2"
           }
      );
            //      //seed project

            //      //seed member project
            //modelBuilder.Entity<ProjectMember>().HasData(
            //    new ProjectMember()
            //    {
            //        ProjectID = Guid.Parse("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"),
            //        UserID = Guid.Parse("E4894112-58A3-4FE6-9222-8AD70816ADFE")
            //    },
            //      new ProjectMember()
            //      {
            //          ProjectID = Guid.Parse("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"),
            //          UserID = Guid.Parse("FBD9F087-D6D2-4A27-A763-F1951DA04361")
            //      },
            //        new ProjectMember()
            //        {
            //            ProjectID = Guid.Parse("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"),
            //            UserID = Guid.Parse("C3488DB1-AFE6-40AE-BEBB-424C4ACF97C1")
            //        }
            //    );
            //      //seeding tiền organ gửi tới project-true   // 1 organization gửi tới 1 project 2 transaction
           
        }
    }
}
