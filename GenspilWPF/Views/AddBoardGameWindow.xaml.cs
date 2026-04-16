using GenspilWPF.Models;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GenspilWPF.Views
{
    public partial class AddBoardGameWindow : Window
    {
        private readonly BoardGame _existingGame;
        // Tilfoejer input til constructoren saa vi kan tilfoeje (null hvis der ikke er et existing game)
        // og redigere (existing game set fra BoardGameViewModel):
        // Constructoren er internal fordi BoardGame klassen er internal. Begge skal have samme access modifiers.
        internal AddBoardGameWindow(BoardGame existingGame = null)
        {
            InitializeComponent();

            _existingGame = existingGame;
            ConditionComboBox.ItemsSource = Enum.GetValues(typeof(GameCondition));
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(GameStatus));
            // Saet ComboBox default value til "Paa lager":
            StatusComboBox.SelectedItem = GameStatus.På_Lager;
            // Hvis et existing game eksisterer saa hent info:
            if (existingGame != null)
            {
                TitleTextBox.Text = existingGame.Title;
                GenreTextBox.Text = existingGame.Genre;
                PlayersTextBox.Text = existingGame.PlayerRange;
                PriceTextBox.Text = existingGame.Price.ToString();
                NotesTextBox.Text = existingGame.Notes;
                ConditionComboBox.SelectedItem = existingGame.GameCondition;
                StatusComboBox.SelectedItem = existingGame.GameStatus;

                // Skifter titlen paa modal fra "Tilfoej spil" til "Rediger spil".
                Title = "Rediger spil";
                // Og overskriften i modalens header fra "Tilfoej spil" til "Rediger spil":
                HeaderTextBlock.Text = "Rediger spil";
                // Og knapteksten fra "Tilfoej spil" til "Gem aendringer":
                AddButton.Content = "Gem ændringer";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TitleTextBox.Text;
                string genre = GenreTextBox.Text;
                string[] parts = PlayersTextBox.Text.Split('-');
                int minPlayers = int.Parse(parts[0]);
                int maxPlayers = parts.Length > 1 ? int.Parse(parts[1]) : minPlayers;
                GameCondition gameCondition = (GameCondition)ConditionComboBox.SelectedItem;
                GameStatus gameStatus = (GameStatus)StatusComboBox.SelectedItem;

                // Fanger hvis PriceTextBox er tom (eller indeholder kun whitespace) for at undgaa FormatException ved decimal.Parse:
                if (string.IsNullOrWhiteSpace(PriceTextBox.Text))
                {
                    throw new ArgumentException("Du har glemt at indtaste en pris.");
                }

                decimal price = decimal.Parse(PriceTextBox.Text);
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
            // Fanger valideringsfejl fra BoardGame.Validate() og viser en besked til brugeren:
            // (Selve teksten kommer fra ArgumentException i BoardGame.cs).
            catch (ArgumentException ex)
            {
                MessageBox.Show($"{ex.Message}", "Hov!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Fanger hvis PriceTextBox er tom eller indeholder ugyldigt format for decimal.Parse:
            catch (FormatException)
            {
                MessageBox.Show($"Du mangler at udfylde et felt!", "Hov!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Fanger hvis ComboBox.SelectedItem er null:
            catch (NullReferenceException)
            {
                MessageBox.Show("Du skal vælge en stand for spillet.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }
        }
        internal BoardGame NewBoardGame { get; private set; }
    }
}