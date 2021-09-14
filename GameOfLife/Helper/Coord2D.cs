using System;

namespace GameOfLife
{

    public struct Coord2D : IEquatable<Coord2D>
    {

        public byte X;

        public byte Y;

        public Coord2D(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public Coord2D(int x, int y)
        {
            X = (byte)x;
            Y = (byte)y;
        }


        public bool Equals(Coord2D other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
    }

}