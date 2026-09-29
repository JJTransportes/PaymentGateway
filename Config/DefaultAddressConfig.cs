namespace PaymentGateway.Config;

public class DefaultAddressConfig
{
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Neighborhood { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string Complement { get; init; } = string.Empty;
    public string Line1 { get; init; } = string.Empty;
    public string Line2 { get; init; } = string.Empty;
}
