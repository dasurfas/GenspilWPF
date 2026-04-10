using System;
using System.Runtime.CompilerServices;

namespace GenspilWPF.Models
{
    internal class Reservation
    {
        public string GameTitle { get; private set; }
        public string CustomerName { get; private set; }
        public string ContactInfo { get; private set; }
        public DateTime Date { get; private set; }
        public ReservationStatus Status { get; private set; }
        public string Notes { get; private set; }
        public int Id { get; private set; }

        // Constructor 1:
        public Reservation(string gameTitle, string customerName, string contactInfo, ReservationStatus status, string notes)
        {
            GameTitle = gameTitle;
            CustomerName = customerName;
            ContactInfo = contactInfo;
            Date = DateTime.Today;
            Notes = notes;
            Status = status;

            Id = Math.Abs(Guid.NewGuid().GetHashCode());

            Validate();
        }

        // Constructor 2:
        public Reservation(int id, string gameTitle, string customerName, string contactInfo, DateTime date, ReservationStatus status, string notes)
        {
            GameTitle = gameTitle;
            CustomerName = customerName;
            ContactInfo = contactInfo;
            Date = date;
            Notes = notes;
            Status = status;
            Id = id;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(GameTitle))
                throw new ArgumentException("Spil titel maa ikke vaere tom!");
            if (string.IsNullOrWhiteSpace(CustomerName))
                throw new ArgumentException("Kundenavn maa ikke vaere tom!");
            if (string.IsNullOrWhiteSpace(ContactInfo))
                throw new ArgumentException("Kontaktinformation maa ikke vaere tom!");
        }

        public void UpdateStatus(ReservationStatus newStatus)
        {
            Status = newStatus;
        }
        public override string ToString()
        {
            return $"{Id};{GameTitle};{CustomerName};{ContactInfo};{Date};{Status};{Notes}";
        }

        public static Reservation FromString(string data)
        {
            string [] parts = data.Split(';');

            return new Reservation(
                int.Parse(parts[0]), // Id fra int til string (Parse()).
                parts[1], // gameTitle
                parts[2], // customerName
                parts[3], // contactInfo
                DateTime.Parse(parts[4]), // date
                (ReservationStatus)Enum.Parse(typeof(ReservationStatus), parts[5]), // status
                parts[6] // notes
            );
        }
    }
}