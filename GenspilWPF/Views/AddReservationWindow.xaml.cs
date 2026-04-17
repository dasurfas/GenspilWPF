using GenspilWPF.Models;
using System;
using System.Windows;

namespace GenspilWPF.Views
{
    public partial class AddReservationWindow : Window
    {
        // Privat felt til at holde den eksisterende valgte reservation til redigring. Null ved ny reservation.
        private readonly Reservation _existingReservation;
        // Den oprettede eller redigerede reservation. Laeses af ReservationViewModel efter vinduet lukkes.
        internal Reservation NewReservation { get; private set; }

        // Reservations klassen er internal, derfor er constructoren internal (adgangsmodfiers).
        // existingReservation = null betyder at parameteret er valgfrit, dvs:
        // Ingen parameter (null): Ny reservation.
        // Parameter (!null): Redigering af eksisterende reservation.
        internal AddReservationWindow(Reservation existingReservation = null)
        {
            InitializeComponent();

            _existingReservation = existingReservation;
            // Initialiser StatusComboBox med enum vaerdier og saet default til "Aktiv":
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(ReservationStatus));
            StatusComboBox.SelectedItem = ReservationStatus.Aktiv;

            // Hvis eksisterende reservation eksisterer, udfyld felterne med existingReservations attributter:
            if (existingReservation != null)
            {
                TitleTextBox.Text = existingReservation.GameTitle;
                CustomerTextBox.Text = existingReservation.CustomerName;
                ContactInfoTextBox.Text = existingReservation.ContactInfo;
                NotesTextBox.Text = existingReservation.Notes;
                StatusComboBox.SelectedItem = existingReservation.Status;

                // Samme som rediger BoardGame. Aendrer teksten i knappen naar man redigerer.
                AddButton.Content = "Gem ændringer";
                // Skifter titlen paa modal fra "Tilfoej reservation" til "Rediger reservation".
                Title = "Rediger reservation";
                // Og overskriften i modalen:
                HeaderTextBlock.Text = "Rediger reservation";
            }

        }

        // Henter vaerdier fra inputfelterne og opretter en ny reservation. Hvis det er en redigering, bevares det originale ID:
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hent vaerdier fra inputfelterne:
                string gameTitle = TitleTextBox.Text;
                string customerName = CustomerTextBox.Text;
                string contactInfo = ContactInfoTextBox.Text;
                // Status af ComboBox sat til enum vaerdien valgt i ComboBox (default: Aktiv):
                ReservationStatus status = (ReservationStatus)StatusComboBox.SelectedItem;
                string notes = NotesTextBox.Text;

                // Bestemmer hvilken Constructor (Reservation.cs) vi skal bruge:
                if (_existingReservation != null)
                {
                    // Redigering - Bevar det originale ID (Constructor 2):
                    NewReservation = new Reservation(_existingReservation.Id, gameTitle, customerName, contactInfo, _existingReservation.Date, status, notes);
                }
                else
                {
                    // Ny reservation - Opret nyt ID (Constructor 1):
                    NewReservation = new Reservation(gameTitle, customerName, contactInfo, status, notes);
                }
                // Luk modal:
                this.Close();
            }
            catch (ArgumentException ex)
            {
                // Vis MessageBox popup fejlbesked:
                MessageBox.Show(ex.Message, "Fejl i input", MessageBoxButton.OK, MessageBoxImage.Error);
                // Luk ikke vinduet, lad brugeren rette. Return returnerer tidligt fra metoden, saa at vinduet forbliver aabent:
                return;
            }
        }

        // Lukker vinduet uden at gemme. NewReservation forbliver null.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
