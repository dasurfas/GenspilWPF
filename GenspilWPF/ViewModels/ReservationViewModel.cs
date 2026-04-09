using GenspilWPF.Models;
using GenspilWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace GenspilWPF.ViewModels
{
    internal class ReservationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Reservation> _reservations;
        private readonly GenspilService _service;
        private Reservation _selectedReservation;
        private string _newGameTitle;
        private string _newCustomerName;
        private string _newContactInfo;

        public ReservationViewModel(GenspilService service)
        {
            _service = service;
            _reservations = new ObservableCollection<Reservation>(_service.GetAllReservations());
            AddReservationCommand = new RelayCommand(AddReservation);
            DeleteReservationCommand = new RelayCommand(RemoveReservation);
        }

        private void AddReservation()
        {
            var newReservation = new Reservation(NewGameTitle, NewCustomerName, NewContactInfo, ReservationStatus.Aktiv);
            _service.AddReservation(newReservation);
            Reservations.Add(newReservation);
            ClearNewReservationFields();
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

        private void ClearNewReservationFields()
        {
            NewGameTitle = string.Empty;
            NewCustomerName = string.Empty;
            NewContactInfo = string.Empty;
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

        public string NewGameTitle
        { 
            get { return _newGameTitle; }
            set
            {
                _newGameTitle = value;
                OnPropertyChanged(nameof(NewGameTitle));
            }
        }

        public string NewCustomerName
        {
            get { return _newCustomerName; }
            set
            {
                _newCustomerName = value;
                OnPropertyChanged(nameof(NewCustomerName));
            }
        }

        public string NewContactInfo
        {
            get { return _newContactInfo; }
            set
            {
                _newContactInfo = value;
                OnPropertyChanged(nameof(NewContactInfo));
            }
        }
        public ICommand AddReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}