﻿// <auto-generated />
using System;
using HB.Database;
using HB.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HB.Database.Migrations
{
    [DbContext(typeof(HBContext))]
    [Migration("20230221052055_TableCreateionProfileDetails_CPDetails_CPConnector")]
    partial class TableCreationProfileDetails_CPDetails_CPConnector
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cpms")
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("cpms.Database.DbModels.CPConnector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CPDetailsId")
                        .HasColumnType("int");

                    b.Property<string>("ConnectorName")
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceID")
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CPDetailsId");

                    b.ToTable("CPConnector", "cpms");
                });

            modelBuilder.Entity("HB.Database.DbModels.CPDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18,10)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18,10)");

                    b.Property<int>("ProfileDetailsId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProfileDetailsId");

                    b.ToTable("CPDetails", "cpms");
                });

            modelBuilder.Entity("HB.Database.DbModels.ProfileDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOtherLocation")
                        .HasColumnType("bit");

                    b.Property<string>("MaintenanceProgram")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MobileNo")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OfficeNo")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ProfileDetails", "cpms");
                });

            modelBuilder.Entity("Strateq.Core.Database.DbModel.AuditLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("AffectedId")
                        .HasColumnType("bigint");

                    b.Property<string>("AffectedTable")
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Operation")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("AuditLog", "cpms");
                });

            modelBuilder.Entity("Strateq.Core.Database.DbModel.AuditLogDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("AuditLogId")
                        .HasColumnType("bigint");

                    b.Property<string>("ColumnName")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("NewValue")
                        .HasColumnType("varchar(MAX)");

                    b.Property<string>("OriginalValue")
                        .HasColumnType("varchar(MAX)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogDetail", "cpms");
                });

            modelBuilder.Entity("HB.Database.DbModels.CPConnector", b =>
                {
                    b.HasOne("HB.Database.DbModels.CPDetails", "CPDetails")
                        .WithMany()
                        .HasForeignKey("CPDetailsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CPDetails");
                });

            modelBuilder.Entity("HB.Database.DbModels.CPDetails", b =>
                {
                    b.HasOne("HB.Database.DbModels.ProfileDetails", "ProfileDetails")
                        .WithMany()
                        .HasForeignKey("ProfileDetailsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ProfileDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
