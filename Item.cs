using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOnV1
{
 public class Item
    {
       public string ItemDescription { get; set; }

       public string ItemCategory { get; set; }

      public decimal ItemPrice { get; set; }

      public bool IsImported { get; set; }

      public decimal ItemSalesTax { get; set; }
      
        public decimal ItemImportTax { get; set; }

        public decimal ItemTotal { get; set; }
    }
}
