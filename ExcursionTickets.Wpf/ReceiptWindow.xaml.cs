using ExcursionTickets.Api.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для ReceiptWindow.xaml
    /// </summary>
    public partial class ReceiptWindow : Window
    {
        private PaymentResponse _paymentResponse;

        public ReceiptWindow(PaymentResponse paymentResponse)
        {
            InitializeComponent();
            _paymentResponse = paymentResponse;

            PaymentIdTextBlock.Text = $"ID Платежа: {_paymentResponse.Id}";
            TicketQuantityTextBlock.Text = $"ID Мест: {string.Join(", ", _paymentResponse.TicketQuantity)}";
            PaymentTypeTextBlock.Text = $"Тип оплаты: {_paymentResponse.PaymentMethod}";
            AmountTextBlock.Text = $"Сумма: {_paymentResponse.AmountPaid}₽";
            ChangeTextBlock.Text = _paymentResponse.Change.HasValue
                ? $"Сдача: {_paymentResponse.Change.Value}₽"
                : "Сдача: Нет";
            PaymentTimeTextBlock.Text = $"Время платежа: {_paymentResponse.PaymentTime.AddHours(3)}";
        }

        private void GetTicketButton_Click(object sender, RoutedEventArgs e)
        {
            var TicketWindow = new TicketWindow(_paymentResponse.Id);
            TicketWindow.ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
