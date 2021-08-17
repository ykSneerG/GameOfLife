namespace GameOfLife
{

    public enum Status { Life, Dead }


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

    public class Cell
    {

        public Coord2D Position { get; set; }

        public Status Status { get; set; } = Status.Dead;

        public Status StatusNext { get; set; } = Status.Dead;



        public Cell(byte x, byte y)
        {
            Position = new Coord2D(x, y);
        }



        public void StatusToggle()
        {
            Status = (Status == Status.Dead) ? Status.Life : Status.Dead;
        }

        public void SetStatus(Status status)
        {
            Status = status;
        }


        public void NextGeneration()
        {
            Status = StatusNext;
        }

    }

}