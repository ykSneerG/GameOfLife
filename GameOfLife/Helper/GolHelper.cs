namespace GameOfLife
{

    public enum Status { Life, Dead, Surround }


    public struct Coord2D
    {
        public byte X;

        public byte Y;

        public Coord2D(byte x, byte y)
        {
            X = x;
            Y = y;
        }
    }

}