using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using ExcursionTickets.Core.Models;


namespace ExcursionTickets.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _httpClient;
        private Excursion selectedExcursion;
        private List<Excursion> excursionsInfo;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadExcursions();
            LoadAdvertisement();
        }

        private async Task LoadExcursions()
        {
            try
            {
                var excursions = await GetAllExcursions();
                ExcursionsComboBox.ItemsSource = excursions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки экскурсий: {ex.Message}");
            }
        }

        private async Task LoadAdvertisement()
        {
            try
            {
                var advertisements = await GetAdvertisements();

                Random random = new Random();
                int randomNumber = random.Next(0, 4);

                var advertisement = advertisements[randomNumber];

                AdvertisementTextBlock.Text =  $"{advertisement.AdText}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рекламы: {ex.Message}");
            }
        }

        private async void ExcursionsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ExcursionsComboBox.SelectedValue != null)
            {
                selectedExcursion = (Excursion)ExcursionsComboBox.SelectedItem;
                await LoadExcursionDetails(selectedExcursion.Id);

                NameTextBlock.Text = "Название экскурсии:";
                DescriptionTextBlock.Text = "Описание:";
                StartTimeTextBlock.Text = "Время начала:";
                AvailableTicketsTextBlock.Text = "Кол-во билетов в наличии:";
                PriceTextBlock.Text = "Стоимость билета:";

                NameTextBlock.Text += $" {selectedExcursion.Name}";
                DescriptionTextBlock.Text += $" {selectedExcursion.Description}";
                StartTimeTextBlock.Text += $" {selectedExcursion.StartTime.ToString()}";
                AvailableTicketsTextBlock.Text += $" {selectedExcursion.AvailableTickets.ToString()}";
                PriceTextBlock.Text += $" {selectedExcursion.Price.ToString()} рублей";
            }
        }
        private async Task LoadExcursionDetails(int flightId)
        {
            try
            {
                selectedExcursion = await GetExcursionDetails(flightId);
                BuyingTicketsButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки информации о экскурсии: {ex.Message}");
            }
        }

        private void BuyingTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExcursionsComboBox.SelectedValue != null)
            {
                int excursionId = (int)ExcursionsComboBox.SelectedValue;
                decimal price = selectedExcursion.Price;

                var paymentWindow = new PaymentWindow(excursionId, price);
                paymentWindow.Show();
                this.Close();
            }
            else MessageBox.Show("Выберите экскурсию!");
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadExcursions();
                await LoadAdvertisement();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}");
            }
        }

        private async Task<List<Excursion>> GetAllExcursions()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7219/api/Excursion/get-all-excursions");
            if (response.IsSuccessStatusCode)
            {

                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Excursion>>(content);
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorContent);

                throw new Exception(errorResponse.ContainsKey("error") ? errorResponse["error"] : "Unknown error occurred");
            }
        }

        private async Task<Excursion> GetExcursionDetails(int excursionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7219/api/Excursion/get-excursion-details/{excursionId}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Excursion>(content);
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorContent);

                throw new Exception(errorResponse.ContainsKey("error") ? errorResponse["error"] : "Unknown error occurred");
            }
        }

        private async Task<List<Advertisement>> GetAdvertisements()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7219/api/Advertisement/get-advertisement/advertisement");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Advertisement>>(content);
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorContent);

                throw new Exception(errorResponse.ContainsKey("error") ? errorResponse["error"] : "Unknown error occurred");
            }
        }
    }
}
