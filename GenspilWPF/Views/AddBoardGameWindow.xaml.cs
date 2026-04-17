using GenspilWPF.Models;
using System;
using System.Collections.Generic;
using System.Windows;

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

            // Gemmer det eksisterende spil i en private field saa vi kan bruge det senere i AddButton_Click for at bevare ID ved redigering:
            _existingGame = existingGame;
            // Sætter ComboBox ItemsSource til enum vaerdier for GameCondition:
            ConditionComboBox.ItemsSource = Enum.GetValues(typeof(GameCondition));
            // Sætter default vaerdi for ConditionComboBox til "God" (hvis det er et nyt spil der skal tilfoejes):
            ConditionComboBox.SelectedItem = GameCondition.God;

            // Midlertidig loesning for at aendre navn paa ComboBox. TODO: Flyt ind i hjaelper class (MVVM):
            // Dictionary for at mappe enum vaerdier til mere brugervenlige tekster i ComboBox:
            var statusItems = new Dictionary<GameStatus, string>
            {
                { GameStatus.På_Lager, "På lager" },
                { GameStatus.Ikke_På_Lager, "Ikke på lager" },
                { GameStatus.Reserveret, "Reserveret" },
                { GameStatus.TilReparation, "Til reparation" },
                { GameStatus.PåVej, "På vej" },
                { GameStatus.Solgt, "Solgt" }
            };

            // Sæt ComboBox ItemsSource til dictionary vaerdier og brug Key som SelectedValue:
            // Key er enum vaerdien (GameStatus.På_Lager) og Value er den brugervenlige tekst ("På lager"):
            StatusComboBox.ItemsSource = statusItems;
            StatusComboBox.DisplayMemberPath = "Value";
            StatusComboBox.SelectedValuePath = "Key";
            StatusComboBox.SelectedValue = GameStatus.På_Lager;


            // Hvis et existing game eksisterer saa hent info:
            if (existingGame != null)
            {
                TitleTextBox.Text = existingGame.Title;
                GenreTextBox.Text = existingGame.Genre;
                PlayersTextBox.Text = existingGame.PlayerRange;
                PriceTextBox.Text = existingGame.Price.ToString();
                NotesTextBox.Text = existingGame.Notes;
                ConditionComboBox.SelectedItem = existingGame.GameCondition;
                StatusComboBox.SelectedValue = existingGame.GameStatus;

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
                GameStatus gameStatus = (GameStatus)StatusComboBox.SelectedValue;

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