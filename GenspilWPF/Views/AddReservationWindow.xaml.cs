using GenspilWPF.Models;
using System.Windows;


namespace GenspilWPF.Views
{
    public partial class AddReservationWindow : Window
    {
        // Privat felt til at holde den eksisterende valgte reservation til redigring. Null ved ny reservation.
        private Reservation _existingReservation;
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

            // Hvis eksisterende reservation eksisterer, udfyld felterne med existingReservations attributter:
            if (existingReservation != null)
            {
                TitleTextBox.Text = existingReservation.GameTitle;
                CustomerTextBox.Text = existingReservation.CustomerName;
                ContactInfoTextBox.Text = existingReservation.ContactInfo;
                NotesTextBox.Text = existingReservation.Notes;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Hent vaerdier fra inputfelterne:
            string gameTitle = TitleTextBox.Text;
            string customerName = CustomerTextBox.Text;
            string contactInfo = ContactInfoTextBox.Text;
            ReservationStatus status = ReservationStatus.Aktiv; // Nye reservationer er altid aktive (default).
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

        // Lukker vinduet uden at gemme. NewReservation forbliver null.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
