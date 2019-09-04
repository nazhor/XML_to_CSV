using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoCSV.DialogData
{
    public class Conversation
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Dialog> Dialogos { get; set; }

        public Conversation()
        {
            Id = 0;
            Title = "";
            Dialogos = new List<Dialog>();
        }

        public Conversation(int pId, string pTitle, List<Dialog> pDialogos)
        {
            Id = pId;
            Title = pTitle;
            Dialogos = pDialogos;
        }



        public void ResetConversation()
        {
            Id = 0;
            Title = "";
            Dialogos = new List<Dialog>();
        }

        //Devuelve un diálogo dado un int ID
        public Dialog findDialogNode(int pId)
        {
            int Index = 0;
            bool Found = false;
            Dialog DFound = new Dialog();

            while (!Found)
            {
                if(Dialogos[Index].Id == pId)
                {
                    Found = true;
                    DFound = Dialogos[Index];
                }
                Index++;
            }

            return DFound;
        }
    }
}
