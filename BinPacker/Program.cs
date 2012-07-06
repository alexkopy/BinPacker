using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinPacker
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                PrintHelp();
                return;
            }

            var path = args[0];
            var numberOfBins = Convert.ToInt32(args[1]);
            var numberOfBytes = Convert.ToInt32(args[2]);

            var bins = InitializeBins(numberOfBins, numberOfBytes);

            List<Item> items = ParseCsv(path);

            try
            {
                AllocateItemsToBins(bins, items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            PrintBinContents(bins);

            Console.ReadKey();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: BinPacker \"path\to\file\" #ofBins");
            Console.WriteLine("\te.g. BinPacker \"c:\\csv_listing.txt\" 15");
        }

        private static Bin[] InitializeBins(int numberOfBins, int numberOfBytes)
        {
            var bins = new Bin[numberOfBins];
            for (int i = 0; i < numberOfBins; i++)
            {
                bins[i] = new Bin(numberOfBytes);
            }
            return bins;
        }

        /// <summary>
        /// Parse the CSV file in format "length in bytes, name" into a List of Item objects
        /// </summary>
        private static List<Item> ParseCsv(string path)
        {
            var sizeToNameMap = File.ReadAllLines(path);
            var items = new List<Item>(sizeToNameMap.Length);

            foreach (var line in sizeToNameMap)
            {
                string[] tuple = line.Split(',');
                var item = new Item();

                item.Size = Convert.ToDouble(tuple[0]);
                item.Name = tuple[1].Trim();

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Longest Processing Time algorith (from scheduling theory):
        /// Sort the items by their processing time and then assigns them to the bin with the lowest fill level so far
        /// </summary>
        private static void AllocateItemsToBins(Bin[] bins, List<Item> items)
        {
            if (bins == null || bins.Length < 1)
                throw new ArgumentException("The bins array may not be null or empty", "bins");
            if(items == null)
                throw new ArgumentException("The list of items cannot be uninitialized", "sortedItems");

            List<Item> sortedItems = items.OrderByDescending(x => x.Size).ToList();

            foreach (var item in sortedItems)
            {
                Bin bin = FindLeastFilledBin(bins, item.Size);
                bin.Fill += item.Size;
                bin.Items.Add(item);
            }
        }

        private static Bin FindLeastFilledBin(Bin[] bins, double itemSize)
        {
            if (bins == null || bins.Length < 1)
                throw new ArgumentException("The bins array may not be null or empty", "bins");

            var candidate = bins.OrderBy(x => x.Fill).First();

            if (candidate.Fill + itemSize > candidate.Size)
                throw new Exception("There are not enough bins to hold all the items");

            return candidate;
        }

        private static void PrintBinContents(Bin[] bins)
        {
            if (bins == null || bins.Length < 1)
                return;

            for (int i = 0; i < bins.Length; i++)
            {
                var header = string.Format("Bin #{0} ({1}%)", i + 1, Math.Round(bins[i].Fill / bins[i].Size * 100, 1));
                Console.WriteLine(header);

                foreach (var item in bins[i].Items)
                {
                    Console.WriteLine("\t" + item.Name);
                }
            }
        }
    }
}
