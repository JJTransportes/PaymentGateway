namespace PaymentGateway.Models.Info;

public class UserMetadata
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Value { get; init; } = string.Empty;
}