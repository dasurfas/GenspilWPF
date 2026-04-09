using GenspilWPF.Models;
using System.Windows;


namespace GenspilWPF.Views
{
    /// <summary>
    /// Interaction logic for AddReservationWindow.xaml
    /// </summary>
    public partial class AddReservationWindow : Window
    {
        internal Reservation NewReservation { get; private set; }
        public AddReservationWindow()
        {
            InitializeComponent();


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string gameTitle = TitleTextBox.Text;
            string customerName = CustomerTextBox.Text;
            string contactInfo = ContactInfoTextBox.Text;
            ReservationStatus status = ReservationStatus.Aktiv; // Default status for nye reservationer.
            string notes = NotesTextBox.Text;

            NewReservation = new Reservation(gameTitle, customerName, contactInfo, status, notes);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
