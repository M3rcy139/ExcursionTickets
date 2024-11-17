using ExcursionTickets.Core.Models;
using ExcursionTickets.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using ExcursionTickets.Api.Dto.Request;
using ExcursionTickets.Api.Dto.Response;

namespace ExcursionTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository excursionRepository)
        {
            _paymentRepository = excursionRepository;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = paymentRequest.Name,
                    Surname = paymentRequest.Surname,
                    Email = paymentRequest.Email,
                };

                var payment = await _paymentRepository.ProcessPayment
                    (user, paymentRequest.AmountPaid, paymentRequest.PaymentMethod, paymentRequest.TicketQuantity,
                        paymentRequest.ExcursionId);

                var response = new PaymentResponse
                (
                    payment.Id,
                    payment.TicketQuantity,
                    payment.PaymentMethod,
                    payment.AmountPaid,
                    payment.Change,
                    payment.PaymentTime

                );

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("get-tickets/ticket")]
        public async Task<IActionResult> GetTickets(Guid paymentId)
        {
            try
            {
                var tickets = await _paymentRepository.GetTickets(paymentId);

                var response = tickets.Select(ticket => new TicketResponse
                (
                    ticket.Excursion.Name,
                    ticket.Excursion.Price,
                    ticket.User.Name,
                    ticket.User.Surname,
                    ticket.Excursion.StartTime,
                    ticket.Payment.PaymentTime
                )).ToList();

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
