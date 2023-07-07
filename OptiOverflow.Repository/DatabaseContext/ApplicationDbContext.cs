using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Entities;

namespace OptiOverflow.Repository.DatabaseContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid,
    IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    #region Override Methods

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.LastUpdate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastUpdate = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Entity.LastUpdate = DateTime.UtcNow;
                    break;
            }
        }
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

    #endregion

    public DbSet<Question>? Question { get; set; }
    public DbSet<Answer>? Answer { get; set; }
    public DbSet<Vote>? Vote { get; set; }

}