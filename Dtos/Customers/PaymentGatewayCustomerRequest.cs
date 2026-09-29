using PaymentGateway.Dtos.Info.Address;
using PaymentGateway.Dtos.Info.PhoneNumber;

namespace PaymentGateway.Dtos.Customers;

public record PaymentGatewayCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public PaymentGatewayAddressDto Address { get; init; } = null!;
    public Dictionary<string, PaymentGatewayPhoneNumberDto> Phones { get; init; } = null!;
    public Dictionary<string, string> Metadata { get; init; } = null!;
}