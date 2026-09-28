using PaymentProvider.Application.Common.Interfaces.Processor;

namespace PaymentProvider.Application.Common.Interfaces.FactorySelector;

public interface ISelectorFactory
{
    IPaymentProcessor Select(int selectionId);
}
