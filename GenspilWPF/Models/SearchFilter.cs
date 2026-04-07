using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenspilWPF.Models
{
    internal class SearchFilter
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int? MinPlayers { get; set; }
        public int? MaxPlayers { get; set; }
        public decimal? MaxPrice { get; set; }
        public GameCondition? Condition { get; set; }
        public GameStatus? Status { get; set; }
    }
}