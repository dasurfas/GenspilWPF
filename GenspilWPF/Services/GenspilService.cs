using GenspilWPF.Models;
using GenspilWPF.Repositories;
using System;
using System.Collections.Generic;

namespace GenspilWPF.Services
{
    internal class GenspilService
    {
        private IRepository<BoardGame> _gameRepo;
        private IRepository<Reservation> _reservationRepo;

        private List<BoardGame> _games;
        private List<Reservation> _reservations;


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
    }
}