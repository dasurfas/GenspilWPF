using GenspilWPF.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace GenspilWPF.ViewModels
{
    internal class MainViewModels : INotifyPropertyChanged
    {
        private object _currentView;
        private readonly BoardGameViewModel _boardGameViewModel;
        private readonly ReservationViewModel _reservationViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public MainViewModels(GenspilService service)
        {
            _boardGameViewModel = new BoardGameViewModel(service);
            _reservationViewModel = new ReservationViewModel(service);
            _currentView = _boardGameViewModel;

            ShowBoardGamesCommand = new RelayCommand(() => CurrentView = _boardGameViewModel); // Lamdba pga. Intellisense, men ellers det samme.
            ShowReservationCommand = new RelayCommand(() => CurrentView = _reservationViewModel);
        }

        public ICommand ShowBoardGamesCommand { get; }
        public ICommand ShowReservationCommand { get; }
    }
}