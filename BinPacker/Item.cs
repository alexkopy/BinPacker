using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinPacker
{
    public sealed class Item
    {
        public double Size { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Size.ToString() + " - " + Name;
        }
    }
}
