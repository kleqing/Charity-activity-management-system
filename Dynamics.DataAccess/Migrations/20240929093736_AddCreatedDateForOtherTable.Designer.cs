﻿// <auto-generated />
using System;
using Dynamics.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240929093736_AddCreatedDateForOtherTable")]
    partial class AddCreatedDateForOtherTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dynamics.Models.Models.Award", b =>
                {
                    b.Property<Guid>("AwardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AwardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AwardID");

                    b.HasIndex("UserID");

                    b.ToTable("Awards");
                });

            modelBuilder.Entity("Dynamics.Models.Models.History", b =>
                {
                    b.Property<Guid>("HistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Attachment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Phase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProjectID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HistoryID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Organization", b =>
                {
                    b.Property<Guid>("OrganizationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CEOID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrganizationDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationPictures")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("ShutdownDay")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("StartTime")
                        .HasColumnType("date");

                    b.Property<bool>("isBanned")
                        .HasColumnType("bit");

                    b.HasKey("OrganizationID");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Dynamics.Models.Models.OrganizationMember", b =>
                {
                    b.Property<Guid>("OrganizationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrganizationID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("OrganizationMember");
                });

            modelBuilder.Entity("Dynamics.Models.Models.OrganizationResource", b =>
                {
                    b.Property<Guid>("ResourceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentTransaction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExpectedQuantity")
                        .HasColumnType("int");

                    b.Property<Guid>("OrganizationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ResourceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResourceID");

                    b.HasIndex("OrganizationID");

                    b.ToTable("OrganizationResources");
                });

            modelBuilder.Entity("Dynamics.Models.Models.OrganizationToProjectHistory", b =>
                {
                    b.Property<Guid>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrganizationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ResourceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Time")
                        .HasColumnType("date");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.HasIndex("OrganizationID");

                    b.HasIndex("ProjectID");

                    b.ToTable("OrganizationToProjectTransactionHistory");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Project", b =>
                {
                    b.Property<Guid>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Attachment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly?>("EndTime")
                        .HasColumnType("date");

                    b.Property<Guid?>("LeaderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrganizationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProjectDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectStatus")
                        .HasColumnType("int");

                    b.Property<Guid?>("RequestID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly?>("StartTime")
                        .HasColumnType("date");

                    b.HasKey("ProjectID");

                    b.HasIndex("OrganizationID");

                    b.HasIndex("RequestID")
                        .IsUnique()
                        .HasFilter("[RequestID] IS NOT NULL");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Dynamics.Models.Models.ProjectMember", b =>
                {
                    b.Property<Guid>("ProjectID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProjectID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("ProjectMembers");
                });

            modelBuilder.Entity("Dynamics.Models.Models.ProjectResource", b =>
                {
                    b.Property<Guid>("ResourceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ExpectedQuantity")
                        .HasColumnType("int");

                    b.Property<Guid>("ProjectID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ResourceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResourceID");

                    b.HasIndex("ProjectID");

                    b.ToTable("ProjectResources");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Request", b =>
                {
                    b.Property<Guid>("RequestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Attachment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("isEmergency")
                        .HasColumnType("int");

                    b.HasKey("RequestID");

                    b.HasIndex("UserID");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Dynamics.Models.Models.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserAvatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("UserDOB")
                        .HasColumnType("date");

                    b.Property<string>("UserDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserFullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isBanned")
                        .HasColumnType("bit");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dynamics.Models.Models.UserToOrganizationHistory", b =>
                {
                    b.Property<Guid>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoneyTransactionAmout")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrganizationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("Time")
                        .HasColumnType("date");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TransactionID");

                    b.HasIndex("OrganizationID");

                    b.HasIndex("UserID");

                    b.ToTable("UserToOrganizationTransactionHistories");
                });

            modelBuilder.Entity("Dynamics.Models.Models.UserToProjectHistory", b =>
                {
                    b.Property<Guid>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoneyTransactionAmout")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProjectID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TransactionID");

                    b.HasIndex("ProjectID");

                    b.HasIndex("UserID");

                    b.ToTable("UserToProjectTransactionHistories");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Award", b =>
                {
                    b.HasOne("Dynamics.Models.Models.User", "User")
                        .WithMany("Award")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dynamics.Models.Models.History", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Project", "Project")
                        .WithMany("History")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Dynamics.Models.Models.OrganizationMember", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Organization", "Organization")
                        .WithMany("OrganizationMember")
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dynamics.Models.Models.User", "User")
                        .WithMany("OrganizationMember")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dynamics.Models.Models.OrganizationResource", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Organization", "Organization")
                        .WithMany("OrganizationResource")
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Dynamics.Models.Models.OrganizationToProjectHistory", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Organization", "Organization")
                        .WithMany("OrganizationToProjectTransactions")
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dynamics.Models.Models.Project", "Project")
                        .WithMany("OrganizationToProjectTransactions")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Project", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Organization", "Organization")
                        .WithMany("Project")
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dynamics.Models.Models.Request", "Request")
                        .WithOne("Project")
                        .HasForeignKey("Dynamics.Models.Models.Project", "RequestID");

                    b.Navigation("Organization");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("Dynamics.Models.Models.ProjectMember", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Project", "Project")
                        .WithMany("ProjectMember")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dynamics.Models.Models.User", "User")
                        .WithMany("ProjectMember")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dynamics.Models.Models.ProjectResource", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Project", "Project")
                        .WithMany("ProjectResource")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Request", b =>
                {
                    b.HasOne("Dynamics.Models.Models.User", "User")
                        .WithMany("Request")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dynamics.Models.Models.UserToOrganizationHistory", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Organization", "Organization")
                        .WithMany("UserToOrganizationTransactions")
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dynamics.Models.Models.User", "User")
                        .WithMany("UserToOrganizationTransactions")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dynamics.Models.Models.UserToProjectHistory", b =>
                {
                    b.HasOne("Dynamics.Models.Models.Project", "Project")
                        .WithMany("UserToProjectTransactions")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dynamics.Models.Models.User", "User")
                        .WithMany("UserToProjectTransactions")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Organization", b =>
                {
                    b.Navigation("OrganizationMember");

                    b.Navigation("OrganizationResource");

                    b.Navigation("OrganizationToProjectTransactions");

                    b.Navigation("Project");

                    b.Navigation("UserToOrganizationTransactions");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Project", b =>
                {
                    b.Navigation("History");

                    b.Navigation("OrganizationToProjectTransactions");

                    b.Navigation("ProjectMember");

                    b.Navigation("ProjectResource");

                    b.Navigation("UserToProjectTransactions");
                });

            modelBuilder.Entity("Dynamics.Models.Models.Request", b =>
                {
                    b.Navigation("Project")
                        .IsRequired();
                });

            modelBuilder.Entity("Dynamics.Models.Models.User", b =>
                {
                    b.Navigation("Award");

                    b.Navigation("OrganizationMember");

                    b.Navigation("ProjectMember");

                    b.Navigation("Request");

                    b.Navigation("UserToOrganizationTransactions");

                    b.Navigation("UserToProjectTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
