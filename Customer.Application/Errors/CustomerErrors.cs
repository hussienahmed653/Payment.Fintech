namespace Customer.Application.Errors;

public record CustomerErrors
{
    public static readonly Error EmailDublicated =
        new("CUSTOMER_EMAIL_DUBLICATED", "Customer email is already in use.", StatusCodes.Status409Conflict);
    public static readonly Error CustomerNotFound =
        new("CUSTOMER_NOTFOUND", "Customer not found.", StatusCodes.Status404NotFound);
    public static readonly Error InvalidGuid =
        new("CUSTOMER_INVALID_GUID", "Invalid Customer GUID.", StatusCodes.Status400BadRequest);
    public static readonly Error BusinessTypeNotFound =
        new("CUSTOMER_BUSINESS_TYPE_NOT_FOUND", "No Customers found for the specified business type.", StatusCodes.Status404NotFound);
    public static readonly Error SearchNotFound =
        new("CUSTOMER_SEARCH_NOT_FOUND", "No Customer was found matching the search criteria.", StatusCodes.Status404NotFound);
    public static readonly Error SearchKeyWordNotFound =
        new("CUSTOMER_SEARCH_KEY_WORD_NOT_FOUND", "You Shoud Pass Search Key Word", StatusCodes.Status204NoContent);
    public static readonly Error Filter =
        new("NO_CUSTOMER_MATCHES_FILTERS", "We Couldn't Find Any Customer Matches Your Filters", StatusCodes.Status404NotFound);
    public static readonly Error ZeroRowsAffected =
        new("NO_ROWS_AFFECTED", "No Rows Affected. Expected (1) Row Affected", StatusCodes.Status400BadRequest);
    public static readonly Error MultibleRowsAffected =
        new("MULTIBLE_CUSTOMER_ROWS_AFFECTED", "Multible Rows Affected. Expected (1) Row Affected", StatusCodes.Status400BadRequest);
}
