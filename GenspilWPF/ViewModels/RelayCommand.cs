using System;
using System.Windows.Input;

namespace GenspilWPF.ViewModels
{
    // En simpel implementation af ICommand, som bruges til at binde knapper i UI'et til metoder i ViewModel'erne:
    internal class RelayCommand : ICommand
    {
        // Action er en delegate type der repraesenterer en metode uden parametre og uden returvaerdi.
        // Vi bruger den til at gemme reference til den metode, der skal kaldes naar kommandoen kaldes:
        private readonly Action _execute;

        public event EventHandler CanExecuteChanged;

        // CanExecute bestemmer om kommandoen kan udfoeres. I dette simple eksempel returnerer den altid true:
        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Execute kaldes naar kommandoen udfoeres (f.eks. naar en knap klikkes). Den kalder den metode, der er gemt i _execute:
        public void Execute(object parameter)
        {
            _execute();
        }

        // Constructor. Den tager en Action som parameter, som er den metode der skal kaldes naar kommandoen udfoeres:
        public RelayCommand(Action execute)
        {
            _execute = execute;
        }
    }
}