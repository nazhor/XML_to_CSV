using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoCSV.DialogData
{
    public class Dialog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Actor { get; set; }
        public int Conversant { get; set; }
        public string MenuText { get; set; }
        public string DialogText { get; set; }
        public string Music { get; set; }
        /*public int OriginConvoID { get; set; }
        public int DestinationConvoID { get; set; }*/
        public int OriginDialogID { get; set; }
        public string DestinationDialogIDs { get; set; }
        public bool Interruptible { get; set; }
        public bool Interruption { get; set; }
        public bool Multispeech { get; set; }
        public int ChangeOfMind { get; set; }
        public bool Silence { get; set; }
        public bool EsFinal { get; set; }
        
        public Dialog()
        {
        }

        public Dialog(int pId, string pTitle, int pActor, int pConversant, string pMenuText, string pDialogText, string pMusic, int pOriginDialogID, string pDestinationDialogIDs, bool pInterruptible, bool pInterruption, bool pMultispeech, int pChangeofMind, bool pSilence, bool pEsFinal)
        {
            Id = pId;
            Title = pTitle;
            Actor = pActor;
            Conversant = pConversant;
            MenuText = pMenuText;
            DialogText = pDialogText;
            Music = pMusic;            
            OriginDialogID = pOriginDialogID;
            DestinationDialogIDs = pDestinationDialogIDs;
            Interruptible = pInterruptible;
            Interruption = pInterruption;
            Multispeech = pMultispeech;
            ChangeOfMind = pChangeofMind;
            Silence = pSilence;
            EsFinal = pEsFinal;
        }

        public void ResetDialog()
        {
            Id = 0;
            Title = "";
            Actor = 0;
            Conversant = 0;
            MenuText = "";
            DialogText = "";
            Music = "";
            OriginDialogID = 0;
            DestinationDialogIDs = "";
            Interruptible = false;
            Interruption = false;
            Multispeech = false;
            ChangeOfMind = 0;
            Silence = false;
            EsFinal = false;
        }
    }
}
