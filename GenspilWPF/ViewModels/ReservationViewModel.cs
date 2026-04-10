using GenspilWPF.Models;
using GenspilWPF.Views;
using GenspilWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Net.Http.Headers;

namespace GenspilWPF.ViewModels
{
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
            if (SelectedReservation != null)
            {
                _service.RemoveReservation(SelectedReservation);
                Reservations.Remove(SelectedReservation);
                SelectedReservation = null;
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

        public ObservableCollection<Reservation> Reservations
            {
                get { return _reservations; }
                set
                {
                    _reservations = value;
                    OnPropertyChanged(nameof(Reservations));
                }
        }
        public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }
        public ICommand AddReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand EditReservationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}