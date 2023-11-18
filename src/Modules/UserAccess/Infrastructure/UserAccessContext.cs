using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Infrastructure.Domain.PasswordResetRequests;
using App.Modules.UserAccess.Infrastructure.Domain.UserRegistrations;
using App.Modules.UserAccess.Infrastructure.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure;

public class UserAccessContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public DbSet<UserRegistration>? UserRegistrations { get; set; }

    public DbSet<PasswordResetRequest>? PasswordResetRequests { get; set; }

    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public UserAccessContext(DbContextOptions<UserAccessContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(UserAccessModule.DatabaseSchemaName);

        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserRegistrationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordResetRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
