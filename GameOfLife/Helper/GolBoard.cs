using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class GolBoard
    {

        public Coord2D Amount;

        public int AmountOfCells => Amount.X * Amount.Y;


        public GolBoard()
        {
            Amount = new Coord2D();
        }


        public void SetBoardWidth(byte width)
        {
            Amount.X = width;
        }

        public void SetBoardHeight(byte height)
        {
            Amount.Y = height;
        }


        public IEnumerable<Coord2D> RandomCells(double percentage)
        {

            HashSet<Coord2D> randLife = new HashSet<Coord2D>();

            double m = AmountOfCells * (percentage / 100);

            int n = 0;

            Random random = new Random();

            while (n < m)
            {
                int randX = random.Next(0, Amount.X - 1);
                int randY = random.Next(0, Amount.Y - 1);

                bool res = randLife.Add(new Coord2D((byte)randX, (byte)randY));

                if (res)
                {
                    n++;
                }
            }

            return randLife;
        }


        public static Coord2D NeighbourCell(Coord2D amount, Coord2D center, sbyte nX, sbyte nY)
        {

            byte x = NeighbourCoordinate(center.X, nX, amount.X);

            byte y = NeighbourCoordinate(center.Y, nY, amount.Y);

            return new Coord2D(x, y);
        }

        public static byte NeighbourCoordinate(byte pt, sbyte dist, byte amount)
        {

            int ptdist = pt + dist;

            if (ptdist >= amount)
            {
                ptdist = 0;
            }
            else if (ptdist < 0)
            {
                ptdist = amount - 1;
            }

            return (byte)ptdist;
        }

    }

}