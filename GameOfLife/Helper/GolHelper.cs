using System;
using System.Collections.Generic;

namespace GameOfLife
{

    public enum Status { Life, Dead, Surround }


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


        /// <summary>
        /// Stores a given dictonary at the first positon of an array an pushes all existing dictonaries.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="records">Array of Dictonaries.</param>
        /// <param name="start">Dictonary to RECORD</param>
        public static void Record<TKey, TValue>(ref Dictionary<TKey, TValue>[] records, Dictionary<TKey, TValue> start)
        {

            for (int i = records.Length - 1; i >= 0; i--)
            {
                records[i].Clear();

                Dictionary<TKey, TValue> dictTemp = (i == 0) ? start : records[i - 1];

                foreach (KeyValuePair<TKey, TValue> entry in dictTemp)
                {
                    records[i].Add(entry.Key, entry.Value);
                }
            }
        }

        public static double FractionToPercentage(int nominator, int denominator)
        {
            return Math.Round(nominator / (double)denominator * 100, 1);
        }

    }

}