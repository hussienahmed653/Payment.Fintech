using System.Linq.Expressions;

namespace PaymentRequest.Application.Common.Contracts.Merchants;

public class PaymentFilterSpecification(PaymentFilterRequest request)
{
    private readonly PaymentFilterRequest request = request;
    //public Expression<Func<Domain.Entities.PaymentRequest, bool>> ToExpression()
    //{
    //    return m => (string.IsNullOrWhiteSpace(request.ContactFirstName) || request.ContactFirstName == m.ContactFirstName)
    //                && (string.IsNullOrWhiteSpace(request.ContactLastName) || request.ContactLastName == m.ContactLastName)
    //                && (string.IsNullOrWhiteSpace(request.Phone) || request.Phone == m.Phone)
    //                && (string.IsNullOrWhiteSpace(request.BusinessName) || request.BusinessName == m.BusinessName)
    //                && (string.IsNullOrWhiteSpace(request.BusinessType) || request.BusinessType == m.BusinessType.ToString())
    //                && (string.IsNullOrWhiteSpace(request.Status) || request.Status == m.Status.ToString())
    //                && (string.IsNullOrWhiteSpace(request.Currency) || request.Currency == m.Currency);
    //}
}
