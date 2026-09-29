using PaymentGateway.Dtos.Customers;
using PaymentGateway.Errors;
using PaymentGateway.Models.Customers;

namespace PaymentGateway.Interfaces;

public interface ICustomerRepository
{
    Task<ApiBaseError?> CreateCustomerAsync(
        Guid userId,
        NewPaymentGatewayCustomerDto request,
        CancellationToken cancellationToken = default
      );
}
