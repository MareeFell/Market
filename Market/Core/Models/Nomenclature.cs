using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Models
{
    public class Nomenclature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SellPrice { get; set; }
        public int BuyPrice { get; set; }
        public int Count { get; set; }
        public Category Category { get; set; }
    }
}
