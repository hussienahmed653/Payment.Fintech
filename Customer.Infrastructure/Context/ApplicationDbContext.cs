namespace Customer.Infrastructure.Context;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<Domain.Entities.Customer> Customers { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var FK in cascadeFKs)
            FK.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userGuid = Guid.CreateVersion7(); //_httpContextAccessor.HttpContext?.User.GetUserGuid()!;
        var entries = ChangeTracker.Entries<AuditableEntity>();
        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                entityEntry.Property(a => a.CreatedByGuid).CurrentValue = userGuid;
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.UpdatedByGuid).CurrentValue = userGuid;
                entityEntry.Property(a => a.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
internal static class UserExtensions
{
    public static Guid GetUserGuid(this ClaimsPrincipal user) => Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
}