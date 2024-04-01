using Dateien_Sortierprogramm.Data;
using Dateien_Sortierprogramm.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Dateien_Sortierprogramm.Services
{
    public static class SortingDataAlgorithm
    {
        public static List<SortingLogInfos> StartSortingcService(MainWindowViewModel vm, List<string> fileformats)
        {
            string loggingInformation = "";
            int _countSortedFiles = 0;
            List<SortingLogInfos> _successfullSortedItemsLog = new List<SortingLogInfos>();
            List<string> allFilesFoundToSort = new List<string>();

            if (vm == null)
            {
                loggingInformation += "Bitte eine Datei in den Zwischenspeicher Laden.";
                MessageBox.Show(loggingInformation);
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
                loggingInformation += "Die folgenden Quellordner sind ungültig:" + Environment.NewLine + message;
                MessageBox.Show(loggingInformation);
                return null;
            }

            allFilesFoundToSort = vm.lstSourceFolders
                .SelectMany(folder => Directory.GetFiles(folder.FolderPath, "*.*", SearchOption.TopDirectoryOnly))
                .Where(file => fileformats.Any(fileformat => file.EndsWith(fileformat)))
                .ToList();



            if (allFilesFoundToSort.Count() == 0)
            {
                loggingInformation += "Derzeit keine Dateien zum Sortieren vorhanden.";
                MessageBox.Show(loggingInformation);
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
                                    "Zielordnerpfad + Umbenannte Datei: \n" + lstOrderElement.TargetFolderPath + filename);
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
                            MessageBox.Show(loggingInformation);
                        }
                    }
                }
            }

            if (_countSortedFiles < allFilesFoundToSort.Count())
            {
                loggingInformation += "Mehrere Dateien aus den angegebenen Quellordnern sind noch nicht einsortiert worden, da es noch keinen passenden Suchbegriff gibt";
                MessageBox.Show(loggingInformation);
            }
            return _successfullSortedItemsLog;
        }

        public static (List<SortingLogInfos>, string loggingInformation) StartSortingcConsoleService(SortingInformation sortingInformation)
        {
            string loggingInformation = "";
            int _countSortedFiles = 0;
            List<string> fileformats = sortingInformation.LstFileFormats;
            List<SortingLogInfos> _successfullSortedItemsLog = new List<SortingLogInfos>();
            List<string> allFilesFoundToSort = new List<string>();

            if (sortingInformation == null)
            {
                loggingInformation += "Bitte eine Datei in den Zwischenspeicher Laden.";
                return (null, loggingInformation);
            }

            //Alle Dateien mit zu suchenden Dateiformaten 

            //Überprüfen, ob Quellordner korrekt sind
            var invalidSourceFolders = sortingInformation.LstSourceFolders
                .Where(folder => !Directory.Exists(folder.FolderPath))
                .ToList();

            // Wenn ungültige Quellordner gefunden wurden, eine Meldung anzeigen
            if (invalidSourceFolders.Any())
            {
                var message = string.Join(Environment.NewLine, invalidSourceFolders.Select(folder => folder.FolderPath));
                loggingInformation += "Die folgenden Quellordner sind ungültig:" + Environment.NewLine + message;
                return (null, loggingInformation);
            }

            allFilesFoundToSort = sortingInformation.LstSourceFolders
                .SelectMany(folder => Directory.GetFiles(folder.FolderPath, "*.*", SearchOption.TopDirectoryOnly))
                .Where(file => fileformats.Any(fileformat => file.EndsWith(fileformat)))
                .ToList();



            if (allFilesFoundToSort.Count() == 0)
            {
                loggingInformation += "Derzeit keine Dateien zum Sortieren vorhanden.";
                return (null, loggingInformation);
            }

            //In jedem Quellordner nach Dateien mit Schlüsselwörtern suchen und dann in TargetPathFolder verschieben
            //Wo?
            //Was?
            foreach (var lstOrderElement in sortingInformation.LstOrderElements)
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
                            loggingInformation = $"\nErrorMessage: {ex.Message}";
                        }
                    }
                }
            }

            if (_countSortedFiles < allFilesFoundToSort.Count())
            {
                loggingInformation += "Mehrere Dateien aus den angegebenen Quellordnern sind noch nicht einsortiert worden, da es noch keinen passenden Suchbegriff gibt";
            }
            return (_successfullSortedItemsLog, loggingInformation);
        }


        //TODO: Diese Methode einbinden
        public static MainWindowViewModel ChangeDirectoryToCurrentYear(MainWindowViewModel vm)
        {
            string _currentYear = Convert.ToString(DateTime.Now.Year);
            string _previousYear = Convert.ToString(DateTime.Now.Year - 1);

            foreach (var orderElement in vm.lstOrderElements)
            {
                string loggingInformation = "";
                string directoryPattern = $"(.*\\\\){_previousYear}\\\\";
                try
                {
                    Regex regex = new Regex(directoryPattern);
                    if (regex.IsMatch(orderElement.TargetFolderPath))
                    {
                        loggingInformation += $"Jahreszahl im Pfad \n{orderElement.TargetFolderPath}\n wurde aktualisiert auf dieses Jahr.";
                        string newpathstring = orderElement.TargetFolderPath.Replace(_previousYear, _currentYear);
                        if (!Directory.Exists(newpathstring))
                        {
                            loggingInformation += $"\n Zusätzlich wurde ein neuer Order mit der aktuellen Jahreszahl erstellt, in der zukünftig die passenden Dokumente einsortiert werden.";
                            string newFolderForCurrentYear = $@"{regex.Match(orderElement.TargetFolderPath).Groups[1].Value}{_currentYear}\";
                            Directory.CreateDirectory(newFolderForCurrentYear);
                        }
                        orderElement.TargetFolderPath = newpathstring;
                        MessageBox.Show(loggingInformation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return vm;
        }

        public static string ChangeDirectoryToCurrentYearConsoleService(SortingInformation sortingInformation)
        {
            string _currentYear = Convert.ToString(DateTime.Now.Year);
            string _previousYear = Convert.ToString(DateTime.Now.Year - 1);
            string loggingInformation = "";

            foreach (var orderElement in sortingInformation.LstOrderElements)
            {
                string directoryPattern = $"(.*\\\\){_previousYear}\\\\";
                try
                {
                    Regex regex = new Regex(directoryPattern);
                    if (regex.IsMatch(orderElement.TargetFolderPath))
                    {
                        loggingInformation += "-------";
                        loggingInformation += $"\nJahreszahl im Pfad geupdated: \n{orderElement.TargetFolderPath}\n";
                        string newpathstring = orderElement.TargetFolderPath.Replace(_previousYear, _currentYear);
                        if (!Directory.Exists(newpathstring))
                        {
                            loggingInformation += $"\n Zusätzlich wurde ein neuer Order mit der aktuellen Jahreszahl erstellt, in der zukünftig die passenden Dokumente einsortiert werden.";
                            string newFolderForCurrentYear = $@"{regex.Match(orderElement.TargetFolderPath).Groups[1].Value}{_currentYear}\";
                            Directory.CreateDirectory(newFolderForCurrentYear);
                        }
                        orderElement.TargetFolderPath = newpathstring;
                        loggingInformation += "\n-------";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return loggingInformation;
        }

    }
}
