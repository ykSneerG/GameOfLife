using System;
using System.Collections.Generic;

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


    //public struct Coord2D
    //{
    //    public byte X;

    //    public byte Y;

    //    public Coord2D(byte x, byte y)
    //    {
    //        X = x;
    //        Y = y;
    //    }
    //}


    public static class GolHelper
    {

        public static readonly Tuple<sbyte, sbyte>[] NextCells = new Tuple<sbyte, sbyte>[]
        {
            new Tuple<sbyte, sbyte>(-1, -1),
            new Tuple<sbyte, sbyte>( 0, -1),
            new Tuple<sbyte, sbyte>( 1, -1),
            new Tuple<sbyte, sbyte>(-1,  0),
            new Tuple<sbyte, sbyte>( 1,  0),
            new Tuple<sbyte, sbyte>(-1,  1),
            new Tuple<sbyte, sbyte>( 0,  1),
            new Tuple<sbyte, sbyte>( 1,  1)
        };


        public static Dictionary<TKey, TValue> MergeLeft<TKey, TValue>(params Dictionary<TKey, TValue>[] dicts)
        {

            Dictionary<TKey, TValue> mergedDict = new Dictionary<TKey, TValue>();

            for (int i = 0; i < dicts.Length; i++)
            {

                foreach (KeyValuePair<TKey, TValue> entry in dicts[i])
                {
                    _ = mergedDict.TryAdd(entry.Key, entry.Value);
                }
            }

            return mergedDict;
        }
    }

}