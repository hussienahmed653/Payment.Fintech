namespace Payment.Fintech.Domain.Errors;

public record MerchantErrors
{
    public static readonly Error EmailDublicated = 
        new("MERCHANT_EMAIL_DUBLICATED", "Merchant email is already in use.", StatusCodes.Status409Conflict);
    public static readonly Error MerchantNotFound =
        new("MERCHANT_NOTFOUND", "Merchant not found.", StatusCodes.Status404NotFound);
    public static readonly Error InvalidGuid =
        new("MERCHANT_INVALID_GUID", "Invalid merchant GUID.", StatusCodes.Status400BadRequest);
    public static readonly Error BusinessTypeNotFound =
        new("MERCHANT_BUSINESS_TYPE_NOT_FOUND", "No merchants found for the specified business type.", StatusCodes.Status404NotFound);
    public static readonly Error SearchNotFound =
        new("MERCHANT_SEARCH_NOT_FOUND", "No merchant was found matching the search criteria.", StatusCodes.Status404NotFound);
    public static readonly Error SearchKeyWordNotFound =
        new("MERCHANT_SEARCH_KEY_WORD_NOT_FOUND", "You Shoud Pass Search Key Word", StatusCodes.Status204NoContent);
    public static readonly Error Filter =
        new("NO_MERCHANT_MATCHES_FILTERS", "We Couldn't Find Any Merchant Matches Your Filters", StatusCodes.Status404NotFound);
    public static readonly Error ZeroRowsAffected =
        new("NO_ROWS_AFFECTED", "No Rows Affected. Expected (1) Row Affected", StatusCodes.Status400BadRequest);
    public static readonly Error MultibleRowsAffected =
        new("MULTIBLE_MERCHANT_ROWS_AFFECTED", "Multible Rows Affected. Expected (1) Row Affected", StatusCodes.Status400BadRequest);
}
