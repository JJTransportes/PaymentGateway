namespace PaymentGateway.Dtos.Info.Address;

public record PaymentGatewayAddressDto(
    string Street,
    string Number,
    string ZipCode,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string Complement,
    string Line1,
    string Line2
);