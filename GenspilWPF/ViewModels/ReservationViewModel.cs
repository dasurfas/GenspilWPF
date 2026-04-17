using GenspilWPF.Models;
using GenspilWPF.Services;
using GenspilWPF.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GenspilWPF.ViewModels
{
    // ReservationViewModel har samme struktur og funktionalitet som BoardGameViewModel:
    internal class ReservationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Reservation> _reservations;
        private readonly GenspilService _service;
        private Reservation _selectedReservation;

        public ReservationViewModel(GenspilService service)
        {
            _service = service;
            _reservations = new ObservableCollection<Reservation>(_service.GetAllReservations());
            AddReservationCommand = new RelayCommand(AddReservation);
            DeleteReservationCommand = new RelayCommand(RemoveReservation);
            EditReservationCommand = new RelayCommand(UpdateReservation);
        }

        private void AddReservation()
        {
            // Aabn et nyt vindue for at tilfoeje en reservation. Dette vindue vil returnere den nye reservation, hvis brugeren vaelger at tilfoeje den:
            var window = new AddReservationWindow();
            window.ShowDialog();
            if (window.NewReservation != null)
            {
                _service.AddReservation(window.NewReservation);
                Reservations.Add(window.NewReservation);
            }
        }

        private void RemoveReservation()
        {
            if (SelectedReservation == null) return;                    // Intet spil valgt, intet at slette.

            var result = MessageBox.Show
                (
                "Er du sikker på at du vil slette denne reservation?",  // Tekst i dialogen.
                "Bekræft sletning",                                     // Titlen i boksen.
                MessageBoxButton.YesNo,                                 // Hvilke knapper der skal vises - Yes og No.
                MessageBoxImage.Warning                                 // Ikonet i boksen. Warning er gul trekant.
                );

            if (result == MessageBoxResult.Yes)                         // Hvis brugeren trykker "ja".
            {
                _service.RemoveReservation(SelectedReservation);        // Slet fra fil.
                Reservations.Remove(SelectedReservation);               // Opdater UI'et.
                SelectedReservation = null;                             // Nulstil selectedReservation (null).
            }
        }

        private void UpdateReservation()
        {
            if (_selectedReservation == null) return;

            var window = new AddReservationWindow(_selectedReservation);
            window.ShowDialog();

            if (window.NewReservation != null)
            {
                _service.UpdateReservation(window.NewReservation);
                int index = Reservations.IndexOf(_selectedReservation);
                Reservations[index] = window.NewReservation;
            }
        }

        // Reservations er listen DataGrid (xaml) binder (binding) til:
        public ObservableCollection<Reservation> Reservations
            {
                get { return _reservations; }
                set
                {
                    _reservations = value;
                    OnPropertyChanged(nameof(Reservations));
                }
        }
        // SelectedReservation er den reservation som brugeren har valgt i DataGrid'et i ReservationView.xaml.
        // Den bruges til at vide hvilken reservation der skal slettes eller redigeres, og til at sende data til AddReservationWindow.xaml ved redigering:
        public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }
        // Kommandoer til at tilfoeje, slette og redigere reservationer. Disse kommandoer bindes til knapper i ReservationView.xaml:
        public ICommand AddReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand EditReservationCommand { get; }

        // INotifyPropertyChanged interface implementering for at notify UI'et om aendringer i properties:
        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged. Denne metode kaldes for at notify UI'et om aendringer i properties, saa det kan opdatere det viste data i ReservationView.xaml:
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}