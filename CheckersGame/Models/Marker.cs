namespace CheckersGame.Models
{
    public enum MarkerColor
    {
        Undefined = 0,
        Black = 1,
        White = 2
    }

    public class Marker
    {
        public bool IsQueen { get; set; }
        public MarkerColor Color { get; set; }

        public Marker()
        {
            IsQueen = false;
        }
    }
}
