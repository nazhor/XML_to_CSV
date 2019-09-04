using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoCSV.DialogData
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }        

        public Location()
        {
        }

        public Location(int pId, string pName)
        {
            Id = pId;
            Name = pName;            
        }

        public void ResetLocation()
        {
            Id = 0;
            Name = "";            
        }
    }
}
