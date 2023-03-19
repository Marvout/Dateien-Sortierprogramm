using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Xps;
using Microsoft.VisualBasic;
using Dateien_Sortierprogramm.Model;
using Microsoft.Win32;
using System.Diagnostics;


namespace Dateien_Sortierprogramm.ViewModel
{
    public class XMLSerializer
    {
        public static void CreateXMLFile(XMLData xmlData)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*"; // Filter
            saveFileDialog.FileName = "MeineDatei"; // Vorgabe-Dateiname
            Nullable<bool> resultOfDialog = saveFileDialog.ShowDialog();
            //Wenn etwas im Dialog eingegeben wurde, dann wird bei dem Zustand true das Speichern ausgeführt
            if (resultOfDialog == true)
            {
                string fileName = saveFileDialog.FileName;

                //Wenn das Objekt nicht gefüllt ist, dann wird ein leeres neues erstellt
                if (xmlData == null)
                {
                    xmlData = new XMLData();
                    xmlData.SourceFolder.Add(new SortingData()
                    {
                        Quellordner = @"\Users\hmarv\Desktop\Ablage\"
                    });
                    xmlData.ExecuteList.Add(new SortingData()
                    {
                        Suchwort = "Test1",
                        Zielordner = @"C:\Users\hmarv\Desktop\Ablage\"
                    });
                    xmlData.ExecuteList.Add(new SortingData()
                    {
                        Suchwort = "Test2",
                        Zielordner = @"C:\Users\hmarv\Desktop\Ablage\"
                    });
                    xmlData.ExecuteList.Add(new SortingData()
                    {
                        Suchwort = "Test3",
                        Zielordner = @"C:\Users\hmarv\Desktop\Ablage\"
                    });
                }

                XmlSerializer ser = new XmlSerializer(typeof(XMLData));
                if (File.Exists(fileName)) //Prüfen, ob Datei existiert. Dazu muss da Speicherort geprüft werden
                {
                    File.Delete(fileName);
                }
                using (Stream s = File.OpenWrite(fileName))
                {
                    ser.Serialize(s, xmlData);
                }
                MessageBox.Show("Datei erfolgreich gespeichert.");
            }
        }

        public static XMLData LoadXMLFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*"; // Filter
            Nullable<bool> isDialogResult = openFileDialog.ShowDialog();

            XMLData xmldataload = new XMLData();

            if (isDialogResult == true)
            {
                string fileName = openFileDialog.FileName;

                //Laden der Inhalte der Datei in Objekt
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(XMLData));
                    using (Stream s = File.OpenRead(fileName))
                    {
                        //TODO: Fehlerabfrage, ob Objekt null ist, also Fehlerbehandlung verbessern
                        xmldataload = ser.Deserialize(s) as XMLData;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return xmldataload;

        }

        public static string AddFolderDirectory()
        {
            var dialog = new OpenFileDialog
            {
                FileName = "Ordner auswählen",
                Filter = "Ordner |*.none",
                CheckFileExists = false,
                CheckPathExists = true,
                RestoreDirectory = true,
                DereferenceLinks = true
            };

            if (dialog.ShowDialog() == true)
            {
                return Path.GetDirectoryName(dialog.FileName) + @"\";
            }
            else
            {
                MessageBox.Show("Kein Ordner wurde ausgewählt");
                return string.Empty;
            }
            //if (ViewModel.Validation.PathValidation(txt_sourceFolder.Text))
            //{
            //    xmlDataCollector.SourceFolder.Add(txt_sourceFolder.Text);
            //    MessageBox.Show("Quellordner erfolgreich hinzugefügt.");
            //}
        }
    }
}
