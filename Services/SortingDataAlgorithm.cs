using Dateien_Sortierprogramm.Data;
using Dateien_Sortierprogramm.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Dateien_Sortierprogramm.Services
{
    public static class SortingDataAlgorithm
    {
        public static (List<SortingLogInfos>, string loggingInformation) StartSortingcService(MainWindowViewModel vm, List<string> fileformats)
        {
            string loggingInformation = "";
            int _countSortedFiles = 0;
            List<SortingLogInfos> _successfullSortedItemsLog = new List<SortingLogInfos>();
            List<string> allFilesFoundToSort = new List<string>();

            if (vm == null)
            {
                loggingInformation += "\nBitte eine Datei in den Zwischenspeicher Laden.";
                return (null, loggingInformation);
            }

            //Alle Dateien mit zu suchenden Dateiformaten 

            //Überprüfen, ob Quellordner korrekt sind
            var invalidSourceFolders = vm.lstSourceFolders
                .Where(folder => !Directory.Exists(folder.FolderPath))
                .ToList();

            // Wenn ungültige Quellordner gefunden wurden, eine Meldung anzeigen
            if (invalidSourceFolders.Any())
            {
                var message = string.Join(Environment.NewLine, invalidSourceFolders.Select(folder => folder.FolderPath));
                loggingInformation += "\nDie folgenden Quellordner sind ungültig:" + Environment.NewLine + message;
                return (null, loggingInformation);
            }

            allFilesFoundToSort = vm.lstSourceFolders
                .SelectMany(folder => Directory.GetFiles(folder.FolderPath, "*.*", SearchOption.TopDirectoryOnly))
                .Where(file => fileformats.Any(fileformat => file.EndsWith(fileformat)))
                .ToList();



            if (allFilesFoundToSort.Count() == 0)
            {
                loggingInformation += "\nDerzeit keine Dateien zum Sortieren vorhanden.";
                return (null, loggingInformation);
            }

            //In jedem Quellordner nach Dateien mit Schlüsselwörtern suchen und dann in TargetPathFolder verschieben
            //Wo?
            //Was?
            foreach (var lstOrderElement in vm.lstOrderElements)
            {
                //Für Welche Datei?
                foreach (string file in allFilesFoundToSort)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string filename = fileInfo.Name;

                    if (fileInfo.Name.Contains(lstOrderElement.SearchTerm))
                    {

                        //Datum hinzufügen wenn Datei noch keines hat
                        string pattern = @"(\d{4}(_|-)\d{2}(_|-)\d{2}(_|-)).*";
                        Regex regex = new Regex(pattern);
                        if (!regex.IsMatch(file))
                        {
                            filename = fileInfo.LastWriteTime.ToString("yyyy_MM_dd") + "_" + fileInfo.Name;
                            //Muss geprüft werden ob die Umbenennung mit Pfad funktioniert, sonst wird eine Exception spätter geworfen
                            if (!Directory.Exists(lstOrderElement.TargetFolderPath + filename))
                                loggingInformation += ("\nUmbenennung der Datei hat zu Fehler geführt." +
                                    "\nZielordnerpfad + Umbenannte Datei: \n" + lstOrderElement.TargetFolderPath + filename);
                        }

                        //TODO:
                        //Wenn eine identische Datei im Zielordner schon vorhanden ist, nachfrage

                        try
                        {
                            File.Move(file, lstOrderElement.TargetFolderPath + filename);
                            _successfullSortedItemsLog.Add(new SortingLogInfos()
                            {
                                File = filename,
                                FromFolder = fileInfo.DirectoryName,
                                ToFolder = lstOrderElement.TargetFolderPath
                            });
                            _countSortedFiles++;
                        }
                        catch (Exception ex)
                        {
                            loggingInformation = $"\n File: {file} \t TargetFolderPath: {lstOrderElement.TargetFolderPath} \t SearchTerm: {lstOrderElement.SearchTerm} ";
                            loggingInformation = "\nErrorMessage: ex.Message";
                        }
                    }
                }
            }

            if (_countSortedFiles < allFilesFoundToSort.Count())
            {
               loggingInformation += "\nMehrere Dateien aus den angegebenen Quellordnern sind noch nicht einsortiert worden, da es noch keinen passenden Suchbegriff gibt";
            }
            return (_successfullSortedItemsLog, loggingInformation);
        }

        //TODO: Diese Methode einbinden
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
