namespace Persistence
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Common;
    using Domain.Common;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AuctionSystemDbContext : IdentityDbContext<AuctionUser>, IAuctionSystemDbContext
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public AuctionSystemDbContext(DbContextOptions<AuctionSystemDbContext> options)
            : base(options)
        {
        }

        public AuctionSystemDbContext(
            DbContextOptions<AuctionSystemDbContext> options,
            IDateTime dateTime,
            ICurrentUserService currentUserService)
            : base(options)
        {
            this.dateTime = dateTime;
            this.currentUserService = currentUserService;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUserService?.UserId;
                        entry.Entity.Created = dateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = currentUserService?.UserId;
                        entry.Entity.LastModified = dateTime.UtcNow;
                        break;
                }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionSystemDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}