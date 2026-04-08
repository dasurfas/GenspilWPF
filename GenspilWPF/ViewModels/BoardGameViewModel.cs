using GenspilWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GenspilWPF.Models;

namespace GenspilWPF.ViewModels
{
    internal class BoardGameViewModel : INotifyPropertyChanged
    {
        private GenspilService _service;

        // ObservableCollection er en samling, der automatisk opdaterer UI'et, når elementer tilfoejes eller fjernes.
        ObservableCollection<BoardGame> _boardGames;

        public ObservableCollection<BoardGame> BoardGames
        {
            get { return _boardGames; }
            set
            {
                _boardGames = value;
                OnPropertyChanged(nameof(BoardGames));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BoardGameViewModel(GenspilService service)
        {
            _service = service;
            _boardGames = new ObservableCollection<BoardGame>(_service.GetAllBoardGames());
        }
    }
}