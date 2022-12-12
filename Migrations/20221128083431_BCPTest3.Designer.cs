﻿// <auto-generated />
using System;
using BCPUtilityAzureFunction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BCPUtilityAzureFunction.Migrations
{
    [DbContext(typeof(BCPUtilityDBContext))]
    [Migration("20221128083431_BCPTest3")]
    partial class BCPTest3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BCPUtilityAzureFunction.Models.BCPDocData", b =>
                {
                    b.Property<int>("DocId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocId"));

                    b.Property<string>("BCP_Flag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Config")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discipline_Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Document_Last_Updated_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Document_Rendition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Document_Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName_Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("File_Last_Updated_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("File_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File_OBID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File_Rendition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File_UID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plant_Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Primary_File")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rendition_OBID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rendition_Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Revision")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sub_Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("DocId");

                    b.ToTable("SPM_JOB_DETAILS");
                });
#pragma warning restore 612, 618
        }
    }
}
