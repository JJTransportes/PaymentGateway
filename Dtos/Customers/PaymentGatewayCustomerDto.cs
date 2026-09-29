
using PaymentGateway.Dtos.Info.Address;

namespace PaymentGateway.Dtos.Customers;

public record PaymentGatewayCustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public NewAddressDto? Address { get; set; }
}
