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

        public Reservation(string gameTitle, string customerName, string contactInfo, ReservationStatus status)
        {
            GameTitle = gameTitle;
            CustomerName = customerName;
            ContactInfo = contactInfo;
            Date = DateTime.Today;
            Status = status;

            Validate();
        }
        public Reservation(string gameTitle, string customerName, string contactInfo, DateTime date, ReservationStatus status)
        {
            GameTitle = gameTitle;
            CustomerName = customerName;
            ContactInfo = contactInfo;
            Date = date;
            Status = status;

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
            return $"{GameTitle};{CustomerName};{ContactInfo};{Date};{Status}";
        }

        public static Reservation FromString(string data)
        {
            string [] parts = data.Split(';');

            return new Reservation(
                parts[0], // gameTitle
                parts[1], // customerName
                parts[2], // contactInfo
                DateTime.Parse(parts[3]), // date
                (ReservationStatus)Enum.Parse(typeof(ReservationStatus), parts[4]) // status
            );
        }

    }
}