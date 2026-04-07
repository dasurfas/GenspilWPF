using System.Collections.Generic;
using GenspilWPF.Models;
using System.IO;

namespace GenspilWPF.Repositories
{
    internal class BoardGameRepository : IRepository<BoardGame>
    {
        private string _filePath;

        public BoardGameRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<BoardGame> LoadAll()
        {
            List<BoardGame> games = new List<BoardGame>();
            if (!File.Exists(_filePath)) return games; // Returner en tom liste, hvis filen ikke findes

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        games.Add(BoardGame.FromString(line));
                    }
                    catch
                    {
                        // Log fejl eller ignorer ugyldige linjer
                    }
                }
            }
            return games; // Returner den indlaeste liste af spil
        }

        public void SaveAll(List<BoardGame> items)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false)) // false for at overskrive filen
            {
                foreach (var game in items)
                {
                    writer.WriteLine(game.ToString());
                }
            }
        }
    }
}
