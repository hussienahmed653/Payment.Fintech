namespace PaymentRequest.Application.Common.Interfaces.Merchants;

public interface IPaymentRepository
{
    //Task<IEnumerable<MerchantResponse>> GetMerchantAsync(CancellationToken cancellationToken = default);
    //Task<IEnumerable<MerchantResponse>> GetMerchantByBusinessTypeAsync(string businessType, CancellationToken cancellationToken = default);
    //Task<Domain.Entities.PaymentRequest> GetMerchantByGuidAsync(Guid id, CancellationToken cancellationToken = default);
    //Task<IEnumerable<Domain.Entities.PaymentRequest>> SearchAsync(string search, CancellationToken cancellationToken = default);
    //Task<IEnumerable<Domain.Entities.PaymentRequest>> FilterAsync(MerchantFilterSpecification spec, CancellationToken cancellationToken = default);
    //Task<bool> MerchantIsExistsAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<bool> ReferenceIsExistsAsync(string reference, CancellationToken cancellationToken = default);
    Task<Domain.Entities.PaymentRequest> CreatePaymentRequestAsync(Contracts.Merchants.PaymentRequest request, CancellationToken cancellationToken = default);
    //Task UpdateMerchantAsync(Domain.Entities.PaymentRequest merchant, CancellationToken cancellationToken = default);
    //Task DeleteMerchantAsync(Domain.Entities.PaymentRequest merchant, CancellationToken cancellationToken = default);
}
