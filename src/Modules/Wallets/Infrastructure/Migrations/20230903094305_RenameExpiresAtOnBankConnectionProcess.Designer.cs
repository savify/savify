﻿// <auto-generated />
using System;
using App.Modules.Wallets.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    [DbContext(typeof(WalletsContext))]
    [Migration("20230903094305_RenameExpiresAtOnBankConnectionProcess")]
    partial class RenameExpiresAtOnBankConnectionProcess
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.ToTable("outbox_messages", "wallets");
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

                    b.ToTable("inbox_messages", "wallets");
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

                    b.ToTable("internal_commands", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.BankConnectionProcessing.BankConnectionProcess", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BankId")
                        .HasColumnType("uuid")
                        .HasColumnName("bank_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid")
                        .HasColumnName("wallet_id");

                    b.Property<DateTime>("_initiatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("initiated_at");

                    b.Property<string>("_redirectUrl")
                        .HasColumnType("text")
                        .HasColumnName("redirect_url");

                    b.Property<DateTime?>("_redirectUrlExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("redirect_url_expires_at");

                    b.Property<DateTime?>("_updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("bank_connection_processes", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.BankConnections.BankConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("_bankId")
                        .HasColumnType("uuid")
                        .HasColumnName("bank_id");

                    b.Property<DateTime>("_createdAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("_refreshedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refreshed_at");

                    b.Property<Guid>("_userId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("bank_connections", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.CashWallets.CashWallet", b =>
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

                    b.Property<bool>("_isRemoved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_removed");

                    b.Property<DateTime?>("_removedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("removed_at");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime?>("_updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("cash_wallets", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.CreditWallets.CreditWallet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<int>("_availableBalance")
                        .HasColumnType("integer")
                        .HasColumnName("available_balance");

                    b.Property<DateTime>("_createdAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("_creditLimit")
                        .HasColumnType("integer")
                        .HasColumnName("credit_limit");

                    b.Property<bool>("_isRemoved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_removed");

                    b.Property<DateTime?>("_removedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("removed_at");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime?>("_updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("credit_wallets", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.DebitWallets.DebitWallet", b =>
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

                    b.Property<bool>("_isRemoved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_removed");

                    b.Property<DateTime?>("_removedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("removed_at");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime?>("_updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("debit_wallets", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.WalletViewMetadata.WalletViewMetadata", b =>
                {
                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid")
                        .HasColumnName("wallet_id");

                    b.Property<string>("Color")
                        .HasColumnType("text")
                        .HasColumnName("color");

                    b.Property<string>("Icon")
                        .HasColumnType("text")
                        .HasColumnName("icon");

                    b.Property<bool>("IsConsideredInTotalBalance")
                        .HasColumnType("boolean")
                        .HasColumnName("is_considered_in_total_balance");

                    b.HasKey("WalletId");

                    b.ToTable("wallet_view_metadata", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections.SaltEdgeConnection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country_code");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("customer_id");

                    b.Property<Guid>("InternalConnectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("internal_connection_id");

                    b.Property<string>("LastConsentId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_consent_id");

                    b.Property<string>("ProviderCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("provider_code");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("salt_edge_connections", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers.SaltEdgeCustomer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<Guid>("Identifier")
                        .HasColumnType("uuid")
                        .HasColumnName("identifier");

                    b.HasKey("Id");

                    b.ToTable("salt_edge_customers", "wallets");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.BankConnectionProcessing.BankConnectionProcess", b =>
                {
                    b.OwnsOne("App.Modules.Wallets.Domain.BankConnectionProcessing.BankConnectionProcessStatus", "_status", b1 =>
                        {
                            b1.Property<Guid>("BankConnectionProcessId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("status");

                            b1.HasKey("BankConnectionProcessId");

                            b1.ToTable("bank_connection_processes", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("BankConnectionProcessId");
                        });

                    b.OwnsOne("App.Modules.Wallets.Domain.Wallets.WalletType", "_walletType", b1 =>
                        {
                            b1.Property<Guid>("BankConnectionProcessId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("wallet_type");

                            b1.HasKey("BankConnectionProcessId");

                            b1.ToTable("bank_connection_processes", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("BankConnectionProcessId");
                        });

                    b.Navigation("_status");

                    b.Navigation("_walletType");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.BankConnections.BankConnection", b =>
                {
                    b.OwnsMany("App.Modules.Wallets.Domain.BankConnections.BankAccounts.BankAccount", "_accounts", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<Guid>("BankConnectionId")
                                .HasColumnType("uuid")
                                .HasColumnName("bank_connection_id");

                            b1.Property<int>("Amount")
                                .HasColumnType("integer")
                                .HasColumnName("amount");

                            b1.Property<string>("_externalId")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("external_id");

                            b1.Property<string>("_name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name");

                            b1.HasKey("Id", "BankConnectionId");

                            b1.HasIndex("BankConnectionId");

                            b1.ToTable("bank_accounts", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("BankConnectionId");

                            b1.OwnsOne("App.Modules.Wallets.Domain.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("BankAccountId")
                                        .HasColumnType("uuid");

                                    b2.Property<Guid>("BankAccountBankConnectionId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("currency");

                                    b2.HasKey("BankAccountId", "BankAccountBankConnectionId");

                                    b2.ToTable("bank_accounts", "wallets");

                                    b2.WithOwner()
                                        .HasForeignKey("BankAccountId", "BankAccountBankConnectionId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.OwnsOne("App.Modules.Wallets.Domain.BankConnections.Consent", "_consent", b1 =>
                        {
                            b1.Property<Guid>("BankConnectionId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("ExpiresAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("consent_expires_at");

                            b1.HasKey("BankConnectionId");

                            b1.ToTable("bank_connections", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("BankConnectionId");
                        });

                    b.Navigation("_accounts");

                    b.Navigation("_consent");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.CashWallets.CashWallet", b =>
                {
                    b.OwnsOne("App.Modules.Wallets.Domain.Currency", "_currency", b1 =>
                        {
                            b1.Property<Guid>("CashWalletId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("currency");

                            b1.HasKey("CashWalletId");

                            b1.ToTable("cash_wallets", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("CashWalletId");
                        });

                    b.Navigation("_currency");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.CreditWallets.CreditWallet", b =>
                {
                    b.OwnsOne("App.Modules.Wallets.Domain.Currency", "_currency", b1 =>
                        {
                            b1.Property<Guid>("CreditWalletId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("currency");

                            b1.HasKey("CreditWalletId");

                            b1.ToTable("credit_wallets", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("CreditWalletId");
                        });

                    b.Navigation("_currency");
                });

            modelBuilder.Entity("App.Modules.Wallets.Domain.Wallets.DebitWallets.DebitWallet", b =>
                {
                    b.OwnsOne("App.Modules.Wallets.Domain.Wallets.BankAccountConnections.BankAccountConnection", "_bankAccountConnection", b1 =>
                        {
                            b1.Property<Guid>("DebitWalletId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("BankAccountId")
                                .HasColumnType("uuid")
                                .HasColumnName("bank_account_id");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("bank_connection_id");

                            b1.HasKey("DebitWalletId");

                            b1.ToTable("debit_wallets", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("DebitWalletId");
                        });

                    b.OwnsOne("App.Modules.Wallets.Domain.Currency", "_currency", b1 =>
                        {
                            b1.Property<Guid>("DebitWalletId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("currency");

                            b1.HasKey("DebitWalletId");

                            b1.ToTable("debit_wallets", "wallets");

                            b1.WithOwner()
                                .HasForeignKey("DebitWalletId");
                        });

                    b.Navigation("_bankAccountConnection");

                    b.Navigation("_currency");
                });
#pragma warning restore 612, 618
        }
    }
}
