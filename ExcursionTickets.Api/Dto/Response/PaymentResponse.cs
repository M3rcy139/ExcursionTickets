namespace ExcursionTickets.Api.Dto.Response
{
    public record PaymentResponse
    (
        Guid Id,
        int TicketQuantity,
        string PaymentMethod,
        decimal AmountPaid,
        decimal? Change,
        DateTime PaymentTime
    );
}
