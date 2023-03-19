using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dateien_Sortierprogramm.Model;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Navigation;

namespace Dateien_Sortierprogramm.ViewModel
{
    public class SortingAlgorithm
    {
        public static void MoveFileTo(XMLData xmlData, bool isTaxStructureTrue)
        {
            //Zählt mit wie viele Dateien sortiert wurden. Wenn diese Zahl null ist wird zurückgegeben, dass es derzeit keine
            //zu sortierenden Dateien gibt.
            int _countSortedFiles = 0;

            //Wird zuerst geprüft, ob Steuer Ordnerstruktur für aktuelles Jahr existiert
            if (isTaxStructureTrue)
                CreateTaxFolderStructure(xmlData);

            if (xmlData == null)
            {
                MessageBox.Show("Bitte eine Datei in den Zwischenspeicher Laden.");
                return;
            }

            //Erstelle neue Liste mit allen PDF Dateinamen in Input Folder!
            //Auch andere Dateien einbinden wie excel oder Txt oder vlt auch Word oder Open Office dateien
            foreach (SortingData sourceFolder in xmlData.SourceFolder)
            {
                //Beliebige Dateitypen suchen lassen 
                //OPTIONAL: User selber ermöglichen welche Dateitypen gesucht werden sollen
                List<string> allFileNames = new List<string>();
                List<string> fileTypes = new List<string>() { "*.pdf", "*.csv" };
                for (int i = 0; i < fileTypes.Count(); i++)
                {
                    string[] filenames = Directory.GetFiles(sourceFolder.Quellordner, fileTypes[i]);
                    foreach (string filename in filenames)
                    {
                        allFileNames.Add(filename);
                    }
                }

                //Doppelte Dateien löschen. Mit Bestätigung, als Schutz vor falschem Löschen.
                DeleteEqualFiles(ref allFileNames, "(1)");

                for (int i = 0; i < xmlData.ExecuteList.Count; i++)
                {

                    string targetterm = xmlData.ExecuteList[i].Suchwort;
                    string targetpath = xmlData.ExecuteList[i].Zielordner;

                    //Schleife für fileEntries (Anzahl an Dateien in Liste)
                    foreach (string filename in allFileNames)
                    {
                        FileInfo filenameshort = new FileInfo(filename);
                        //Schleife für Begriffe

                        //Wenn Dateiname ein Schlüsselwort enthält, dann wird es in den Ordner verschoben
                        /*TODO: Für Steuer noch Jahr beachten dass Zielordner mit jedem Jahreswechsel sich ändern 
                         * => Nutzer sollte hier nicht seber Jahres-Zielordner vorgeben, da er ja auch die Ordnerstruktur 
                         * automatisch erstellen lässt*/
                        if (filenameshort.Name.ToLower().Contains(targetterm.ToLower()))
                        {
                            _countSortedFiles++;
                            if (sourceFolder.Quellordner.ToLower().Contains(("Downloads").ToLower()))
                            {
                                string fileRenamed = SortingDownloadFiles(filenameshort);
                                File.Move(filename, targetpath + fileRenamed);
                                //FIX:? Weil eine Datei für mehrere Schlüsselwörter passen könnte, muss der Dateiname nachdem dieser zugeordnet wird
                                //aus der Liste genommen werden, sonst wird er ggf. nochmal gesucht aber nicht mehr gefunden.
                                MessageBox.Show(fileRenamed + "\n =>  " + targetpath);
                                //allFileNames.Remove(filename);
                            }
                            else
                            {
                                try
                                {
                                    File.Move(filename, targetpath + filenameshort.Name);
                                    //FIX:? Weil eine Datei für mehrere Schlüsselwörter passen könnte, muss der Dateiname nachdem dieser zugeordnet wird
                                    //aus der Liste genommen werden, sonst wird er ggf. nochmal gesucht aber nicht mehr gefunden.
                                    MessageBox.Show(filenameshort.Name + "\n=====>  " + targetpath);
                                    //allFileNames.Remove(filename);
                                }
                                catch(Exception e)
                                {
                                    MessageBox.Show(e.Message);
                                }
                            }
                        }
                    }
                }
            }
            if (_countSortedFiles == 0)
                MessageBox.Show("Derzeit keine Dateien zum Sortieren in den gewählten Quellordnern vorhanden.");
        }
        private static void CreateTaxFolderStructure(XMLData xmlData)
        {
            //TODO: Dateien soll anhand von Jahreszahl im Dateinamen einsortiert werden
            //TODO: Ganze Ordnerstruktur anlegbar machen und nicht hardcoden
            //IDEE: Idee, dass nur Sonderkosten, Einkommen und Werbungskosten als Ordner da sind, und nur die Dateien dann sagen was genau es ist
            //OPTIONAL: Am Ende des Jahres alles automatisch mit PDF24 API oder so als eine PDF pro Kategorie zusammenfassen

            //Zuerst wird überprüft, ob Aktuelles Jahr Ordner existiert, wenn nicht dann Ordner erstellen
            //string currentYearFolder = fileCreator.TaxFolder + @"\" + DateTime.Now.Year + 

            string currentYearFolder = xmlData.OrdnerFuerSteuerstruktur + DateTime.Now.ToString("yyyy") + @"\";
            if (!Directory.Exists(currentYearFolder))
            {
                string[] folderLevel1CostType = new string[] {
                    @"01 Sonderkosten\", @"02 Einkommen\", @"03 Werbungskosten\" };
                string[] folderLevel2Sonderkosten = new string[] {
                    @"01 Gesundheits- I Krankheitskosten\", @"02 Spenden\" };
                string[] folderLevel2Werbungskosten = new string[] {
                    @"01 Arbeitsmittel\", @"02 Fortbildung\", @"03 Reisekosten\", @"04 Häusliches Arbeitszimmer \",@"05 Werbungskosten in Sonderfällen\", @"06 Steuerberatung\"};

                // CreatingTaxFolders(currentYearFolder, folderLevel1CostType, folderLevel2Sonderkosten, folderLevel2Werbungskosten);
                Directory.CreateDirectory(currentYearFolder);
                foreach (string folder1 in folderLevel1CostType)
                {
                    Directory.CreateDirectory(currentYearFolder + folder1);
                }

                foreach (string folder2Sonder in folderLevel2Sonderkosten)
                {
                    Directory.CreateDirectory(currentYearFolder + folderLevel1CostType[0] + folder2Sonder);
                }

                foreach (string folder2Werbung in folderLevel2Werbungskosten)
                {
                    Directory.CreateDirectory(currentYearFolder + folderLevel1CostType[2] + folder2Werbung);
                }

                MessageBox.Show("Neue Steuer Ordnerstruktur für das aktuelle Jahr wurde angelegt.");
            }
        }

        //TODO: Strings Nicht im Code belassen, sondern Nutzereingaben ermöglichen
        private static string SortingDownloadFiles(FileInfo filename)
        {
            string oldFilename = filename.Name;
            string newFilename = "";
            if (oldFilename.ToLower().Contains(("Kontoauszug").ToLower()))
                newFilename = DateTime.Now.ToString("yyyy_MM_dd") + "_" + oldFilename;
            else if (oldFilename.ToLower().Contains(("Umsatzanzeige").ToLower()))
                newFilename = filename.LastWriteTime.ToString("yyyy_MM_dd") + "_" + "ING_Girokonto_Umsatz" + ".csv";
            else if (oldFilename.ToLower().Contains(("Direkt_Depot").ToLower()))
                newFilename = filename.LastWriteTime.ToString("yyyy_MM_dd") + "_" + oldFilename;
            else if (oldFilename.ToLower().Contains(("Anleitung").ToLower()))
                newFilename = oldFilename;
            if (oldFilename.ToLower().Contains(("KfW_Kontoauszug").ToLower()))
                newFilename = DateTime.Now.AddMonths(-1).ToString("yyyy_MM") + "_" + "KFW_Kontoauszug";
            else
                newFilename = filename.LastWriteTime.ToString("yyyy_MM_dd") + "_" + oldFilename;
            return newFilename;
        }

        private static void DeleteEqualFiles(ref List<string> listToClean, string searchString)
        {
            foreach (string file in listToClean)
            {
                if (file.Contains("(1)"))
                {
                    MessageBoxResult dialogResult = MessageBox.Show(" Da es sich um eine doppelte Datei handeln könnte: Soll die Datei: " + file + " gelöscht werden?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        File.Delete(file);
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {
                        MessageBox.Show("Bitte Datei umbenenen, damit die Datei einsortiert werden kann. Dann den Startvorgang bitte wiederholen.");
                        //TODO: Hier noch Code schreiben, der dafür sorgt, dass Datei umbenannt wird oder so, sodass sie dann für den
                        //Sortiervorgang berücksichtigt wird
                        // File.Move(file, targetpath + filenameshort.Name);
                    }
                }
            }

            //Wenn eine Datei mit einer (1) nicht gelöscht werden soll wird sie trotzdem in der Liste hier gelöscht und somit nicht mehr sortiert
            listToClean.RemoveAll(item => item.Contains(searchString));
        }
    }
}
