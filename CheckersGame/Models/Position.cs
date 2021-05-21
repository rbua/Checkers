using System;

namespace CheckersGame.Models
{
    public class Position : IEquatable<Position>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }      

        public bool Equals(Position other)
        {
            return other != null && other.X.Equals(this.X) && other.Y.Equals(this.Y);         
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 23 + this.X.GetHashCode();
                hash = hash * 23 + this.Y.GetHashCode();
                return hash;
            }
        }

    }
}
