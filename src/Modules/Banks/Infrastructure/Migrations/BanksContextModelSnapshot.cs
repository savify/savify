﻿// <auto-generated />
using System;
using App.Modules.Banks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Modules.Banks.Infrastructure.Migrations
{
    [DbContext(typeof(BanksContext))]
    partial class BanksContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("App.BuildingBlocks.Application.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("data");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occurred_on");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_date");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("outbox_messages", "banks");
                });

            modelBuilder.Entity("App.BuildingBlocks.Infrastructure.Inbox.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("data");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occurred_on");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_date");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("inbox_messages", "banks");
                });

            modelBuilder.Entity("App.BuildingBlocks.Infrastructure.InternalCommands.InternalCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("data");

                    b.Property<DateTime>("EnqueueDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("enqueue_date");

                    b.Property<string>("Error")
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_date");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("internal_commands", "banks");
                });

            modelBuilder.Entity("App.Modules.Banks.Domain.Banks.Bank", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("_externalId")
                        .HasColumnType("text")
                        .HasColumnName("external_id");

                    b.Property<DateTime>("_createdAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("_defaultLogoUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("default_logo_url");

                    b.Property<bool>("_isRegulated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_regulated");

                    b.Property<string>("_logoUrl")
                        .HasColumnType("text")
                        .HasColumnName("logo_url");

                    b.Property<int?>("_maxConsentDays")
                        .HasColumnType("integer")
                        .HasColumnName("max_consent_days");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("_updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id", "_externalId");

                    b.ToTable("banks", "banks");
                });

            modelBuilder.Entity("App.Modules.Banks.Domain.BanksSynchronisationProcessing.BanksSynchronisationProcess", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("_finishedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("finished_at");

                    b.Property<DateTime>("_startedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.HasKey("Id");

                    b.ToTable("banks_synchronisation_processes", "banks");
                });

            modelBuilder.Entity("App.Modules.Banks.Domain.Banks.Bank", b =>
                {
                    b.OwnsOne("App.Modules.Banks.Domain.Banks.BankStatus", "_status", b1 =>
                        {
                            b1.Property<Guid>("BankId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Bank_externalId")
                                .HasColumnType("text");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("status");

                            b1.HasKey("BankId", "Bank_externalId");

                            b1.ToTable("banks", "banks");

                            b1.WithOwner()
                                .HasForeignKey("BankId", "Bank_externalId");
                        });

                    b.OwnsOne("App.Modules.Banks.Domain.Country", "_country", b1 =>
                        {
                            b1.Property<Guid>("BankId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Bank_externalId")
                                .HasColumnType("text");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("country_code");

                            b1.HasKey("BankId", "Bank_externalId");

                            b1.ToTable("banks", "banks");

                            b1.WithOwner()
                                .HasForeignKey("BankId", "Bank_externalId");
                        });

                    b.OwnsOne("App.Modules.Banks.Domain.ExternalProviders.ExternalProviderName", "_externalProviderName", b1 =>
                        {
                            b1.Property<Guid>("BankId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Bank_externalId")
                                .HasColumnType("text");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("external_provider_name");

                            b1.HasKey("BankId", "Bank_externalId");

                            b1.ToTable("banks", "banks");

                            b1.WithOwner()
                                .HasForeignKey("BankId", "Bank_externalId");
                        });

                    b.Navigation("_country");

                    b.Navigation("_externalProviderName");

                    b.Navigation("_status");
                });

            modelBuilder.Entity("App.Modules.Banks.Domain.BanksSynchronisationProcessing.BanksSynchronisationProcess", b =>
                {
                    b.OwnsOne("App.Modules.Banks.Domain.BanksSynchronisationProcessing.BanksSynchronisationProcessInitiator", "_initiatedBy", b1 =>
                        {
                            b1.Property<Guid>("BanksSynchronisationProcessId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("UserId")
                                .HasColumnType("uuid")
                                .HasColumnName("initiated_by_user_id");

                            b1.HasKey("BanksSynchronisationProcessId");

                            b1.ToTable("banks_synchronisation_processes", "banks");

                            b1.WithOwner()
                                .HasForeignKey("BanksSynchronisationProcessId");

                            b1.OwnsOne("App.Modules.Banks.Domain.BanksSynchronisationProcessing.BanksSynchronisationProcessInitiatorType", "Type", b2 =>
                                {
                                    b2.Property<Guid>("BanksSynchronisationProcessInitiatorBanksSynchronisationProcessId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("initiated_by_type");

                                    b2.HasKey("BanksSynchronisationProcessInitiatorBanksSynchronisationProcessId");

                                    b2.ToTable("banks_synchronisation_processes", "banks");

                                    b2.WithOwner()
                                        .HasForeignKey("BanksSynchronisationProcessInitiatorBanksSynchronisationProcessId");
                                });

                            b1.Navigation("Type")
                                .IsRequired();
                        });

                    b.OwnsOne("App.Modules.Banks.Domain.BanksSynchronisationProcessing.BanksSynchronisationProcessStatus", "_status", b1 =>
                        {
                            b1.Property<Guid>("BanksSynchronisationProcessId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("status");

                            b1.HasKey("BanksSynchronisationProcessId");

                            b1.ToTable("banks_synchronisation_processes", "banks");

                            b1.WithOwner()
                                .HasForeignKey("BanksSynchronisationProcessId");
                        });

                    b.Navigation("_initiatedBy")
                        .IsRequired();

                    b.Navigation("_status");
                });
#pragma warning restore 612, 618
        }
    }
}
