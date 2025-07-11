using Domain.Abstractions;
using Domain.Companies;
using Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Context;
internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        
        // Check if HttpContext exists
        if (httpContextAccessor.HttpContext?.User?.Claims != null)
        {
            var userIdClaim = httpContextAccessor
                .HttpContext
                .User
                .Claims
                .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(p => p.CreateAt)
                            .CurrentValue = DateTime.Now;
                        entry.Property(p => p.CreateUserId)
                            .CurrentValue = userId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                        {
                            entry.Property(p => p.DeleteAt)
                            .CurrentValue = DateTime.Now;
                            entry.Property(p => p.DeleteUserId)
                            .CurrentValue = userId;
                        }
                        else
                        {
                            entry.Property(p => p.UpdateAt)
                                .CurrentValue = DateTime.Now;
                            entry.Property(p => p.UpdateUserId)
                            .CurrentValue = userId;
                        }
                    }

                    if (entry.State == EntityState.Deleted)
                    {
                        throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
                    }
                }
            }
            else
            {
                // No valid user context - handle entities without user info
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(p => p.CreateAt)
                            .CurrentValue = DateTime.Now;
                        // Don't set CreateUserId if no user context
                    }

                    if (entry.State == EntityState.Deleted)
                    {
                        throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
                    }
                }
            }
        }
        else
        {
            // No HttpContext - likely during migration or seeding
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreateAt)
                        .CurrentValue = DateTime.Now;
                    // Don't set CreateUserId if no HttpContext
                }

                if (entry.State == EntityState.Deleted)
                {
                    throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
