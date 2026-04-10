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
        // privat felt med reference til GenspilService (som har alt forretningslogik).
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

        // Constructor: Initialiserer service og indlaeser alle braetspil fra fil og kobler kommandoer til deres metoder i RelayCommand.
        public BoardGameViewModel(GenspilService service)
        {
            _service = service;
            _boardGames = new ObservableCollection<BoardGame>(_service.GetAllBoardGames());
            SearchCommand = new RelayCommand(Search);
            AddBoardGameCommand = new RelayCommand(AddBoardGame);
            DeleteBoardGameCommand = new RelayCommand(DeleteBoardGame);
            EditBoardGameCommand = new RelayCommand(UpdateBoardGame);
        }
        // Soeger efter spil baseret paa udfyldte soegefelter.
        // Opretter et SearchFilter objekt og sender det til GenspilService
        // Resultatet opdaterer BoardGames listen som DataGrid binder til.
        private void Search()
        {
            var filter = new SearchFilter
            {
                Title = SearchTitle,
                Genre = SearchGenre,
                MinPlayers = SearchMinPlayers,
                MaxPlayers = SearchMaxPlayers,
                MaxPrice = SearchMaxPrice,
                // Opdater Condition til den valgte soegetilstand eller standard til "Alle" (null). TODO: Faa det til at virke.
                Condition = SearchCondition is GameCondition gc ? gc : (GameCondition?)null,
                Status = SearchStatus
            };
            var results = _service.Search(filter);
            BoardGames = new ObservableCollection<BoardGame>(results);

            // TODO: ClearInput() efter soegning.
        }

        // Aabner "Tilfoej spil" vinduet som modal (popup).
        // Hvis brugeren gemmer saa tilfoejes spillet til service (fil) og BoardGames listen (UI).
        private void AddBoardGame()
        {
            AddBoardGameWindow addBoardGameWindow = new AddBoardGameWindow();
            // Modal:
            addBoardGameWindow.ShowDialog();
            if (addBoardGameWindow.NewBoardGame != null)
            {
                _service.AddBoardGame(addBoardGameWindow.NewBoardGame); // Gem til fil.
                BoardGames.Add(addBoardGameWindow.NewBoardGame); // Opdater UI.
            }
        }

        // Sletter det valgte spil fra fil (service) og BoardGames listen (UI):
        // Returnerer tidligt hvis intet spil er valgt.
        private void DeleteBoardGame()
        {
            if (_selectedBoardGame == null) return; // Intet spil valgt, intet at slette
            {
                _service.RemoveBoardGame(_selectedBoardGame); // Slet fra fil.
                BoardGames.Remove(_selectedBoardGame); // Opdater UI'et.
            }
        }

        // Aabner redigerings vinduet med det valgte spils data udfyldt.
        // Hvis brugeren gemmer, saa opdateres spillet i service (fil) og paa den korrekte position i BoardGames (UI).
        // TODO: Aendre knap-teksten "Tilfoej spil" til "Opdater" i dette vindue.
        private void UpdateBoardGame()
        {
            if (_selectedBoardGame == null) return;

            var window = new AddBoardGameWindow(_selectedBoardGame);
            window.ShowDialog();

            if (window.NewBoardGame != null)
            {
                _service.UpdateBoardGame(window.NewBoardGame);
                int index = BoardGames.IndexOf(_selectedBoardGame);
                BoardGames[index] = window.NewBoardGame;
            }
        }

        // Boardgames er listen DataGrid (xaml) binder (binding) til.
        // OnProptertyChanged sikrer at UI opdateres naar listen udskiftes (f.eks. efter soegning).
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

        // Commands til at udfoere Click. ICommand kommer System.Windows.Input og bruges til at binde (binding)
        // knapper til metoder.
        // INotifyPropertyChanged bruges til at opdatere UI naar Properties aendrer sig.
        public ICommand SearchCommand { get; }
        public ICommand AddBoardGameCommand { get; }
        public ICommand DeleteBoardGameCommand { get; }
        public ICommand EditBoardGameCommand { get; }
    }
}