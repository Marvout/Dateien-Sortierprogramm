using Dateien_Sortierprogramm.Data;
using Dateien_Sortierprogramm.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dateien_Sortierprogramm.Services
{
    public class SortingDataAlgorithm
    {
        public IEnumerable<LogInfos> StartSortingcService(MainWindowViewModel vm, List<string> fileformats)
        {
            //Zählt mit, wie viele Dateien sortiert wurden. Wenn diese Zahl null ist, wird zurückgegeben, dass es derzeit keine
            //zu sortierenden Dateien gibt.
            int _countSortedFiles = 0;
            List<LogInfos> logInfos = new List<LogInfos>();

            if (vm == null)
            {
                MessageBox.Show("Bitte eine Datei in den Zwischenspeicher Laden.");
                return null;
            }


            //Alle Dateien mit zu suchenden Dateiformaten 
            foreach (Folder sourceFolder in vm.lstSourceFolders)
            {
                if (!Directory.Exists(sourceFolder.FolderPath))
                {
                    MessageBox.Show("Der Dateipfad des Quellordners " + sourceFolder.FolderPath +
                        "ist entweder veraltet oder nicht richtig.");
                }
                List<string> allFileNames = new List<string>();

                //Auflistung aller Dateien mit gesuchten Dateiformaten in gewählten Quellordner
                for (int i = 0; i < fileformats.Count(); i++)
                {
                    string fileformat = fileformats[i];
                    string[] filenames = Directory.GetFiles(sourceFolder.FolderPath, "*" + fileformat);

                    foreach (string filename in filenames)
                    {
                        allFileNames.Add(filename);
                    }
                }

                //Doppelte Dateien löschen. Mit Bestätigung, als Schutz vor falschem Löschen.
                // DeleteEqualFiles(ref allFileNames, "(1)");

                //In jedem Quellordner nach Dateien mit Schlüsselwörtern suchen und dann in TargetPathFolder verschieben
                for (int i = 0; i < vm.lstOrderElements.Count; i++)
                {
                    string targetterm = vm.lstOrderElements[i].SearchTerm;
                    string targetpath = vm.lstOrderElements[i].TargetFolderPath;
                    //Schleife für fileEntries (Anzahl an Dateien in Liste)
                    foreach (string filename in allFileNames)
                    {
                        FileInfo filenameshort = new FileInfo(filename);
                        //Wenn Dateiname ein Schlüsselwort enthält, dann wird es in den Ordner verschoben
                        if (filenameshort.Name.ToLower().Contains(targetterm.ToLower()))
                        {
                            _countSortedFiles++;
                            string errorMessage = "";
                            //Wenn Download Order durchsucht wird, dann ist es häufig so, dass Dateien
                            //umbenannt werden müssen, da sie noch nichtssagend sind, zb von Bank etc.
                            if (sourceFolder.FolderPath.ToLower().Contains(("Downloads").ToLower()))
                            {
                                string fileRenamed = filenameshort.LastWriteTime.ToString("yyyy_MM_dd") + "_" + filenameshort;
                                if (!Directory.Exists(targetpath + fileRenamed))
                                    errorMessage += ("Der neue Dateipfad mit Umbenennung der Datei ist fehlerhaft." +
                                        "\nZielordnerpfad + Umbenannte Datei: " + targetpath + fileRenamed);
                            }
                            try
                            {
                                File.Move(filename, targetpath + filenameshort.Name);
                                logInfos.Add(new LogInfos()
                                {
                                    File = filenameshort.ToString(),
                                    FromFolder = sourceFolder.FolderPath,
                                    ToFolder = targetpath
                                });
                                MessageBox.Show(filenameshort.Name + "\n=====>  " + targetpath);
                            }
                            catch
                            {

                                if (!Directory.Exists(filename))
                                    errorMessage += "Es scheint etwas nicht mit dem Dateinamen zu stimmen.\n" +
                                        "Dateiname: " + filename + "\n";
                                if (!Directory.Exists(targetpath))
                                    errorMessage += "Der Zielordnerpfad " + targetpath + "ist entweder veraltet, es ist keine Verbindung damit hergestellt (Netzwerk) oder er ist nicht richtig.\n";

                                MessageBox.Show(errorMessage);
                            }
                        }
                    }
                }
            }
            if (_countSortedFiles == 0)
                MessageBox.Show("Derzeit keine Dateien zum Sortieren in den gewählten Quellordnern vorhanden. " +
                    "\n\nHinweise: " +
                    "\n\nMöglicherweise haben Sie noch keine Quellordner hinzugefügt." +
                    "\n\nMöglicherweise wurden noch keine Dateiformate oder noch nicht die passenden Formate ausgewählt." +
                    "\n\nGegebenfalls sind Dateien vorhanden, aber noch kein Suchbegriff der in den Dateinamen vorkommt. Fügen " +
                    "Sie dann einfach weitere Suchbegriffe, die in dem Dateinamen stecken, mit Zielordnern hinzu. Starten Sie " +
                    "den Suchvorgang anschließend erneut.");
            return logInfos;
        }


        //TODO: Hard coded strings wie "Kontoauszug" Nicht im Code belassen, sondern Nutzereingaben ermöglichen
        //private static string SortingDownloadFiles(FileInfo filename)
        //{
        //    string oldFilename = filename.Name;
        //    string newFilename = "";
        //    if (oldFilename.ToLower().Contains(("Kontoauszug").ToLower()))
        //        newFilename = DateTime.Now.ToString("yyyy_MM_dd") + "_" + oldFilename;
        //    else if (oldFilename.ToLower().Contains(("Umsatzanzeige").ToLower()))
        //        newFilename = filename.LastWriteTime.ToString("yyyy_MM_dd") + "_" + "ING_Girokonto_Umsatz" + ".csv";
        //    else if (oldFilename.ToLower().Contains(("Direkt_Depot").ToLower()))
        //        newFilename = filename.LastWriteTime.ToString("yyyy_MM_dd") + "_" + oldFilename;
        //    else if (oldFilename.ToLower().Contains(("Anleitung").ToLower()))
        //        newFilename = oldFilename;
        //    if (oldFilename.ToLower().Contains(("KfW_Kontoauszug").ToLower()))
        //        newFilename = DateTime.Now.AddMonths(-1).ToString("yyyy_MM") + "_" + "KFW_Kontoauszug";
        //    else
        //        newFilename = filename.LastWriteTime.ToString("yyyy_MM_dd") + "_" + oldFilename;
        //    return newFilename;
        //}

        //private static void DeleteEqualFiles(ref List<string> listToClean, string searchString)
        //{
        //    foreach (string file in listToClean)
        //    {
        //        //TODO: Es kann auch ein Datum sein, und dann wird der Hinweis ausgelöst, obwohl es keine doppelte Datei ist
        //        if (file.Contains("(1)"))
        //        {
        //            MessageBoxResult dialogResult = MessageBox.Show(" Da es sich um eine doppelte Datei handeln könnte: Soll die Datei: " + file + " gelöscht werden?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (dialogResult == MessageBoxResult.Yes)
        //            {
        //                File.Delete(file);
        //            }
        //            else if (dialogResult == MessageBoxResult.No)
        //            {
        //                MessageBox.Show("Bitte Datei umbenenen, damit die Datei einsortiert werden kann. Dann den Startvorgang bitte wiederholen.");
        //            }
        //        }
        //    }

        //    listToClean.RemoveAll(item => item.Contains(searchString));
        //}

        public static MainWindowViewModel ChangeAllYearRelevantDirectionsToCurrentYear(MainWindowViewModel vm)
        {
            string _currentYear = Convert.ToString(DateTime.Now.Year);
            string _previousYear = Convert.ToString(DateTime.Now.Year - 1);

            for (int i = 0; i < vm.lstOrderElements.Count; i++)
            {
                //TODO: Noch einbauen, dass wenn Ordner nicht aktualisiert werden soll, dass dieser nicht bei jedem
                //Laden erneut abgefragt wird. 
                if (vm.lstOrderElements[i].TargetFolderPath.Contains(_previousYear))
                {
                    MessageBoxResult _messageBoxResult = MessageBox.Show("Soll der Orderpfad: \n" + vm.lstOrderElements[i].TargetFolderPath + "   , " +
                        "\nin dem das vorherige Jahr " + _previousYear + " Bestandteil" +
                        " des Pfades ist, geändert werden auf das aktuelle Jahr " + _currentYear + " ?", "Prüfen", MessageBoxButton.YesNo);

                    if (_messageBoxResult == MessageBoxResult.Yes)
                    {
                        string newpathstring = vm.lstOrderElements[i].TargetFolderPath.Replace(_previousYear, _currentYear);
                        vm.lstOrderElements[i].TargetFolderPath = newpathstring;
                    }
                }
            }
            return vm;
        }
    }
}
