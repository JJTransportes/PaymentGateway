using PaymentGateway.Dtos;
using PaymentGateway.Dtos.Customers;
using PaymentGateway.Errors;
using PaymentGateway.Interfaces;

namespace PaymentGateway.Endpoints.Customers;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers");

        group.MapPost("/{userId:guid}", async (
        Guid userId,
        NewPaymentGatewayCustomerDto dto,
        ICustomerRepository repository,
        CancellationToken ct) =>
        {
            var error = await repository.CreateCustomerAsync(userId, dto, ct);

            return error != null ?
                _BuildCustomerFailureResponse(error!) :
                _BuildCustomerCreationSuccessfullResponse(userId);
        });
    }

    private static IResult _BuildCustomerCreationSuccessfullResponse(Guid userId)
    {
        return Results.Created(
          $"/customers/{userId}",
          new ResponseDto<string>
          {
              Data = "Parabéns!",
              Message = "Cliente cadastrado na central financeira."
          });
    }

    private static IResult _BuildCustomerFailureResponse(ApiBaseError error)
    {
        int? statusCode = error.ErrorType switch
        {
            ErrorType.PaymentGatewayCreationFailure => StatusCodes.Status400BadRequest,
            ErrorType.CustomerAlreadyRegistered => StatusCodes.Status409Conflict,
            _ => null
        };

        if (statusCode is null)
        {
            return Results.InternalServerError(new ResponseDto<string?>
            {
                Data = null,
                Message = "Erro desconhecido ao processar cliente."
            });
        }

        return Results.Json(
          new ResponseDto<ErrorType>
          {
              Data = error.ErrorType,
              Message = error.Message,
              Errors = error.Errors
          },
          statusCode: statusCode);
    }
}
