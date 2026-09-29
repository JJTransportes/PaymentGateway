namespace PaymentGateway.Config;

public class AppConfig
{
    public static string SectionName = "AppConfig";
    public int Port { get; init; }
    public string Service { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
    public string MessageHost { get; init; } = string.Empty;
    public string MessageUser { get; init; } = string.Empty;
    public string MessagePassword { get; init; } = string.Empty;
    public TimeOnly
    Time
    { get; init; } = TimeOnly.FromDateTime(DateTime.UtcNow);
    public string PaymentUrl { get; init; } = string.Empty;
    public string PaymentGatewayKey { get; init; } = string.Empty;
    public string PaymentGatewayCustomerType { get; init; } = string.Empty;
    public string PaymentGatewayDocumentType { get; init; } = string.Empty;
    public DefaultAddressConfig DefaultAddress { get; init; } = new();
}
