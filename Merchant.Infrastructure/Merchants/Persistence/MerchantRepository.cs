using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Merchant.Infrastructure.Merchants.Persistence;

internal class MerchantRepository(ApplicationDbContext context) : IMerchantRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<MerchantResponse> CreateMerchantAsync(MerchantRequest request, CancellationToken cancellationToken = default)
    {
        var merchant = request.Adapt<Domain.Entities.Merchant>();
        await _context.Merchants.AddAsync(merchant, cancellationToken);
        return merchant.Adapt<MerchantResponse>();
    }

    public async Task DeleteMerchantAsync(Domain.Entities.Merchant merchant, CancellationToken cancellationToken = default)
    {
        _context.Merchants.Remove(merchant);
    }

    public async Task<bool> EmailIsExistsAsync(string email, CancellationToken cancellationToken = default) =>
        await _context.Merchants.AnyAsync(m => m.Email == email, cancellationToken);

    public async Task<IEnumerable<Domain.Entities.Merchant>> FilterAsync(MerchantFilterSpecification spec, CancellationToken cancellationToken = default)
    {
        return await _context.Merchants
            .AsNoTracking()
            .Where(spec.ToExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MerchantResponse>> GetMerchantAsync(CancellationToken cancellationToken = default) =>
        _context.Merchants
            .AsNoTracking()
            .Adapt<IEnumerable<MerchantResponse>>();

    public async Task<IEnumerable<MerchantResponse>> GetMerchantByBusinessTypeAsync(string businessType, CancellationToken cancellationToken = default) =>
        await _context.Merchants
            .Where(m => m.BusinessType.ToString().Equals(businessType))
            .AsNoTracking()
            .ProjectToType<MerchantResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Domain.Entities.Merchant> GetMerchantByGuidAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _context.Merchants
            .Where(m => m.GuidId == guid)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
    public async Task<bool> MerchantIsExistsAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _context.Merchants
            .AsNoTracking()
            .AnyAsync(m => m.GuidId == guid, cancellationToken);

    public async Task<IEnumerable<Domain.Entities.Merchant>> SearchAsync(string search, CancellationToken cancellationToken = default)
    {
        return await _context.Merchants.Where(m =>
        m.ContactFirstName.Contains(search) ||
        m.ContactLastName.Contains(search) ||
        m.Email.Contains(search) ||
        m.Phone.Contains(search) ||
        m.BusinessName.Contains(search))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateMerchantAsync(Domain.Entities.Merchant merchant, CancellationToken cancellationToken = default) =>
        _context.Merchants.Update(merchant);
}