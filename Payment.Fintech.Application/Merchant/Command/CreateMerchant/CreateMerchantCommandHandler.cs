using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Payment.Fintech.Application.Abstraction;
using Payment.Fintech.Application.Common.Contracts.Merchants;
using Payment.Fintech.Application.Common.Interfaces.Merchants;
using Payment.Fintech.Application.Common.Interfaces.UnitOfWork;
using Payment.Fintech.Domain.Errors;

namespace Payment.Fintech.Application.Merchant.Command.CreateMerchant;

public class CreateMerchantCommandHandler(IUnitOfWork unitOfWork, IMerchantRepository merchantRepository) : IRequestHandler<CreateMerchantCommand, Result<MerchantResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<MerchantResponse>> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
                return Result.Failure<MerchantResponse>(MerchantErrors.EmailDublicated);
            var merchant = await _merchantRepository.CreateMerchantAsync(request.Request, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant);
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<MerchantResponse>(new Error("CreateMerchantFailed", ex.Message, StatusCodes.Status500InternalServerError));
        }
    }
}
