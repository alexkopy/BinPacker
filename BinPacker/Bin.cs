using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinPacker
{
    public sealed class Bin
    {
        public double Size { get; set; }
        public double Fill { get; set; }
        public List<Item> Items { get; private set; }

        public Bin()
        {
            Items = new List<Item>();
        }

        public Bin(double size) : this()
        {
            Size = size;
        }

        public Bin(double size, double fill) : this(size)
        {
            Fill = fill;
        }
    }
}
