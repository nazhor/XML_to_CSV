using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoCSV.DialogData
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Actor()
        {
        }

        public Actor(int pId, string pName)
        {
            Id = pId;
            Name = pName;        
        }

        public void ResetActor()
        {
            Id = 0;
            Name = "";            
        }
    }
}
