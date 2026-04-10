using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace GenspilWPF.Models
{
    internal class BoardGame
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Genre { get; private set; }
        public int MinPlayerCount { get; private set; }
        public int MaxPlayerCount { get; private set; }
        // PlayerRange er en string der goer at man ikke skal indtaste baade min og max players naar man opretter et spil:
        public string PlayerRange
        {
            get
            {
                if (MinPlayerCount == MaxPlayerCount) return $"{MinPlayerCount}";
                else return $"{MinPlayerCount}-{MaxPlayerCount}";
            }
        }
        public decimal Price { get; private set; }
        public string Notes { get; private set; }
        public GameCondition GameCondition { get; private set; }
        public GameStatus GameStatus { get; private set; }
    
        // Constructor 1 (genererer et nyt ID)
    public BoardGame(string title, string genre, int minPlayerCount, int maxPlayerCount, decimal price, GameCondition gameCondition, GameStatus gameStatus, string notes)
        {
            Title = title;
            Genre = genre;
            MinPlayerCount = minPlayerCount;
            MaxPlayerCount = maxPlayerCount;
            Price = price;
            GameCondition = gameCondition;
            GameStatus = gameStatus;
            Notes = notes;

            Id = Math.Abs(Guid.NewGuid().GetHashCode());

            Validate();
        }

        // Constructor 2 - Overload (bruges ved indlaesning fra fil, hvor ID allerede er kendt)
        public BoardGame(int id, string title, string genre, int minPlayerCount, int maxPlayerCount, decimal price, GameCondition gameCondition, GameStatus gameStatus, string notes)
        {
            Id = id;
            Title = title;
            Genre = genre;
            MinPlayerCount = minPlayerCount;
            MaxPlayerCount = maxPlayerCount;
            Price = price;
            GameCondition = gameCondition;
            GameStatus = gameStatus;
            Notes = notes;

            Validate();
        }

        // Validering af inputdata:
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new ArgumentException("Titel maa ikke vaere tom!");
            if (string.IsNullOrWhiteSpace(Genre))
                throw new ArgumentException("Genre maa ikke vaere tom!");
            if (MinPlayerCount < 1)
                throw new ArgumentException("Minimum antal spillere skal vaere mindst 1!");
            if (MaxPlayerCount < 1)
                throw new ArgumentException("Maximum antal spillere skal vaere mindst 1!");
            if (Price < 0)
                throw new ArgumentException("Prisen maa ikke vaere negativ!");
            if (MinPlayerCount > MaxPlayerCount)
                throw new ArgumentException("Minimum antal spillere kan ikke vaere stoerre end maximum antal spillere!");
        }

        // Metode til at saette pris og validere:
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Prisen maa ikke vaere negativ!");
            Price = newPrice;
        }
        public void UpdateCondition(GameCondition newCondition)
        {
            GameCondition = newCondition;
        }

        public void UpdateStatus(GameStatus newStatus)
        {
            GameStatus = newStatus;
        }

        public void UpdateNotes(string newNotes)
        {
            Notes = newNotes;
        }

        public override string ToString()
        {
            return $"{Id};{Title};{Genre};{MinPlayerCount};{MaxPlayerCount};{Price};{GameCondition};{GameStatus};{Notes}";
        }

        // FromString() metoden der splitter vores string up i parts og samler dem i et array:
        public static BoardGame FromString(string data)
        {
            string[] parts = data.Split(';');

            return new BoardGame(
                int.Parse(parts[0]), // Id
                parts[1], // Title
                parts[2], // Genre
                int.Parse(parts[3]), // MinPlayerCount
                int.Parse(parts[4]), // MaxPlayerCount
                // Pris - brug InvariantCulture for at sikre korrekt decimalformat uanset systemindstillinger
                decimal.Parse(parts[5]),
                (GameCondition)Enum.Parse(typeof(GameCondition), parts[6]), // GameCondition
                (GameStatus)Enum.Parse(typeof(GameStatus), parts[7]), // GameStatus
                parts[8] // Notes
                );
        }
    }
}