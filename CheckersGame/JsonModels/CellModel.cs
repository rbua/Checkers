using CheckersGame.Models;

namespace CheckersGame.JsonModels
{
    public class CellModel
    {
        public Position CellPosition { get; set; }
        public bool Disabled { get; set; }
        public Marker Owner { get; set; }

        public CellModel(Position cellPosition, Marker owner, bool disabled)
        {
            CellPosition = cellPosition;
            Disabled = disabled;
            Owner = owner;
        }
    }
}
