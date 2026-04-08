using GenspilWPF.Models;
using GenspilWPF.Repositories;
using System.Collections.Generic;

namespace GenspilWPF.Services
{
    internal class GenspilService
    {
        // readonly saa vi ikke kan aendre variabler efter de er blevet sat i contructoren.
        private readonly IRepository<BoardGame> _gameRepo;
        private readonly IRepository<Reservation> _reservationRepo;

        private readonly List<BoardGame> _games;
        private readonly List<Reservation> _reservations;

        public GenspilService(IRepository<BoardGame> gameRepo, IRepository<Reservation> reservationRepo)
        {
            _gameRepo = gameRepo;
            _reservationRepo = reservationRepo;

            _games = _gameRepo.LoadAll();
            _reservations = _reservationRepo.LoadAll();
        }

        public void AddBoardGame(BoardGame game)
        {
            _games.Add(game);
            _gameRepo.SaveAll(_games);
        }

        public void RemoveBoardGame(BoardGame game)
        {
            _games.Remove(game);
            _gameRepo.SaveAll(_games);
        }

        public List<BoardGame> GetAllBoardGames()
        {
            return _games;
        }

        public void UpdateBoardGame(BoardGame game)
        {
            for (int i = 0; i < _games.Count; i++)
            {
                if (_games[i].Id == game.Id)
                {
                    _games[i] = game;
                    break;
                }
            }
            _gameRepo.SaveAll(_games);
        }

        public List<BoardGame> Search(SearchFilter filter)
        {
            List<BoardGame> results = new List<BoardGame>();

            foreach (var game in _games)
            {
                bool match = true;

                if (!string.IsNullOrEmpty(filter.Title) && !game.Title.ToLower().Contains(filter.Title.ToLower()))
                    match = false;
                if (!string.IsNullOrEmpty(filter.Genre) && !game.Genre.ToLower().Contains(filter.Genre.ToLower()))
                    match = false;
                if (filter.MinPlayers.HasValue && game.MinPlayerCount < filter.MinPlayers.Value)
                    match = false;
                if (filter.MaxPlayers.HasValue && game.MaxPlayerCount > filter.MaxPlayers.Value)
                    match = false;
                if (filter.MaxPrice.HasValue && game.Price > filter.MaxPrice.Value)
                    match = false;
                if (filter.Condition.HasValue && game.GameCondition != filter.Condition)
                    match = false;
                if (filter.Status.HasValue && game.GameStatus != filter.Status)
                    match = false;

                if (match) results.Add(game);
            }

            return results;
        }

        // reservation = r i klassediagram.
        // reservationStatus = r i klassediagram.
        // Hvis jeg skal opdatere status på en reservation, så skal jeg finde den i listen og opdatere den. Det er det jeg goer i UpdateReservationStatus metoden. Jeg sammenligner game title og customer name for at finde den rigtige reservation, og så opdaterer jeg status på den. Efter det gemmer jeg alle reservationerne igen for at sikre at aendringen bliver gemt.
        public void AddReservation(Reservation reservation)
        {
            _reservations.Add(reservation);
            _reservationRepo.SaveAll(_reservations);
        }

        public void RemoveReservation(Reservation reservation)
        {
            _reservations.Remove(reservation);
            _reservationRepo.SaveAll(_reservations);
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservations;
        }

        public void UpdateReservationStatus(Reservation reservation, ReservationStatus newStatus)
        {
            for (int i = 0; i < _reservations.Count; i++)
            {
                if (_reservations[i].GameTitle == reservation.GameTitle &&
                    _reservations[i].CustomerName == reservation.CustomerName)
                {
                    _reservations[i].UpdateStatus(newStatus);
                    break;
                }

            }
            _reservationRepo.SaveAll(_reservations);
        }
    }
}