using ExcursionTickets.Api.Dto.Request;
using ExcursionTickets.Api.Dto.Response;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ExcursionTickets.Wpf
{
    /// <summary>
    /// Логика взаимодействия для CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Window
    {
        private PaymentRequest _paymentRequest;
        private decimal _amountPaid;
        public CardWindow(PaymentRequest paymentRequest, decimal amountPaid)
        {
            InitializeComponent();
            _paymentRequest = paymentRequest;
            _amountPaid = amountPaid;

            AmountCardTextBlock.Text = $"К оплате: {_amountPaid}₽";
        }

        private async void PayButton_Click(object sender, RoutedEventArgs e)
        {
            var cardNumber = CardNumberTextBox.Text;
            var cardDate = CardDateTextBox.Text;
            var cardCVC = CardCVCTextBox.Text;

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cardDate) || string.IsNullOrEmpty(cardCVC))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!Regex.IsMatch(cardNumber, @"^(\d{4} ){3}\d{4}$"))
            {
                MessageBox.Show("Введите корректный номер карты (16 цифр).");
                return;
            }

            if (!Regex.IsMatch(cardDate, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                MessageBox.Show("Введите корректную дату (MM/YY).");
                return;
            }

            if (!Regex.IsMatch(cardCVC, @"^\d{3}$"))
            {
                MessageBox.Show("Введите корректный CVC (3 цифры).");
                return;
            }

            try
            {
                var result = await ProcessPayment(_paymentRequest);
                if (result != null)
                {
                    var receiptWindow = new ReceiptWindow(result);
                    receiptWindow.Show();
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оплате: {ex.Message}");
            }
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string text = textBox.Text.Replace(" ", "");
            if (text.Length > 16) text = text.Substring(0, 16);

            var formattedText = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0) formattedText += " ";
                formattedText += text[i];
            }

            textBox.Text = formattedText;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private async Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            using (var client = new HttpClient())
            {
                var url = "https://localhost:7219/api/Payment/pay";

                var content = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");
                try
                {
                    var response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResult = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Ошибка при выполнении запроса: {errorResult}");
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PaymentResponse>(result);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при выполнении запроса: {ex.Message}");
                }
            }
        }
    }
}
