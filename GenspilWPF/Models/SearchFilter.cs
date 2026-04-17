namespace GenspilWPF.Models
{
    // SearchFilter klassen bruges til at holde information om de forskellige filterkriterier,
    // som brugeren kan indtaste i soegefeltet for at filtrere braetspillene i BoardGameViewModel:
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