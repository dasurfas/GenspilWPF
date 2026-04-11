using GenspilWPF.Models;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows;

namespace GenspilWPF.Views
{
    public partial class AddBoardGameWindow : Window
    {
        private BoardGame _existingGame;
        // Tilfoejer input til constructoren saa vi kan tilfoeje (null hvis der ikke er et existing game)
        // og redigere (existing game set fra BoardGameViewModel):
        // Constructoren er internal fordi BoardGame klassen er internal. Begge skal have samme access modifiers.
        internal AddBoardGameWindow(BoardGame existingGame = null)
        {
            InitializeComponent();

            _existingGame = existingGame;
            ConditionComboBox.ItemsSource = Enum.GetValues(typeof(GameCondition));

            // Hvis et existing game eksisterer:
            if (existingGame != null)
            {
                TitleTextBox.Text = existingGame.Title;
                GenreTextBox.Text = existingGame.Genre;
                PlayersTextBox.Text = existingGame.PlayerRange;
                ConditionComboBox.Text = existingGame.GameCondition.ToString();
                PriceTextBox.Text = existingGame.Price.ToString();
                NotesTextBox.Text = existingGame.Notes;

                AddButton.Content = "Gem ændringer"; // Aendrer teksten fra "tilfoej spil" naar man redigerer.
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            string title = TitleTextBox.Text;
            string genre = GenreTextBox.Text;
            string [] parts = PlayersTextBox.Text.Split('-');
            int minPlayers = int.Parse(parts[0]);
            int maxPlayers = parts.Length > 1 ? int.Parse(parts[1]) : minPlayers;
            GameCondition gameCondition = (GameCondition)ConditionComboBox.SelectedItem;
            GameStatus gameStatus = GameStatus.På_Lager;
            decimal price = decimal.TryParse(PriceTextBox.Text, out decimal p) ? p : 0;
            string notes = NotesTextBox.Text;

            // Opret et nyt BoardGame-objekt med de indtastede oplysninger
            // Bestemmer hvilken constructor (BoardGame.cs) vi skal bruge:
            if (_existingGame != null)
            {
                // Redigering - Bevar det originale ID:
                NewBoardGame = new BoardGame(_existingGame.Id, title, genre, minPlayers, maxPlayers, price, gameCondition, gameStatus, notes);
            }
            else
            {
                // Nyt spil - Opret nyt ID:
                NewBoardGame = new BoardGame(title, genre, minPlayers, maxPlayers, price, gameCondition, gameStatus, notes);
            }

            this.Close();

        }
        internal BoardGame NewBoardGame { get; private set; }
    }
}