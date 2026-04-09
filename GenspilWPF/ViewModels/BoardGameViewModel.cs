using GenspilWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GenspilWPF.Models;
using System.Windows.Input;
using GenspilWPF.Views;

namespace GenspilWPF.ViewModels
{
    internal class BoardGameViewModel : INotifyPropertyChanged
    {
        private readonly GenspilService _service;

        // ObservableCollection er en samling, der automatisk opdaterer UI'et, når elementer tilfoejes eller fjernes.
        ObservableCollection<BoardGame> _boardGames;
        private string _searchTitle;
        private string _searchGenre;
        // Nullable int?, decimal? og enums? for at tillade tomme soegefelter uden vaerdi.
        private int? _searchMinPlayers;
        private int? _searchMaxPlayers;
        private decimal? _searchMaxPrice;
        private GameCondition? _searchCondition;
        private GameStatus? _searchStatus;
        private BoardGame _selectedBoardGame;

        // Constructor: Initialiserer kommandoerne og indlaeser alle braetspil ved oprettelse af ViewModel.
        public BoardGameViewModel(GenspilService service)
        {
            _service = service;
            _boardGames = new ObservableCollection<BoardGame>(_service.GetAllBoardGames());
            SearchCommand = new RelayCommand(Search);
            AddBoardGameCommand = new RelayCommand(AddBoardGame);
            DeleteBoardGameCommand = new RelayCommand(DeleteBoardGame);
        }
        private void Search()
        {
            var filter = new SearchFilter
            {
                Title = SearchTitle,
                Genre = SearchGenre,
                MinPlayers = SearchMinPlayers,
                MaxPlayers = SearchMaxPlayers,
                MaxPrice = SearchMaxPrice,
                Condition = SearchCondition is GameCondition gc ? gc : (GameCondition?)null, // Opdater Condition til den valgte oeøgetilstand eller standard til "Alle" (null).
                Status = SearchStatus
            };
            var results = _service.Search(filter);
            BoardGames = new ObservableCollection<BoardGame>(results);
        }

        private void AddBoardGame()
        {
            AddBoardGameWindow addBoardGameWindow = new AddBoardGameWindow();
            addBoardGameWindow.ShowDialog();
            if (addBoardGameWindow.NewBoardGame != null)
            {
                _service.AddBoardGame(addBoardGameWindow.NewBoardGame);
                BoardGames.Add(addBoardGameWindow.NewBoardGame);
            }
        }

        private void DeleteBoardGame()
        {
            if (_selectedBoardGame == null) return; // Intet spil valgt, intet at slette
            {
                _service.RemoveBoardGame( _selectedBoardGame ); // Slet spillet.
                BoardGames.Remove( _selectedBoardGame ); // Opdater UI'et.
            }
        }

        public ObservableCollection<BoardGame> BoardGames
        {
            get { return _boardGames; }
            set
            {
                _boardGames = value;
                OnPropertyChanged(nameof(BoardGames));
            }
        }

        public string SearchTitle
        {
            get { return _searchTitle; }
            set
            {
                _searchTitle = value;
                OnPropertyChanged(nameof(SearchTitle));
            }
        }

        public string SearchGenre
        {
            get { return _searchGenre; }
            set
            {
                _searchGenre = value;
                OnPropertyChanged(nameof(SearchGenre));
            }
        }

        public int? SearchMinPlayers
        {
            get { return _searchMinPlayers; }
            set
            {
                _searchMinPlayers = value;
                OnPropertyChanged(nameof(SearchMinPlayers));
            }
        }

        public int? SearchMaxPlayers
        {
            get { return _searchMaxPlayers; }
            set
            {
                _searchMaxPlayers = value;
                OnPropertyChanged(nameof(SearchMaxPlayers));
            }
        }

        public decimal? SearchMaxPrice
        {
            get { return _searchMaxPrice; }
            set
            {
                _searchMaxPrice = value;
                OnPropertyChanged(nameof(SearchMaxPrice));
            }
        }

        public GameCondition? SearchCondition
        {
            get { return _searchCondition; }
            set
            {
                _searchCondition = value;
                OnPropertyChanged(nameof(SearchCondition));
            }
        }

        public GameStatus? SearchStatus
        {
            get { return _searchStatus; }
            set
            {
                _searchStatus = value;
                OnPropertyChanged(nameof(SearchStatus));
            }
        }

        public BoardGame SelectedBoardGame
        {
            get { return _selectedBoardGame; }
            set
            {
                _selectedBoardGame = value;
                OnPropertyChanged(nameof(SelectedBoardGame));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SearchCommand { get; }
        public ICommand AddBoardGameCommand { get; }
        public ICommand DeleteBoardGameCommand { get; }
    }
}