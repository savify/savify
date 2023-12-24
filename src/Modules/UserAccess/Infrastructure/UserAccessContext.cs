using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Infrastructure.Domain.PasswordResetRequests;
using App.Modules.UserAccess.Infrastructure.Domain.UserRegistrations;
using App.Modules.UserAccess.Infrastructure.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure;

public class UserAccessContext(DbContextOptions<UserAccessContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }

    public required DbSet<UserRegistration> UserRegistrations { get; set; }

    public required DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

    public required DbSet<OutboxMessage> OutboxMessages { get; set; }

    public required DbSet<InboxMessage> InboxMessages { get; set; }

    public required DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserRegistrationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordResetRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
