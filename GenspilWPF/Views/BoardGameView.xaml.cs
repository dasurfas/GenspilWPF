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

            ConditionComboBox.ItemsSource = Enum.GetValues(typeof(GameCondition));
        }
    }
}