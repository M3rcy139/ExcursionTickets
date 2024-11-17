namespace ExcursionTickets.Api.Dto.Request
{
    public record PaymentRequest
    (
        string Name,
        string Surname,
        string Email,
        string PaymentMethod,
        decimal AmountPaid,
        int TicketQuantity,
        int ExcursionId
    );
}
