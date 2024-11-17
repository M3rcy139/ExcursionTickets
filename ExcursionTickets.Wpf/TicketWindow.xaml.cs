using ExcursionTickets.Api.Dto.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExcursionTickets.Wpf
{
    /// <summary>
    /// Логика взаимодействия для TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        private HttpClient _httpClient;
        private Guid _paymentId;

        public TicketWindow(Guid paymentId)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _paymentId = paymentId;

            LoadTicket();
        }

        private async void LoadTicket()
        {
            try
            {
                TicketsStackPanel.Children.Clear(); 

                var tickets = await GetTickets(_paymentId);

                EmailTextBlock.Text = tickets.Count > 1 ? "Билеты были отправлены вам на почту" : "Билет был отправлен вам на почту";

                foreach (var ticket in tickets)
                {
                    var ticketInfo = new TextBlock
                    {
                        Text = $"Название экскурсии: {ticket.ExcursionName}\nВремя начала: {ticket.StartTime}\n" +
                               $"Стоимость: {ticket.Price}\nВладелец билета: {ticket.UserName} {ticket.UserSurname}\n" +
                               $"Время покупки: {ticket.PaymentTime}\n\n",
                        FontSize = 14,
                        LineHeight = 20,
                        Margin = new Thickness(10, 0, 0, 5)
                    };
                    TicketsStackPanel.Children.Add(ticketInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки билетов: {ex.Message}");
            }
        }

        private async Task<List<TicketResponse>> GetTickets(Guid paymentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7219/api/Payment/get-tickets/ticket?paymentId={paymentId}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TicketResponse>>(content);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
