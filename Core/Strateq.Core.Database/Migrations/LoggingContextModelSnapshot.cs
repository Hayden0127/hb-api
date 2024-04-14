﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Strateq.Core.Database;

#nullable disable

namespace Strateq.Core.Database.Migrations
{
    [DbContext(typeof(LoggingContext))]
    partial class LoggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cpms")
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);


            modelBuilder.Entity("Strateq.Core.Database.DbModel.RequestLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Controller")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Request")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.ToTable("RequestLog", "cpms");
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

#pragma warning restore 612, 618
        }
    }
}
