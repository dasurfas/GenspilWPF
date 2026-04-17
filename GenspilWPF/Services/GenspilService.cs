using GenspilWPF.Models;
using GenspilWPF.Repositories;
using System.Collections.Generic;

namespace GenspilWPF.Services
{

    // Datahandler klassen som sidder imellem repositories og view models og haandterer forretningslogikken.
    internal class GenspilService
    {
        // readonly saa vi ikke kan aendre variabler efter de er blevet sat i contructoren:
        private readonly IRepository<BoardGame> _gameRepo;
        private readonly IRepository<Reservation> _reservationRepo;

        // Lister der holder data i RAM mens programmet koerer.
        private readonly List<BoardGame> _games;
        private readonly List<Reservation> _reservations;

        // Constructor: Indlaeser spil og reservationer fra fil ved opstart (LoadAll()):
        public GenspilService(IRepository<BoardGame> gameRepo, IRepository<Reservation> reservationRepo)
        {
            _gameRepo = gameRepo;
            _reservationRepo = reservationRepo;

            _games = _gameRepo.LoadAll();
            _reservations = _reservationRepo.LoadAll();
        }

        // Tilfoejer nyt spil  til listen og gemmer den i filen:
        public void AddBoardGame(BoardGame game)
        {
            _games.Add(game);
            _gameRepo.SaveAll(_games);
        }

        // Fjerner spil fra listen og gemmer listen igen (uden det fjernede spil):
        public void RemoveBoardGame(BoardGame game)
        {
            _games.Remove(game);
            _gameRepo.SaveAll(_games);
        }

        // Returnerer all spil fra RAM:
        public List<BoardGame> GetAllBoardGames()
        {
            // Opretter en ny liste for at undga at returnere den originale liste:
            List<BoardGame> result = new List<BoardGame>();
            // Tilfoejer kun spil der ikke er solgt til den nye liste:
            foreach (var game in _games)
            {
                if (game.GameStatus != GameStatus.Solgt)
                    result.Add(game);
            }
            // Returnerer den nye liste:
            return result;
        }

        // Finder spillet med det samme Id (== game.Id), og erstatter det eksisterende spil.
        // Gemmer derefter hele listen til fil.
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

        // Finder reservationer med samme Id, og erstatter den eksisterende reservation.
        // Gemmer til fil.
        public void UpdateReservation(Reservation reservation)
        {
            for (int i = 0; i < _reservations.Count; i++)
            {
                if (_reservations[i].Id == reservation.Id)
                {
                    _reservations[i] = reservation;
                    break;
                }
            }
            _reservationRepo.SaveAll(_reservations);
        }

        // Soeger efter spil baseret paa SearchFilter. Kun matchende spil der udfylder ALLE kriterier returneres.
        // TODO: Ikke den bedste metode og mangler en "reset after search funktion". Think think...
        public List<BoardGame> Search(SearchFilter filter)
        {
            List<BoardGame> results = new List<BoardGame>();

            // Gaar igennem alle spil i hukommelsen og tjekker om de matcher kriterierne i SearchFilter.
            // Hvis et spil ikke matcher, saetter vi "match" til false.
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
                // Hvis en status er valgt OG spillet ikke matcher: Skjul:
                if (filter.Status.HasValue && game.GameStatus != filter.Status)
                    match = false;
                // Hvis ingen status er valgt OG spillet er solgt: Skjul:
                if (!filter.Status.HasValue && game.GameStatus == GameStatus.Solgt)
                    match = false;

                if (match) results.Add(game);
            }

            return results;
        }

        // Tilfoejer ny reservation og gemmer til fil.
        public void AddReservation(Reservation reservation)
        {
            _reservations.Add(reservation);
            _reservationRepo.SaveAll(_reservations);
        }

        // Fjerner reservation fra listen og gemmer den nye liste til fil.
        public void RemoveReservation(Reservation reservation)
        {
            _reservations.Remove(reservation);
            _reservationRepo.SaveAll(_reservations);
        }

        // Returnerer alle reservationer fra hukommelse (RAM).
        public List<Reservation> GetAllReservations()
        {
            return _reservations;
        }

        // Finder reservation med samme Id (== reservation.Id), og opdaterer dens status.
        // Gemmer derefter listen til fil.
        public void UpdateReservationStatus(Reservation reservation, ReservationStatus newStatus)
        {
            for (int i = 0; i < _reservations.Count; i++)
            {
                if (_reservations[i].Id == reservation.Id)
                {
                    _reservations[i].UpdateStatus(newStatus);
                    break;
                }

            }
            _reservationRepo.SaveAll(_reservations);
        }
    }
}