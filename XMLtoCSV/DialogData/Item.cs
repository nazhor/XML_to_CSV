using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoCSV.DialogData
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Item()
        {
        }

        public Item(int pId, string pName)
        {
            Id = pId;
            Name = pName;        
        }

        public void ResetItem()
        {
            Id = 0;
            Name = "";            
        }
    }
}
