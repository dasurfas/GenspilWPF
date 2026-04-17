using GenspilWPF.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace GenspilWPF.ViewModels
{
    internal class MainViewModels : INotifyPropertyChanged
    {
        // Dette er en "MainViewModel" som holder styr på hvilket view der skal vises i MainWindow.xaml:
        private object _currentView;
        // BoardGameViewModel initialiseres i constructoren og bruges til at vise BoardGameView i MainWindow.xaml:
        private readonly BoardGameViewModel _boardGameViewModel;
        // ReservationViewModel initialiseres i constructoren og bruges til at vise ReservationView i MainWindow.xaml:
        private readonly ReservationViewModel _reservationViewModel;

        // INotifyPropertyChanged interface implementering for at notify UI'et om aendringer i CurrentView property:
        public event PropertyChangedEventHandler PropertyChanged;

        // Hjaelpeproperties til at checke hvilket view der er aktivt, som UI'et kan binde til for at opdatere knapstilarter i MainWindow.xaml:
        public bool IsLagerActive => CurrentView == _boardGameViewModel;
        public bool IsReservationActive => CurrentView == _reservationViewModel;

        // Constructor. Her initialiseres BoardGameViewModel og ReservationViewModel,
        // og CurrentView saettes til BoardGameViewModel som default view:
        public MainViewModels(GenspilService service)
        {
            _boardGameViewModel = new BoardGameViewModel(service);
            _reservationViewModel = new ReservationViewModel(service);
            _currentView = _boardGameViewModel;

            // Kommandoer til at skifte mellem BoardGameView og ReservationView i MainWindow.xaml:
            ShowBoardGamesCommand = new RelayCommand(() => CurrentView = _boardGameViewModel);
            ShowReservationCommand = new RelayCommand(() => CurrentView = _reservationViewModel);
        }

        // OnPropertyChanged. Denne metode kaldes for at notify UI'et om aendringer i CurrentView property,
        // saa det kan opdatere det viste view i MainWindow.xaml:
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // CurrentView property. Dette er den property som MainWindow.xaml binder til for at vise det aktuelle view:
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
                // Notify UI'et om aendringer i IsLagerActive og IsReservationActive, saa knapstilarterne i MainWindow.xaml kan opdateres:
                OnPropertyChanged(nameof(IsLagerActive));
                OnPropertyChanged(nameof(IsReservationActive));
            }
        }

        // Kommandoer til at skifte mellem BoardGameView og ReservationView i MainWindow.xaml:
        public ICommand ShowBoardGamesCommand { get; }
        public ICommand ShowReservationCommand { get; }
    }
}