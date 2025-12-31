using AuditService.Application.Records;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Infrastructure.Persistence;

public sealed class AuditDbContext(DbContextOptions<AuditDbContext> options)
    : DbContext(options)
{
    public DbSet<AuditRecord> AuditRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<AuditRecord>();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Payload)
            .IsRequired();

        builder.Property(x => x.CorrelationId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.OccurredAt)
            .IsRequired();

        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.CorrelationId);
        builder.HasIndex(x => x.OccurredAt);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditRecord>())
        {
            if (entry.State is EntityState.Modified or EntityState.Deleted)
            {
                throw new InvalidOperationException(
                    "Audit records are immutable.");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
