using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Infrastructure.Domain.Users;
using App.Modules.UserAccess.Infrastructure.Inbox;
using App.Modules.UserAccess.Infrastructure.InternalCommands;
using App.Modules.UserAccess.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure;

public class UserAccessContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    
    public DbSet<OutboxMessage>? OutboxMessages { get; set; }
    
    public DbSet<InboxMessage>? InboxMessages { get; set; }
    
    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public UserAccessContext(DbContextOptions<UserAccessContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
