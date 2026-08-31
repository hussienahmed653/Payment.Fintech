namespace Customer.Infrastructure.Customers.Persistence;

internal class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = request.Adapt<Domain.Entities.Customer>();
        await _context.Customers.AddAsync(customer, cancellationToken);
        return customer.Adapt<CustomerResponse>();
    }

    public async Task DeleteCustomerAsync(Domain.Entities.Customer customer, CancellationToken cancellationToken = default) =>
        _context.Customers.Remove(customer);

    public async Task<bool> EmailIsExistsAsync(string email, CancellationToken cancellationToken = default) =>
        await _context.Customers.AnyAsync(m => m.Email == email, cancellationToken);

    public async Task<IEnumerable<Domain.Entities.Customer>> FilterAsync(CustomerFilterSpecification spec, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(spec.ToExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomerResponse>> GetCustomerAsync(CancellationToken cancellationToken = default) =>
        _context.Customers
            .AsNoTracking()
            .Adapt<IEnumerable<CustomerResponse>>();

    public async Task<IEnumerable<CustomerResponse>> GetCustomerByBusinessTypeAsync(string businessType, CancellationToken cancellationToken = default) =>
        await _context.Customers
            .Where(m => m.BusinessType.ToString().Equals(businessType))
            .AsNoTracking()
            .ProjectToType<CustomerResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Domain.Entities.Customer> GetCustomerByGuidAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _context.Customers
            .Where(m => m.GuidId == guid)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
    public async Task<bool> CustomerIsExistsAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _context.Customers
            .AsNoTracking()
            .AnyAsync(m => m.GuidId == guid, cancellationToken);

    public async Task<IEnumerable<Domain.Entities.Customer>> SearchAsync(string search, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.Where(c =>
        c.ContactFirstName.Contains(search) ||
        c.ContactLastName.Contains(search) ||
        c.Email.Contains(search) ||
        c.Phone.Contains(search) ||
        c.BusinessName.Contains(search))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateCustomerAsync(Domain.Entities.Customer customer, CancellationToken cancellationToken = default) =>
        _context.Customers.Update(customer);
}