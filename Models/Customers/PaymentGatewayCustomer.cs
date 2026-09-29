using PaymentGateway.Models.Info;

namespace PaymentGateway.Models.Customers;

public class PaymentGatewayCustomer
{
    public Guid UserId { get; set; }
    public string GatewayId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public IEnumerable<PhoneNumber> Phones { get; set; } = new List<PhoneNumber>();
    public IEnumerable<UserMetadata> Metadata { get; set; } = new List<UserMetadata>();
}
