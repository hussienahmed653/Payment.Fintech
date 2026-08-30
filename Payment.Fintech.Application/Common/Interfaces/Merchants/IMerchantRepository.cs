namespace Payment.Fintech.Application.Common.Interfaces.Merchants;

public interface IMerchantRepository
{
    Task<IEnumerable<MerchantResponse>> GetMerchantAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MerchantResponse>> GetMerchantByBusinessTypeAsync(string businessType, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Merchant> GetMerchantByGuidAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Merchant>> SearchAsync(string search, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Merchant>> FilterAsync(MerchantFilterSpecification spec, CancellationToken cancellationToken = default);
    Task<bool> MerchantIsExistsAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<bool> EmailIsExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<MerchantResponse> CreateMerchantAsync(MerchantRequest request, CancellationToken cancellationToken = default);
    Task UpdateMerchantAsync(Domain.Entities.Merchant merchant, CancellationToken cancellationToken = default);
    Task DeleteMerchantAsync(Domain.Entities.Merchant merchant, CancellationToken cancellationToken = default);
}
