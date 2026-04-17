using System.Collections.Generic;
using GenspilWPF.Models;
using System.IO;

// BoardGameRepository klassen implementerer IRepository interfacet for loade og gemme BoardGame objekter til en tekstfil.
// Den bruger en filsti som parameter i constructoren for at vide hvor data skal gemmes og loades fra.
namespace GenspilWPF.Repositories
{
    internal class BoardGameRepository : IRepository<BoardGame>
    {
        // Filsti fra MainWindow.xaml.cs (Dette er ikke helt korrekt, i MVVM men det er en midlertidig loesning:
        private string _filePath;

        public BoardGameRepository(string filePath)
        {
            _filePath = filePath;
        }

        // LoadAll metoden loader alle BoardGame objekter fra tekstfilen. Hvis filen ikke findes, returneres en tom liste.
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

        // SaveAll metoden gemmer alle BoardGame objekter til tekstfilen:
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
