using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PaymentGateway.Config;
using PaymentGateway.Data;
using PaymentGateway.Dtos.Customers;
using PaymentGateway.Dtos.Info.Address;
using PaymentGateway.Dtos.Info.PhoneNumber;
using PaymentGateway.Enums;
using PaymentGateway.Errors;
using PaymentGateway.Errors.Customers;
using PaymentGateway.Interfaces;
using RestSharp;

namespace PaymentGateway.Repositories;

public class CustomerRepository(
    AppDbContext db,
    IOptions<AppConfig> options,
    IRestClient httpClient) : ICustomerRepository
{
    private static readonly JsonSerializerOptions GatewayJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        MaxDepth = 4,
    };

    public async Task<ApiBaseError?> CreateCustomerAsync(
        Guid userId,
        NewPaymentGatewayCustomerDto dto,
        CancellationToken cancellationToken = default
      )
    {
        try
        {
            var registeredCustomer = await db.Customers
                .AsNoTracking()
                .AnyAsync(
                    customer => customer.UserId == userId
                        || customer.Document == dto.Document
                        || customer.Email == dto.Email,
                    cancellationToken);

            if (registeredCustomer)
            {
                return new PaymentGatewayCustomerError(
                    "Ops... Este cliente já foi cadastrado na central financeira.",
                    [],
                    ErrorType.CustomerAlreadyRegistered
                    );
            }

            RestRequest newCustomerRequest = new RestRequest("v5/customers", Method.Post);
            var serializedRequest = _BuildNewCustomerRequest(userId, dto);
            var requestBody = JsonSerializer.Serialize(serializedRequest, GatewayJsonOptions);

            newCustomerRequest.AddBody(requestBody);
            var response = await httpClient.ExecuteAsync<PaymentGatewayCustomerDto>(newCustomerRequest, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK || response.Data is null)
            {
                var json = JsonDocument.Parse(response.Content ?? "");
                var errorResult = json.RootElement.GetProperty("message").GetString();

                return new PaymentGatewayCustomerError(
                    "Ops... Erro ao cadastrar cliente na central financeira.",
                    [errorResult ?? ""],
                    ErrorType.PaymentGatewayCreationFailure
                    );
            }

            return null;
        }
        catch (HttpRequestException exception)
        {
            return new PaymentGatewayCustomerError(
                $"Ops... {exception.Message}",
                [],
                ErrorType.PaymentGatewayCreationFailure
                );
        }
    }

    private PaymentGatewayCustomerRequest _BuildNewCustomerRequest(Guid userId, NewPaymentGatewayCustomerDto dto)
    {
        return new PaymentGatewayCustomerRequest
        {
            Name = dto.Name,
            Email = dto.Email,
            Document = dto.Document,
            Type = options.Value.PaymentGatewayCustomerType,
            DocumentType = options.Value.PaymentGatewayDocumentType,
            Gender = dto.Gender,
            Address = new PaymentGatewayAddressDto(
                Street: options.Value.DefaultAddress.Street,
                Number: options.Value.DefaultAddress.Number,
                ZipCode: options.Value.DefaultAddress.ZipCode,
                Neighborhood: options.Value.DefaultAddress.Neighborhood,
                City: options.Value.DefaultAddress.City,
                State: options.Value.DefaultAddress.State,
                Country: options.Value.DefaultAddress.Country,
                Complement: options.Value.DefaultAddress.Complement,
                Line1: options.Value.DefaultAddress.Line1,
                Line2: options.Value.DefaultAddress.Line2
            ),
            Phones = _BuildGatewayPhones(dto.Phones),
            Metadata = new Dictionary<string, string>
            {
                ["user_role"] = "Motorista",
                ["created_at"] = DateTime.UtcNow.ToString("dd/mm/yyyy"),
                ["customer_id"] = userId.ToString(),
            }
        };
    }

    private static Dictionary<string, PaymentGatewayPhoneNumberDto> _BuildGatewayPhones(
        ICollection<NewPhoneNumberDto> phones
        )
    {
        return phones
            .GroupBy(phone => _ResolvePhoneSlot(phone.Type))
            .ToDictionary(
                slot => slot.Key,
                slot => new PaymentGatewayPhoneNumberDto(
                    Type: slot.Key,
                    CountryCode: slot.Last().CountryCode,
                    AreaCode: slot.Last().AreaCode,
                    Number: slot.Last().Number
                ));
    }

    private static string _ResolvePhoneSlot(PhoneType type)
    {
        return type switch
        {
            PhoneType.Mobile => "mobile_phone",
            PhoneType.Phone => "home_phone",
            _ => "mobile_phone"
        };
    }
}
