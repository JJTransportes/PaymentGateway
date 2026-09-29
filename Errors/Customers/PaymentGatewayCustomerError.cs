namespace PaymentGateway.Errors.Customers;

public class PaymentGatewayCustomerError : ApiBaseError
{
  public PaymentGatewayCustomerError(
    string message,
    ICollection<string> errors,
    ErrorType errorType) : base(message, errors, errorType)
  {
  }
}
