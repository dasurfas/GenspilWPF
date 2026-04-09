using GenspilWPF.Repositories;
using System.Windows;

namespace GenspilWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var gameRepo = new BoardGameRepository("boardgames.txt");
            var reservationRepo = new ReservationRepository("reservations.txt");
            var service = new Services.GenspilService(gameRepo, reservationRepo);
            DataContext = new ViewModels.MainViewModels(service);
        }
    }
}