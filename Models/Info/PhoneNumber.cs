using PaymentGateway.Enums;

namespace PaymentGateway.Models.Info;

public class PhoneNumber
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Number { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public PhoneType Type { get; set; }
}
