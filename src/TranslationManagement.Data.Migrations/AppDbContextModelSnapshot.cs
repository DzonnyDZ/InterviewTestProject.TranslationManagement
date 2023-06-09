﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TranslationManagement.Data;

#nullable disable

namespace TranslationManagement.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("TranslationManagement.Data.Model.TranslationJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomerName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalContent")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<string>("TranslatedContent")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TranslatorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TranslatorId");

                    b.ToTable("TranslationJobs");
                });

            modelBuilder.Entity("TranslationManagement.Data.Model.Translator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreditCardNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("HourlyRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Translators");
                });

            modelBuilder.Entity("TranslationManagement.Data.Model.TranslationJob", b =>
                {
                    b.HasOne("TranslationManagement.Data.Model.Translator", "Translator")
                        .WithMany()
                        .HasForeignKey("TranslatorId");

                    b.Navigation("Translator");
                });
#pragma warning restore 612, 618
        }
    }
}
