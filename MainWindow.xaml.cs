// MainWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusStation
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<BusRoute> _busRoutes;
        private BusRouteSorter _sorter;
        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            _busRoutes = new ObservableCollection<BusRoute>();
            BusRoutesListView.ItemsSource = _busRoutes;
            _sorter = new BusRouteSorter();
        }

        // Сортировка по номеру автобуса (возрастание)
        private void SortByBusNumberAsc(object sender, RoutedEventArgs e) => _sorter.SortByBusNumber(_busRoutes, true);

        // Сортировка по номеру автобуса (убывание)
        private void SortByBusNumberDesc(object sender, RoutedEventArgs e) => _sorter.SortByBusNumber(_busRoutes, false);

        // Сортировка по пункту отправления (возрастание)
        private void SortByDeparturePointAsc(object sender, RoutedEventArgs e) => _sorter.SortByDeparturePoint(_busRoutes, true);

        // Сортировка по пункту отправления (убывание)
        private void SortByDeparturePointDesc(object sender, RoutedEventArgs e) => _sorter.SortByDeparturePoint(_busRoutes, false);

        // Сортировка по пункту назначения (возрастание)
        private void SortByDestinationPointAsc(object sender, RoutedEventArgs e) => _sorter.SortByDestinationPoint(_busRoutes, true);

        // Сортировка по пункту назначения (убывание)
        private void SortByDestinationPointDesc(object sender, RoutedEventArgs e) => _sorter.SortByDestinationPoint(_busRoutes, false);

        // Сортировка по времени отправления (возрастание)
        private void SortByDepartureTimeAsc(object sender, RoutedEventArgs e) => _sorter.SortByDepartureTime(_busRoutes, true);

        // Сортировка по времени отправления (убывание)
        private void SortByDepartureTimeDesc(object sender, RoutedEventArgs e) => _sorter.SortByDepartureTime(_busRoutes, false);

        // Сортировка по времени прибытия (возрастание)
        private void SortByArrivalTimeAsc(object sender, RoutedEventArgs e) => _sorter.SortByArrivalTime(_busRoutes, true);

        // Сортировка по времени прибытия (убывание)
        private void SortByArrivalTimeDesc(object sender, RoutedEventArgs e) => _sorter.SortByArrivalTime(_busRoutes, false);

        // Сортировка по цене билета (возрастание)
        private void SortByTicketPriceAsc(object sender, RoutedEventArgs e) => _sorter.SortByTicketPrice(_busRoutes, true);

        // Сортировка по цене билета (убывание)
        private void SortByTicketPriceDesc(object sender, RoutedEventArgs e) => _sorter.SortByTicketPrice(_busRoutes, false);


        private void AddRouteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текст из каждого поля и удаляем пробелы в начале и в конце
                var busNumber = BusNumberTextBox.Text.Trim();
                var departurePoint = DeparturePointTextBox.Text.Trim();
                var destinationPoint = DestinationPointTextBox.Text.Trim();
                var departureTimeText = DepartureTimeTextBox.Text.Trim();
                var arrivalTimeText = ArrivalTimeTextBox.Text.Trim();
                var ticketPriceText = TicketPriceTextBox.Text.Trim();

                // Проверка на пустые поля
                if (string.IsNullOrWhiteSpace(busNumber) ||
                    string.IsNullOrWhiteSpace(departurePoint) ||
                    string.IsNullOrWhiteSpace(destinationPoint) ||
                    string.IsNullOrWhiteSpace(departureTimeText) ||
                    string.IsNullOrWhiteSpace(arrivalTimeText) ||
                    string.IsNullOrWhiteSpace(ticketPriceText))
                {
                    MessageBox.Show("Всі поля повинні бути заповнені.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Парсинг DateTime из текстовых полей
                if (!DateTime.TryParseExact(departureTimeText, "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureTime))
                {
                    MessageBox.Show("Невірно вказано час відправлення. Використовуйте формат 'HH:mm dd.MM.yyyy'.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!DateTime.TryParseExact(arrivalTimeText, "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var arrivalTime))
                {
                    MessageBox.Show("Невірно вказано час призначення. Використовуйте формат 'HH:mm dd.MM.yyyy'.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка на "минусовые" даты
                if (departureTime < DateTime.Now || arrivalTime < DateTime.Now)
                {
                    MessageBox.Show("Дата і час не можуть бути в минулому.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка, что дата отправления не позже даты прибытия
                if (departureTime >= arrivalTime)
                {
                    MessageBox.Show("Час відправлення не може бути пізніше або рівним часу призначення.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Парсинг и проверка цены билета
                if (!decimal.TryParse(ticketPriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var ticketPrice) || ticketPrice <= 0)
                {
                    MessageBox.Show("Ціна квитка повинна бути більше нуля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создание нового рейса
                var newRoute = new BusRoute
                {
                    BusNumber = busNumber,
                    DeparturePoint = departurePoint,
                    DestinationPoint = destinationPoint,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    TicketPrice = ticketPrice
                };

                // Добавление рейса в коллекцию
                _busRoutes.Add(newRoute);
                ClearInputFields();

                // Воспроизведение звука
                _mediaPlayer.Open(new Uri("success.mp3", UriKind.Relative));
                _mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка під час додавання рейсу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BusRoutesListView.SelectedItem is BusRoute selectedRoute)
            {
                _busRoutes.Remove(selectedRoute);
            }
            else
            {
                MessageBox.Show("Виберіть рейс для видалення.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields()
        {
            BusNumberTextBox.Clear();
            DeparturePointTextBox.Clear();
            DestinationPointTextBox.Clear();
            DepartureTimeTextBox.Clear();
            ArrivalTimeTextBox.Clear();
            TicketPriceTextBox.Clear();
        }
    }
}