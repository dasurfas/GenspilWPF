using System.Collections.Generic;
using GenspilWPF.Models;
using System.IO;

namespace GenspilWPF.Repositories
{
    internal class ReservationRepository : IRepository<Reservation>
    {
        private string _filePath;

        public ReservationRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Reservation> LoadAll()
        {
            List<Reservation> reservations = new List<Reservation>();
            if (!File.Exists(_filePath)) return reservations; // Returner en tom liste, hvis filen ikke findes

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        reservations.Add(Reservation.FromString(line));
                    }
                    catch
                    {
                        // Log fejl eller ignorer ugyldige linjer
                    }
                }
            }
            return reservations; // Returner den indlaeste liste af spil
        }

        public void SaveAll(List<Reservation> items)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false)) // false for at overskrive filen
            {
                foreach (var reservation in items)
                {
                    writer.WriteLine(reservation.ToString());
                }
            }
        }
    }
}
