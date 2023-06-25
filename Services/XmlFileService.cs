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
using Dateien_Sortierprogramm.Data;
using Microsoft.Win32;
using System.Diagnostics;
using Dateien_Sortierprogramm.ViewModels;

namespace Dateien_Sortierprogramm.Services
{
    public class XmlFileService
    {
        public static void CreateXmlFile(MainWindowViewModel vm)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*"; // Filter
            //saveFileDialog.FileName = ""; // Vorgabe-Dateiname
            bool? resultOfDialog = saveFileDialog.ShowDialog();
            //Wenn etwas im Dialog eingegeben wurde, dann wird bei dem Zustand true das Speichern ausgeführt
            if (resultOfDialog == true)
            {
                //Prüfung ob ViewModel null ist
                if (vm == null)
                {
                    return;
                    MessageBox.Show("ViewModel enthält keine Daten. Null Exception");
                }
                string fileName = saveFileDialog.FileName;



                XmlSerializer ser = new XmlSerializer(typeof(MainWindowViewModel));
                if (File.Exists(fileName)) //Prüfen, ob Datei existiert. Dazu muss der Speicherort geprüft werden
                {
                    File.Delete(fileName);
                }
                using (Stream s = File.OpenWrite(fileName))
                {
                    ser.Serialize(s, vm);
                }
            }
        }

        public static MainWindowViewModel LoadXmlFile(MainWindowViewModel vm)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*"; // Filter
            bool? isDialogResult = openFileDialog.ShowDialog();

            //Objekt wird erzeugt in dem die Daten von xml geladen werden
            //vm = new MainWindowViewModel();

            if (isDialogResult == true)
            {
                string fileName = openFileDialog.FileName;

                //Laden der Inhalte der Datei in Objekt
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(MainWindowViewModel));
                    using (Stream s = File.OpenRead(fileName))
                    {
                        //TODO: Fehlerabfrage, ob Objekt null ist, also Fehlerbehandlung verbessern
                        if (s == null)
                        {
                            MessageBox.Show("Datei konnte nicht geladen werden");
                        }
                        vm = ser.Deserialize(s) as MainWindowViewModel;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            return vm;
        }
    }
}
