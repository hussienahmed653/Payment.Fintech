namespace PaymentProvider.Infrastructure.FactorySelectors.Persistence;

internal class SelectorFactory(IServiceProvider serviceProvider) : ISelectorFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IPaymentProcessor Select(int selectionId)
    {
        return selectionId switch
        {
            (int)PaymentProviderType.PawaPay => _serviceProvider.GetRequiredKeyedService<IPaymentProcessor>((int)PaymentProviderType.PawaPay),
            (int)PaymentProviderType.MMO => _serviceProvider.GetRequiredKeyedService<IPaymentProcessor>((int)PaymentProviderType.MMO),
            _ => throw new ArgumentException($"Invalid selectionId: {selectionId}")
        };
    }
}
