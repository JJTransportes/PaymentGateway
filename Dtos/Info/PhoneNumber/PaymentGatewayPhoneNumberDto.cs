namespace PaymentGateway.Dtos.Info.PhoneNumber;

public record PaymentGatewayPhoneNumberDto(
 string Number,
 string AreaCode,
 string CountryCode,
 string Type
);