﻿// <auto-generated />
using System;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevProdWebApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240724154328_SettingsTbl_Grouping")]
    partial class SettingsTbl_Grouping
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevProdWebApp.Models.Developer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Grouping")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Methodolgy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Preprocessing")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Scale")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("DevProdWebApp.Models.ToolMetric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Scale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SettingId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("SettingId");

                    b.ToTable("ToolMetric");
                });

            modelBuilder.Entity("DevProdWebApp.Models.ToolMetricValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ToolMetricId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ToolMetricId");

                    b.ToTable("ToolMetricValues");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Developer", b =>
                {
                    b.HasOne("DevProdWebApp.Models.Project", null)
                        .WithMany("Developers")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Metric", b =>
                {
                    b.HasOne("DevProdWebApp.Models.Project", null)
                        .WithMany("ProjectMetrics")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("DevProdWebApp.Models.ToolMetric", b =>
                {
                    b.HasOne("DevProdWebApp.Models.Setting", "Setting")
                        .WithMany("ToolMetricList")
                        .HasForeignKey("SettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Setting");
                });

            modelBuilder.Entity("DevProdWebApp.Models.ToolMetricValue", b =>
                {
                    b.HasOne("DevProdWebApp.Models.ToolMetric", "ToolMetric")
                        .WithMany("ToolMetricValues")
                        .HasForeignKey("ToolMetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToolMetric");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Project", b =>
                {
                    b.Navigation("Developers");

                    b.Navigation("ProjectMetrics");
                });

            modelBuilder.Entity("DevProdWebApp.Models.Setting", b =>
                {
                    b.Navigation("ToolMetricList");
                });

            modelBuilder.Entity("DevProdWebApp.Models.ToolMetric", b =>
                {
                    b.Navigation("ToolMetricValues");
                });
#pragma warning restore 612, 618
        }
    }
}
