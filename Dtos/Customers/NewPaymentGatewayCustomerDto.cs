using PaymentGateway.Dtos.Info.PhoneNumber;

namespace PaymentGateway.Dtos.Customers;

public record NewPaymentGatewayCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public ICollection<NewPhoneNumberDto> Phones { get; set; } = new List<NewPhoneNumberDto>();
}
