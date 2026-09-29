using PaymentGateway.Enums;

namespace PaymentGateway.Dtos.Info.PhoneNumber;

public record NewPhoneNumberDto
{
    public string Number { get; init; } = string.Empty;
    public string AreaCode { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;
    public PhoneType Type { get; init; }
}
