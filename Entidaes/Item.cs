using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades 
{
   public class Item : NapseEntity
    {
        //public Item() { } // constructor vacío requerido
        public string itemCode { get; set; }
        public decimal stockUnits { get; set; }
    }
}
