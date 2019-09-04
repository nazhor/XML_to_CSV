using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Cargamos el namespace de las clases de datos
using XMLtoCSV.DialogData;

namespace XMLtoCSV
{
    public partial class Form1 : Form
    {

        #region Variables

        //-- Arrays con los datos leidos del fichero XML --
        public static List<Actor> actores = new List<Actor>();
        public static List<Item> items = new List<Item>();
        public static List<Location> localizaciones = new List<Location>();
        public static List<Conversation> conversaciones = new List<Conversation>();

        private enum camposXml
        {
            Actor,
            Item,
            Location,
            Conversation,
            DialogEntry,
            Title,
            Value
        };

        #endregion

        #region Form

        public Form1()
        {
            InitializeComponent();
        }

        private void BT_Convertir_Click(object sender, EventArgs e)
        {
            string pthFile;
            openFileDialog_FindFile.ShowDialog();
            System.IO.FileInfo fInfo = new System.IO.FileInfo(openFileDialog_FindFile.FileName);
            pthFile = fInfo.ToString();

            try
            {
                string xmlText = System.IO.File.ReadAllText(pthFile);

                if (pthFile.Contains(".xml") == false)
                {
                    MessageBox.Show("El fichero tiene que ser de tipo .XML", "Ese tipo de fichero no vale", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    ReadXML(xmlText);
                    UpdateOriginID();
                    if (!ValidateCSV())
                    {
                        WriteCSV();
                    }                  
                }
            }
            catch
            {
                //Por si le das a cancelar al buscar el fichero
            }            
        }

        #endregion

        #region Read XML File

        //Leemos el fichero XML
        private static void ReadXML(string xmlText)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlText)))
            {
                camposXml campoLeido = new camposXml();

                Actor actorLeido = new Actor();
                Item itemLeido = new Item();
                Location localizacionLeida = new Location();
                Conversation conversacionLeida = new Conversation();
                Dialog dialogoLeido = new Dialog();

                string title = "";

                /*
                 * ----------------- ¡NOTA IMPORTANTE! -----------------
                 * Chatmapper toma como indice inicial el 1 y desprecia el 0, como no quiero tener que dejar vacio el 0 en el CSV,
                 * voy a restar 1 a  TODOS los integer que se lean del XML de Chatmapper
                 * ------------------ ¡AVISADO QUEDO! ------------------
                 */
                while (reader.Read())
                {

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "Actor":
                                    actorLeido.Id = int.Parse(reader.GetAttribute("ID")) -1;
                                    campoLeido = camposXml.Actor;
                                    break;
                                case "Item":
                                    itemLeido.Id = int.Parse(reader.GetAttribute("ID")) -1;
                                    campoLeido = camposXml.Item;
                                    break;
                                case "Location":
                                    localizacionLeida.Id = int.Parse(reader.GetAttribute("ID")) -1;
                                    campoLeido = camposXml.Location;
                                    break;
                                case "Conversation":
                                    conversacionLeida.Id = int.Parse(reader.GetAttribute("ID")) -1;
                                    campoLeido = camposXml.Conversation;
                                    break;
                                case "DialogEntry":
                                    dialogoLeido.Id = int.Parse(reader.GetAttribute("ID")) -1;
                                    campoLeido = camposXml.DialogEntry;
                                    break;
                                case "Title":
                                    title = reader.ReadElementString();
                                    break;
                                case "Value":
                                    switch (campoLeido)
                                    {
                                        case camposXml.Actor:
                                            switch (title)
                                            {
                                                case "Name":
                                                    actorLeido.Name = reader.ReadElementString();
                                                    break;
                                            }
                                            break;
                                        case camposXml.Item:
                                            switch (title)
                                            {
                                                case "Name":
                                                    itemLeido.Name = reader.ReadElementString();
                                                    break;
                                            }
                                            break;
                                        case camposXml.Location:
                                            switch (title)
                                            {
                                                case "Name":
                                                    localizacionLeida.Name = reader.ReadElementString();
                                                    break;
                                            }
                                            break;
                                        case camposXml.Conversation:
                                            switch (title)
                                            {
                                                case "Title":
                                                    conversacionLeida.Title = reader.ReadElementString();
                                                    break;   
                                            }
                                            break;
                                        case camposXml.DialogEntry:
                                            switch (title)
                                            {
                                                case "Title":
                                                    dialogoLeido.Title = reader.ReadElementString();
                                                    break;
                                                case "Actor":
                                                    dialogoLeido.Actor = int.Parse(reader.ReadElementString()) -1;
                                                    break;
                                                case "Conversant":
                                                    dialogoLeido.Conversant = int.Parse(reader.ReadElementString()) -1;
                                                    break;
                                                case "Menu Text":
                                                    dialogoLeido.MenuText = reader.ReadElementString();
                                                    break;
                                                case "Dialogue Text":
                                                    dialogoLeido.DialogText = reader.ReadElementString();
                                                    break;
                                                case "Music":
                                                    dialogoLeido.Music = reader.ReadElementString();
                                                    break;
                                                case "Interruptible":
                                                    dialogoLeido.Interruptible = bool.Parse(reader.ReadElementString()); 
                                                    break;
                                                case "Interruption":
                                                    dialogoLeido.Interruption = bool.Parse(reader.ReadElementString());
                                                    break;
                                                case "Multispeech":
                                                    dialogoLeido.Multispeech = bool.Parse(reader.ReadElementString());
                                                    break;
                                                case "ChangeOfMind":
                                                    dialogoLeido.ChangeOfMind = int.Parse(reader.ReadElementString()) -1;
                                                    break;
                                                case "Silence":
                                                    dialogoLeido.Silence = bool.Parse(reader.ReadElementString());
                                                    break;
                                                case "EsFinal":
                                                    dialogoLeido.EsFinal = bool.Parse(reader.ReadElementString());
                                                    break;
                                            }
                                            break;
                                    }
                                    //Debug.Log(title + ": " + reader.ReadElementString());
                                    break;
                                case "Link":
                                    /*
                                     * dialogoLeido.OriginConvoID = int.Parse(reader.GetAttribute("OriginConvoID")) -1;
                                     * dialogoLeido.DestinationConvoID = int.Parse(reader.GetAttribute("DestinationConvoID")) -1;
                                    */
                                    dialogoLeido.OriginDialogID = int.Parse(reader.GetAttribute("OriginDialogID")) -1;

                                    if (dialogoLeido.DestinationDialogIDs == "") //Es la primera vez que entra
                                    {
                                        dialogoLeido.DestinationDialogIDs += (int.Parse(reader.GetAttribute("DestinationDialogID")) - 1).ToString();
                                    }
                                    else //En las sucesivas añadimos un "_" al principio para separar los distintos índices de los destinos
                                    {
                                        dialogoLeido.DestinationDialogIDs += "_" + (int.Parse(reader.GetAttribute("DestinationDialogID")) - 1).ToString();
                                    }
                                    break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            switch (reader.Name)
                            {
                                case "Actor":
                                    actores.Add(new Actor(actorLeido.Id, actorLeido.Name));
                                    actorLeido.ResetActor();
                                    break;
                                case "Item":
                                    items.Add(new Item(itemLeido.Id, itemLeido.Name));
                                    itemLeido.ResetItem();
                                    break;
                                case "Location":
                                    localizaciones.Add(new Location(localizacionLeida.Id, localizacionLeida.Name));
                                    localizacionLeida.ResetLocation();
                                    break;
                                case "Conversation":
                                    conversaciones.Add(new Conversation(conversacionLeida.Id,
                                        conversacionLeida.Title,
                                        conversacionLeida.Dialogos));
                                    conversacionLeida.ResetConversation();
                                    break;
                                case "DialogEntry":
                                    conversacionLeida.Dialogos.Add(new Dialog(
                                        dialogoLeido.Id,
                                        dialogoLeido.Title,
                                        dialogoLeido.Actor,
                                        dialogoLeido.Conversant,
                                        dialogoLeido.MenuText,
                                        dialogoLeido.DialogText,
                                        dialogoLeido.Music,  
                                        dialogoLeido.OriginDialogID,
                                        dialogoLeido.DestinationDialogIDs,
                                        dialogoLeido.Interruptible,
                                        dialogoLeido.Interruption,
                                        dialogoLeido.Multispeech,
                                        dialogoLeido.ChangeOfMind,
                                        dialogoLeido.Silence,
                                        dialogoLeido.EsFinal
                                        ));
                                    dialogoLeido.ResetDialog();
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        #endregion

        #region Update Origin Index

        /*
         * Por algún motivo Chatmapper saca el campo OriginID con el valor de NodeID, por lo tanto no nos vale para saber el ID del nodo padre.
         * Actualizaremos el valor de OriginID de todos los nodos recorriendo todos los arrays.
         * TENER en cuenta que algunos nodos pueden tener varios padres, pero por el momento nos conformamos con actualizar el 1o
         */
        private static void UpdateOriginID()
        {
            foreach (Conversation c in conversaciones)
            {
                foreach (Dialog d in c.Dialogos)
                {
                    /*
                     * Chatmapper saca la primera línea de la conversación null
                     * PERO como ahora restamos 1 tenemos que añadir el >= en lugar de solo >
                     */
                    if (d.Id >= 0)
                    {
                        //If it is the first node of the conversation, I set OriginDialogID to -1
                        if (d.OriginDialogID == 0)
                        {
                            d.OriginDialogID = -1;
                        }
                        else 
                        {
                            //Has more than one destination node
                            if (d.DestinationDialogIDs.Contains("_"))
                            {
                                Console.WriteLine("Aqui: " + d.Id + " " + d.DestinationDialogIDs);
                                List<Dialog> NDestinos = ReturnDestinationDialogNodes(c, d.DestinationDialogIDs);

                                foreach (Dialog DDestino in NDestinos)
                                {
                                    DDestino.OriginDialogID = d.Id;
                                }
                            }
                            else if (!d.DestinationDialogIDs.Equals("")) //Only have one destination node
                            {
                                Dialog NDestino = c.findDialogNode(int.Parse(d.DestinationDialogIDs));
                                NDestino.OriginDialogID = d.Id;
                            }
                        }
                    }                   
                }
            }
        }                      

        #endregion

        #region Validations

        //Validamos los valores de algunos campos importantes del XML para buscar errores
        private static bool ValidateCSV()
        {
            bool HayErrores = false;
            var ErrorFile = new StringBuilder();

            foreach (Conversation c in conversaciones)
            {
                foreach (Dialog d in c.Dialogos)
                {
                    /*
                     * Chatmapper saca la primera línea de la conversación null
                     * PERO como ahora restamos 1 tenemos que añadir el >= en lugar de solo >
                     */
                    if (d.Id >= 0)
                    {
                        /* Validación que controla que no haya nodos que no vayan a ninguna parte. 
                         * Solo los nodos de final de conversación deben tener DestinationDialogIDs en blanco y tienen que tener marcado el bool EsFinal
                         */                        
                        if (d.DestinationDialogIDs.Equals(""))
                        {
                            if (!d.EsFinal)
                            {
                                HayErrores = true;
                                //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                int conv = c.Id + 1;
                                int dial = d.Id + 1;
                                //---
                                String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", no tiene nodos destino pero no esta marcado como nodo FINAL\r\n";
                                ErrorFile.Append(er);
                            }
                        }

                        /*
                         * Validación que comprueba las opciones de diálogo del jugador.
                         * Si contiene "_" es que son nodos de posibles respuestas del jugador.
                         * Tienen que ser mínimo 5 (4 + un silencio).
                         * Si tienen más de 5 es que hay change of mind
                         */
                        if (d.DestinationDialogIDs.Contains("_"))
                        {
                            int DestinationNodesCounter = ReturnDestinationNodesCount(d.DestinationDialogIDs);
                            if (DestinationNodesCounter < 5)
                            {
                                HayErrores = true;
                                //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                int conv = c.Id + 1;
                                int dial = d.Id + 1;
                                //---
                                String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", tiene menos de 5 nodos respuesta\r\n";
                                ErrorFile.Append(er);
                            }else
                            {
                                List<Dialog> DestinationNodes = ReturnDestinationDialogNodes(c, d.DestinationDialogIDs);
                                bool IsSilence = false;
                                bool IsMenuTextBlank = false;
                                int ChangeOfMindsCounter = 0;
                                bool ParentCouldBeInterrupt = false;
                                bool ChildCanInterrupt = false;

                                //Si el nodo padre (d) es interrumpible ponemos el bool ParentCouldBeInterrupt a true para hacer la validación posterior
                                if (d.Interruptible)
                                {
                                    ParentCouldBeInterrupt = true;
                                }

                                foreach (Dialog d2 in DestinationNodes)
                                {
                                    //Comprobamos que en los nodos destino hay uno que es SILENCIO
                                    if (d2.Silence)
                                    {
                                        IsSilence = true;
                                    }

                                    //Comprobamos que los nodos destino no tienen el MenuText en BLANCO
                                    if (d2.MenuText.Equals(""))
                                    {
                                        IsMenuTextBlank = true;
                                    }

                                    //Si DestinationNodesCounter es mayor de 5 es que hay Change of Minds
                                    if(DestinationNodesCounter > 5)
                                    {
                                        //Si es -1 son los nodos Change of Mind o Silencio
                                        if(d2.ChangeOfMind != -1)
                                        {
                                            ChangeOfMindsCounter++;
                                        }
                                    }

                                    //Si el nodo padre es interrumpible, alguno de sus nodos hijos tiene que tener marcado el IsInterruption
                                    if (ParentCouldBeInterrupt)
                                    {
                                        if (d2.Interruption)
                                        {
                                            ChildCanInterrupt = true;
                                        }
                                    }
                                }

                                //Si IsSilence no se ha puesto a TRUE damos error
                                if (!IsSilence)
                                {
                                    HayErrores = true;
                                    //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                    int conv = c.Id + 1;
                                    int dial = d.Id + 1;
                                    //---
                                    String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", en sus nodos destino no figura ninguno como SILENCIO, ¡fijo que te has olvidado de marcar el bool SILENCE!\r\n";
                                    ErrorFile.Append(er);
                                }

                                //Si IsMenuTextBlank se ha puesto a TRUE damos error
                                if (IsMenuTextBlank)
                                {
                                    HayErrores = true;
                                    //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                    int conv = c.Id + 1;
                                    int dial = d.Id + 1;
                                    //---
                                    String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", en sus nodos destino existe uno o varios con MenuText en BLANCO ¡Yo que te he dicho de dejarme cosas en blanco!\r\n";
                                    ErrorFile.Append(er);
                                }

                                /*
                                 * Tenemos 4 nodos respuesta más silencio (que para ChangeOfMind no cuenta), es decir que máximo podemos tener 4 nodos que cambien y para ellos se tiene que cumplir:
                                 * case 6: ChangeOfMindsCounter = 1
                                 * case 7: ChangeOfMindsCounter = 2
                                 * case 8: ChangeOfMindsCounter = 3
                                 * case 9: ChangeOfMindsCounter = 4
                                 */                                
                                switch (DestinationNodesCounter)
                                {                                    
                                    case 6:
                                        if (ChangeOfMindsCounter != 1)
                                        {
                                            HayErrores = true;
                                            //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                            int conv = c.Id + 1;
                                            int dial = d.Id + 1;
                                            //---
                                            String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", en sus nodos destino hay 1 Change Of Mind pero tienen errores\r\n";
                                            ErrorFile.Append(er);
                                        }
                                        break;
                                    case 7:
                                        if (ChangeOfMindsCounter != 2)
                                        {
                                            HayErrores = true;
                                            //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                            int conv = c.Id + 1;
                                            int dial = d.Id + 1;
                                            //---
                                            String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", en sus nodos destino hay 2 Change Of Mind pero tienen errores\r\n";
                                            ErrorFile.Append(er);
                                        }
                                        break;
                                    case 8:
                                        if (ChangeOfMindsCounter != 3)
                                        {
                                            HayErrores = true;
                                            //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                            int conv = c.Id + 1;
                                            int dial = d.Id + 1;
                                            //---
                                            String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", en sus nodos destino hay 3 Change Of Mind pero tienen errores\r\n";
                                            ErrorFile.Append(er);
                                        }
                                        break;
                                    case 9:
                                        if(ChangeOfMindsCounter != 4)
                                        {
                                            HayErrores = true;
                                            //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                            int conv = c.Id + 1;
                                            int dial = d.Id + 1;                                           
                                            //---
                                            String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", en sus nodos destino hay 4 Change Of Mind pero tienen errores\r\n";
                                            ErrorFile.Append(er);                                            
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                //Si el nodo padre podía ser interrumpido, alguno de sus hijos tiene que poder interrumpir, en caso contrario error
                                if (ParentCouldBeInterrupt)
                                {
                                    if (!ChildCanInterrupt)
                                    {
                                        HayErrores = true;
                                        //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                        int conv = c.Id + 1;
                                        int dial = d.Id + 1;
                                        //---
                                        String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", el nodo puede ser interrumpido sin embargo no tiene ningún hijo que pueda interrumpir\r\n";
                                        ErrorFile.Append(er);
                                    }
                                }


                            }
                        }

                        /* 
                         * Si el campo MultiSpeech esta marcado, tiene que existir el caracter "_" para separar líneas y viceversa
                         */
                        if (d.Multispeech)
                        {
                            if (!d.DialogText.Contains("_"))
                            {
                                HayErrores = true;
                                //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                int conv = c.Id + 1;
                                int dial = d.Id + 1;
                                //---
                                String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", tiene marcado MULTISPEECH pero luego no has puesto ninguna línea separada por _ \r\n";
                                ErrorFile.Append(er);
                            }
                        }else
                        {
                            if (d.DialogText.Contains("_"))
                            {
                                HayErrores = true;
                                //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                int conv = c.Id + 1;
                                int dial = d.Id + 1;
                                //---
                                String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", NO tiene marcado MULTISPEECH pero luego no has puesto líneas separadas por _ ¡marca el puto BOOL! \r\n";
                                ErrorFile.Append(er);
                            }
                        }

                        /*
                         * Si el nodo tiene marcado Interruption, su nodo origen tiene que poder ser interrumpido
                         */
                        if (d.Interruption)
                        {
                            Dialog DPadre = c.findDialogNode(d.OriginDialogID);

                            if (DPadre.Interruptible == false)
                            {
                                HayErrores = true;
                                //--- Sumamos 1 para que los resultados coincidan con Chatmapper por eso de que ese prog empieza a contar en 1
                                int conv = c.Id + 1;
                                int dial = d.Id + 1;
                                //---
                                String er = "Conversacion: " + conv.ToString() + " Nodo: " + dial.ToString() + ", el nodo es interrupción pero !SU PADRE NO SE PUEDE INTERRUMPIR! !COHERENCIA COÑO!\r\n";
                                ErrorFile.Append(er);
                            }
                        }
                    }
                }
            }

            if (HayErrores)
            {
                //Escribimos el fichero con el log de errores
                try
                {
                    File.WriteAllText("ErrorLOG.txt", ErrorFile.ToString());
                    MessageBox.Show("Fichero TXT ErrorLOG creado porque algo has hecho mal (O_O)", "ERROOOOOOORRRRRRR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    MessageBox.Show("No me jodas que ha dado ERROR al escribir el fichero de ERRORes, ¿otra vez abierto? ", "En fin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }            

            return HayErrores;
        }

        #endregion

        #region Write CSV File
        //Escribimos el fichero CSV
        private static void WriteCSV() {

            //Declaramos el fichero de salida CSV DT_DialogsChtM
            var csvChtM = new StringBuilder();
            //Declaramos el fichero de salida CSV DT_DialogsLang 
            var csvLang = new StringBuilder();

            //Ponemos las cabeceras del fichero DT_DialogsChtM
            var newLineChtM = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}{16}",
                "Name", "ConvID", "NodeID", "Actor", "Conversant",
                "OriginDialID", "DestinDialIDs",
                "Interruptible", "Interruption", "Multispeech", "ChangeOfMind", "Silence", "EsFinal",
                "MenuText", "Text", "Music",
                Environment.NewLine);
            csvChtM.Append(newLineChtM);

            //Ponemos las cabeceras del fichero DT_DialogsLang
            var newLineLang = string.Format("{0},{1},{2},{3}{4}",
                "Name",
                "MenuTextEN","TextEN","MusicEN",
                Environment.NewLine);
            csvLang.Append(newLineLang);

            foreach (Conversation c in conversaciones)
            {
                foreach (Dialog d in c.Dialogos)
                {
                    /*
                     * Chatmapper saca la primera línea de la conversación null
                     * PERO como ahora restamos 1 tenemos que añadir el >= en lugar de solo >
                     */
                        if (d.Id >= 0)
                    {
                        //Campos en los que hay que comprobar si existen , o " antes de escribir el archivo en CSV
                        string dMenuText = ReturnCleanTextForCSV(d.MenuText);
                        string dText = ReturnCleanTextForCSV(d.DialogText);

                        //Volcamos los campos en el fichero DT_DialogsChtM
                        newLineChtM = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}{16}",
                            c.Title + "_Conv_" + c.Id + "_Node_" + d.Id,                            
                            c.Id,
                            d.Id,
                            d.Actor,
                            d.Conversant,
                            d.OriginDialogID,
                            d.DestinationDialogIDs,
                            d.Interruptible,
                            d.Interruption,
                            d.Multispeech,
                            d.ChangeOfMind,
                            d.Silence,
                            d.EsFinal,
                            "\"" + dMenuText + "\"",
                            "\"" + dText + "\"",
                            d.Music,
                            Environment.NewLine);

                        //Volcamos el mismo ID en la columna Name y dejamos en blanco los campos de los demás idiomas para facilitar su localización más tarde
                        newLineLang = string.Format("{0},{1},{2},{3}{4}",
                            c.Title + "_Conv_" + c.Id + "_Node_" + d.Id,
                            "","","",
                            Environment.NewLine);

                        //Añadimos la nueva línea de los 2 ficheros a los strings que hay ahora mismo en memoría
                        csvChtM.Append(newLineChtM);
                        csvLang.Append(newLineLang);
                    }
                }
            }

            //Creamos el directorio Datatables, en caso de que ya exista no hará nada
            System.IO.Directory.CreateDirectory("DataTables");

            //Escribimos el fichero DT_DialogsChtM
            try
            {
                File.WriteAllText("DataTables/DT_DialogsChtM.csv", csvChtM.ToString());
                MessageBox.Show("Fichero CSV DT_DialogsChtM creado/actualizado con éxito", "Proceso finalizado",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Error al escribir el fichero CSV DT_DialogsChtM ¿No lo tendrás abierto, cabezón?", "Tengo que estar encima hasta cuando no estoy presente", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

            //Escribimos el fichero DT_DialogsLang con cuidado de que puede tener ya datos por lo que en vez de sobreescribir le añadimos un DATE
            try
            {
                string version;
                version = DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_";

                string fichero = "DataTables/" + version + "DT_DialogsLang.csv";

                File.WriteAllText(fichero, csvLang.ToString());
                MessageBox.Show("Fichero CSV DT_DialogsLang creado con éxito", "Proceso finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Error al escribir el fichero CSV DT_DialogsLang. Esto no debería pasar, mejor me llamas.", "Tengo que estar encima hasta cuando no estoy presente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        #endregion

        #region Functions

        //Devuelve una cadena de texto limpio de carácteres conflictivos para ficheros CSV         
        private static string ReturnCleanTextForCSV(string textToClean)
        {
            string CleanText = textToClean;

            if (textToClean.IndexOf(",") >= 0)
            {
                CleanText = CleanText.Replace("\"", "\"\"");
            }
            if (textToClean.IndexOf('"') >= 0)
            {
                CleanText = CleanText.Replace("\"", "\"");
            }

            return CleanText;
        }

        //Devuelve el número de nodos destino que tiene un nodo dado
        private static int ReturnDestinationNodesCount(string pDestinationNodes)
        {
            int CounterNodes = 0; //Contador de nodos destino

            string[] nodes = pDestinationNodes.Split('_');

            CounterNodes = nodes.Length;

            return CounterNodes;
        }

        //Devuelve una lista de dialogos correspondiente a los nodos destino
        private static List<Dialog> ReturnDestinationDialogNodes(Conversation pConv, string pDestinationNodes)
        {
            List<Dialog> DestinationNodes = new List<Dialog>();

            string[] nodes = pDestinationNodes.Split('_');
            
            foreach(string n in nodes)
            {
                DestinationNodes.Add(pConv.findDialogNode(int.Parse(n)));
            }            

            return DestinationNodes;
        }

        #endregion
        //----- END ------
    }
}
