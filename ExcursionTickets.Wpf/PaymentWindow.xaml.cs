using ExcursionTickets.Api.Dto.Request;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ExcursionTickets.Wpf
{
    /// <summary>
    /// Логика взаимодействия для PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        private int _excursionId;
        private decimal _price;
        private int _ticketQuantity;
        private decimal _amountPaid;

        public PaymentWindow(int excursionId, decimal price)
        {
            InitializeComponent();
            _excursionId = excursionId;
            _price = price;

            PriceTextBlock.Text = $"Стоимость одного билета: {_price}₽";
        }

        public async void PaymentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentTypeComboBox.SelectedValue != null)
            {
                string paymentType = (PaymentTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (paymentType == "Карта")
                {
                    AmountPaidTextBox.Text = _amountPaid.ToString();
                    AmountPaidTextBox.IsEnabled = false;
                }
                else
                {
                    AmountPaidTextBox.Text = "";
                    AmountPaidTextBox.IsEnabled = true;
                }
            }
        }

        public async void TicketQuantity_Changed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(!int.TryParse(TicketQuantityTextBox.Text, out _ticketQuantity))
            {
                MessageBox.Show("Введите целое число.");
                return;
            }

            await SetAmountPaid(_price, _ticketQuantity);
        }

        private async void PayButton_Click(object sender, RoutedEventArgs e)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string email = EmailTextBox.Text;
            string paymentMethod = (PaymentTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Пожалуйста, введите корректный email.");
                return;
            }

            if (!decimal.TryParse(AmountPaidTextBox.Text, out decimal amountPaid))
            {
                MessageBox.Show("Пожалуйста введите сумму");
                return;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(paymentMethod))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (amountPaid < _price)
            {
                MessageBox.Show("Недостаточно средств");
                return;
            }

            var paymentRequest = new PaymentRequest
            (
                name,
                surname,
                email,
                paymentMethod,
                amountPaid,
                _ticketQuantity,
                _excursionId
            );

            try
            {
                var CardWindow = new CardWindow(paymentRequest, amountPaid);
                CardWindow.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оплате: {ex.Message}");
            }
        }

        private async Task SetAmountPaid(decimal price, int ticketQuantity)
        {
            _amountPaid = price * ticketQuantity;

            AmountPaidTextBlock.Text = $"К оплате: {_amountPaid}₽";
        }
    }
}
