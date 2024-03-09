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
    public class SortingDataAlgorithm
    {
        public IEnumerable<LogInfos> StartSortingcService(MainWindowViewModel vm, List<string> fileformats)
        {
            int _countSortedFiles = 0;
            List<LogInfos> _logInfos = new List<LogInfos>();
            List<string> allFilesFoundToSort = new List<string>();

            if (vm == null)
            {
                MessageBox.Show("Bitte eine Datei in den Zwischenspeicher Laden.");
                return null;
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
                MessageBox.Show("Die folgenden Quellordner sind ungültig:" + Environment.NewLine + message);
                return null;
            }

            allFilesFoundToSort = vm.lstSourceFolders
                .SelectMany(folder => Directory.GetFiles(folder.FolderPath, "*.*", SearchOption.TopDirectoryOnly))
                .Where(file => fileformats.Any(fileformat => file.EndsWith(fileformat)))
                .ToList();



            if (allFilesFoundToSort.Count() == 0)
            {
                MessageBox.Show("Keine Dateien zum Sortieren gefunden." +
                    "\n\nHinweise: " +
                    "\n\n-Möglicherweise sind grade keine Dateien in den Quellordnern vorhanden." +
                    "\n\n-Möglicherweise haben Sie noch keine Quellordner hinzugefügt." +
                    "\n\n-Möglicherweise wurden noch keine Dateiformate oder noch nicht die passenden Formate ausgewählt." +
                    "\n\n-Gegebenfalls sind Dateien vorhanden, aber noch kein Suchbegriff der in den Dateinamen vorkommt. Fügen " +
                    "Sie dann einfach weitere Suchbegriffe, die in dem Dateinamen stecken, mit Zielordnern hinzu. " +
                    "\n\n-Starten Sie den Suchvorgang anschließend erneut.");
                return null;
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
                    string filename = "";

                    if (fileInfo.Name.Contains(lstOrderElement.SearchTerm))
                    {
                        string errorMessage = "";

                        //Datum hinzufügen wenn Datei noch keines hat
                        string pattern = @"(\d{4}(_|-)\d{2}(_|-)\d{2}(_|-)).*";
                        Regex regex = new Regex(pattern);
                        if (!regex.IsMatch(file))
                        {
                            filename = fileInfo.LastWriteTime.ToString("yyyy_MM_dd") + "_" + fileInfo.Name;
                            //Muss geprüft werden ob die Umbenennung mit Pfad funktioniert, sonst wird eine Exception spätter geworfen
                            if (!Directory.Exists(lstOrderElement.TargetFolderPath + filename))
                                errorMessage += ("Umbenennung der Datei hat zu Fehler geführt." +
                                    "\nZielordnerpfad + Umbenannte Datei: \n" + lstOrderElement.TargetFolderPath + filename);
                        }

                        //TODO:
                        //Wenn eine identische Datei im Zielordner schon vorhanden ist, nachfrage

                        try
                        {
                            File.Move(file, lstOrderElement.TargetFolderPath + filename);
                            _logInfos.Add(new LogInfos()
                            {
                                File = filename,
                                FromFolder = fileInfo.DirectoryName,
                                ToFolder = lstOrderElement.TargetFolderPath
                            });
                            _countSortedFiles++;
                        }
                        catch (Exception ex)
                        {
                            if (!Directory.Exists(lstOrderElement.TargetFolderPath))
                            {
                                errorMessage += "Der Zielordnerpfad " + lstOrderElement.TargetFolderPath + "ist entweder veraltet, es ist keine Verbindung damit hergestellt (Netzwerk) oder er ist nicht richtig.\n\n";
                            }
                            else
                            {
                                errorMessage = ex.Message;
                            }
                            MessageBox.Show(errorMessage);
                        }
                    }
                }
            }

            if (_countSortedFiles < allFilesFoundToSort.Count())
            {
                MessageBox.Show("Mehrere Dateien aus den angegebenen Quellordnern sind noch nicht einsortiert worden, da es noch keinen passenden Suchbegriff gibt");
            }
            return _logInfos;
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
