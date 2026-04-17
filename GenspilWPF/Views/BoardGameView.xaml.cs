using GenspilWPF.Models;
using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;


namespace GenspilWPF.Views
{
    // Interaction logic for BoardGameView.xaml:
    public partial class BoardGameView : UserControl
    {
        public BoardGameView()
        {
            InitializeComponent();

            // Virker ikke helt: TODO: Skal autovaelge Alle og alle skal vise alle spil uanset stand / condition.
            var items = new List<object> { "Alle" };
            items.AddRange(Enum.GetValues(typeof(GameCondition)).Cast<object>());
            ConditionComboBox.ItemsSource = items;
            ConditionComboBox.SelectedIndex = 0;
        }

        private void ConditionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // vm (viewmodel) er DataContext castet til BoardGameViewModel for at faa adgang til SearchCondition property:
            var vm = DataContext as ViewModels.BoardGameViewModel;

            // Hvis comboboksen ikke er tom (f.eks. "Alle" valgt):
            if (vm != null)
            {
                // Hvis en specifik stand er valgt, opdater SearchCondition i ViewModel:
                if (ConditionComboBox.SelectedItem is GameCondition selectedCondition)
                {
                    vm.SearchCondition = selectedCondition;
                }
                // Hvis "Alle" er valgt, nulstil SearchCondition for at vise alle spil:
                else
                {
                    vm.SearchCondition = null; // "Alle" valgt, ingen filter.
                }
            }
        }
    }
}