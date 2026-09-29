namespace PaymentGateway.Errors;

public abstract class ApiBaseError
{
  public ApiBaseError(string message, ICollection<string> errors, ErrorType errorType)
  {
    Message = message;
    Errors = errors;
    ErrorType = errorType;
  }

  public string Message { get; init; } = string.Empty;
  public ICollection<string> Errors { get; init; } = [];
  public ErrorType ErrorType { get; init; }
}
