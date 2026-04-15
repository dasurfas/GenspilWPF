namespace GenspilWPF.Models
{
    // Enum til at holde status for spil, om det er på lager eller ej.
    // Bruges i BoardGame.cs og i AddEditBoardGameWindow.xaml.cs.
    internal enum GameStatus
        {
            På_Lager,
            Ikke_På_Lager,
            Reserveret,
            TilReparation,
            PåVej,
            Solgt
        }
}
