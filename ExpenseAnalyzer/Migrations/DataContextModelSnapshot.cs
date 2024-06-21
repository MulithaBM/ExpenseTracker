﻿// <auto-generated />
using System;
using ExpenseAnalyzer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpenseAnalyzer.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExpenseAnalyzer.Models.BOC_CC_Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HostReferenceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InternetBankingReferenceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<DateOnly>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("TransactionTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("InternetBankingReferenceNumber", "HostReferenceNumber")
                        .IsUnique();

                    b.ToTable("BOC_CC_Payments");
                });

            modelBuilder.Entity("ExpenseAnalyzer.Models.OwnAccountTransfer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreditAccountNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebitAccountNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FundsTransferMethod")
                        .HasColumnType("text");

                    b.Property<string>("HostReferenceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OnlineBankingReferenceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<DateOnly>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("TransactionTime")
                        .HasColumnType("time without time zone");

                    b.Property<decimal>("TransferAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OnlineBankingReferenceNumber", "HostReferenceNumber")
                        .IsUnique();

                    b.ToTable("OwnAccountTransfers");
                });
#pragma warning restore 612, 618
        }
    }
}