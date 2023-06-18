﻿// <auto-generated />
using System;
using App.Modules.Accounts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Modules.Accounts.Infrastructure.Migrations
{
    [DbContext(typeof(AccountsContext))]
    [Migration("20230617155444_AddCashAccounts")]
    partial class AddCashAccounts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
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

                    b.ToTable("outbox_messages", "accounts");
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

                    b.ToTable("inbox_messages", "accounts");
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

                    b.ToTable("internal_commands", "accounts");
                });

            modelBuilder.Entity("App.Modules.Accounts.Domain.Accounts.CashAccounts.CashAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<int>("_balance")
                        .HasColumnType("integer")
                        .HasColumnName("balance");

                    b.Property<DateTime>("_createdAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("cash_accounts", "accounts");
                });

            modelBuilder.Entity("App.Modules.Accounts.Domain.Accounts.CashAccounts.CashAccount", b =>
                {
                    b.OwnsOne("App.Modules.Accounts.Domain.Accounts.Currency", "_currency", b1 =>
                        {
                            b1.Property<Guid>("CashAccountId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("currency");

                            b1.HasKey("CashAccountId");

                            b1.ToTable("cash_accounts", "accounts");

                            b1.WithOwner()
                                .HasForeignKey("CashAccountId");
                        });

                    b.Navigation("_currency");
                });
#pragma warning restore 612, 618
        }
    }
}
