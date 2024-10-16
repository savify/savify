﻿// <auto-generated />
using System;
using App.Modules.Notifications.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Modules.Notifications.Infrastructure.Migrations
{
    [DbContext(typeof(NotificationsContext))]
    [Migration("20230605181936_ModuleInit")]
    partial class ModuleInit
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

                    b.ToTable("outbox_messages", "notifications");
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

                    b.ToTable("inbox_messages", "notifications");
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

                    b.ToTable("internal_commands", "notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
