using GenspilWPF.Repositories;
using System.Windows;

namespace GenspilWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initialiser repositories og service, og saetter DataContext til MainViewModel:

            // gameRepo er en instans af BoardGameRepository (load / save spil til/fra "boardgames.txt").
            var gameRepo = new BoardGameRepository("boardgames.txt");
            // reservationRepo er en instans af ReservationRepository (load / save reservationer til/fra "reservations.txt").
            var reservationRepo = new ReservationRepository("reservations.txt");
            // service er en instans af GenspilService, som indeholder forretningslogikken og bruger
            // gameRepo og reservationRepo til at udfoere operationer.
            var service = new Services.GenspilService(gameRepo, reservationRepo);
            // DataContext er sat til en ny instans af MainViewModel, som tager service som parameter i constructoren.
            // Det betyder at MainViewModel har adgang til service og kan bruge den til at udfoere operationer, som UI'et binder til.
            // F.eks. kan MainViewModel bruge service til at hente listen af spil og reservationer og expose dem som ObservableCollection'er, som UI'et kan binde til for at vise data og reagere paa brugerinteraktioner.
            DataContext = new ViewModels.MainViewModels(service);
        }
    }
}