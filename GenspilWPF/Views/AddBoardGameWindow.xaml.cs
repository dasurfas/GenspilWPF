using GenspilWPF.Models;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows;

namespace GenspilWPF.Views
{
    /// <summary>
    /// Interaction logic for AddBoardGameWindow.xaml
    /// </summary>
    public partial class AddBoardGameWindow : Window
    {
        public AddBoardGameWindow()
        {
            InitializeComponent();

            ConditionComboBox.ItemsSource = Enum.GetValues(typeof(GameCondition));
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
            GameStatus gameStatus = GameStatus.PaaLager;
            decimal price = decimal.TryParse(PriceTextBox.Text, out decimal p) ? p : 0;
            string notes = NotesTextBox.Text;

            // Opret et nyt BoardGame-objekt med de indtastede oplysninger
            NewBoardGame = new BoardGame(title, genre, minPlayers, maxPlayers, price, gameCondition, gameStatus, notes);

            this.Close();

        }
        internal BoardGame NewBoardGame { get; private set; }
    }
}