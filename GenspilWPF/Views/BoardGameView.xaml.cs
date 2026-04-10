using GenspilWPF.Models;
using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;


namespace GenspilWPF.Views
{
    /// <summary>
    /// Interaction logic for BoardGameView.xaml
    /// </summary>
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
    }
}