using IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence;

public sealed class IdentityDbContext
    : DbContext
{
    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<OAuthApplication> Applications { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder);
        ConfigureOrganization(modelBuilder);
        ConfigureOrganizationMember(modelBuilder);
        ConfigureApplication(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<User>();

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .IsRequired();
    }

    private static void ConfigureOrganization(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<Organization>();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OwnerUserId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }

    private static void ConfigureOrganizationMember(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<OrganizationMember>();

        builder.HasKey(x => new { x.OrganizationId, x.UserId });

        builder.Property(x => x.Role)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.JoinedAt)
            .IsRequired();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.OrganizationId);
    }

    private static void ConfigureApplication(ModelBuilder modelBuilder)
    {
        var b = modelBuilder.Entity<OAuthApplication>();
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.ClientId).IsUnique();
        b.Property(x => x.ClientId).IsRequired().HasMaxLength(100);
        b.Property(x => x.ClientSecretHash).IsRequired();
        b.Property(x => x.Name).IsRequired().HasMaxLength(200);
        b.Property(x => x.RedirectUris).HasMaxLength(2000);
        b.Property(x => x.IsConfidential).IsRequired();
        b.Property(x => x.CreatedAt).IsRequired();
    }

    private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        var b = modelBuilder.Entity<RefreshToken>();
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.ApplicationId);
        b.Property(x => x.TokenHash).IsRequired().HasMaxLength(200);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.ExpiresAt).IsRequired();
    }
}
