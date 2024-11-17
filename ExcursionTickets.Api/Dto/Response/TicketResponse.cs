namespace ExcursionTickets.Api.Dto.Response
{
    public record TicketResponse
    (
        string ExcursionName,
        decimal Price,
        string UserName,
        string UserSurname,
        DateTime StartTime,
        DateTime PaymentTime
    );
}
