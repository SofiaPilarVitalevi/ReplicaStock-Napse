using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Stock : NapseEntity
    {
        //public Stock() { } // constructor vacío requerido

        public string storeCode { get; set; }
        public string locationCode { get; set; }
        public string itemInventoryState { get; set; }
        public string revenueCenter { get; set; }
        public List<Item> item { get; set; }

    }
}
