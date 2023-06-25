﻿using Dateien_Sortierprogramm.Data;
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
        public static void StartSortingcService(MainWindowViewModel vm)
        {
            //Zählt mit wie viele Dateien sortiert wurden. Wenn diese Zahl null ist wird zurückgegeben, dass es derzeit keine
            //zu sortierenden Dateien gibt.
            int _countSortedFiles = 0;


            if (vm == null)
            {
                MessageBox.Show("Bitte eine Datei in den Zwischenspeicher Laden.");
                return;
            }

            //TODO: Bestimmung von Liste, nach welchen Dateiformaten gesucht werden soll
            List<string> fileTypes = new List<string>() { "*.pdf", "*.csv" };

            //Erstelle neue Liste mit allen PDF Dateinamen in Input Folder!
            //Auch andere Dateien einbinden wie excel oder Txt oder vlt auch Word oder Open Office dateien
            foreach (Folder sourceFolder in vm.lstSourceFolders)
            {
                //Beliebige Dateitypen suchen lassen 
                //OPTIONAL: User selber ermöglichen welche Dateitypen gesucht werden sollen
                List<string> allFileNames = new List<string>();
               
                //Auflistung aller Dateien mit gesuchten Dateiformaten in gewählten Quellordner
                for (int i = 0; i < fileTypes.Count(); i++)
                {
                    string[] filenames = Directory.GetFiles(sourceFolder.FolderPath, fileTypes[i]);
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
                    string targetterm =vm.lstOrderElements[i].SearchTerm;
                    string targetpath = vm.lstOrderElements[i].TargetFolderPath;
                    //Schleife für fileEntries (Anzahl an Dateien in Liste)
                    foreach (string filename in allFileNames)
                    {
                        FileInfo filenameshort = new FileInfo(filename);
                        //Wenn Dateiname ein Schlüsselwort enthält, dann wird es in den Ordner verschoben
                        if (filenameshort.Name.ToLower().Contains(targetterm.ToLower()))
                        {
                            _countSortedFiles++;
                            //Wenn Download Order durchsucht wird, dann ist es häufig so, dass Dateien
                            //umbenannt werden müssen, da sie noch nichtssagend sind, zb von Bank etc.
                            if (sourceFolder.FolderPath.ToLower().Contains(("Downloads").ToLower()))
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
                                   //Aber ist das nicht egal, da nach dem ersten passenden Suchwort die Datei einsortiert wird ?
                                   
                                    //TODO: Logdatei erstellen oder anzeigen, da jedes Mal klicken nervt
                                    MessageBox.Show(filenameshort.Name + "\n=====>  " + targetpath);
                                    //allFileNames.Remove(filename);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.Message);
                                }
                            }
                        }
                    }
                }
            }
            if (_countSortedFiles == 0)
                MessageBox.Show("Derzeit keine Dateien zum Sortieren in den gewählten Quellordnern vorhanden. " +
                    "\n\nHinweise: " +
                    "\n\nMöglicherweise haben Sie noch keine Quellordner hinzugefügt." +
                    "\n\nGegebenfalls sind Dateien vorhanden, aber noch kein Suchbegriff der in den Dateinamen vorkommt. Fügen " +
                    "Sie dann einfach weitere Suchbegriffe, die in dem Dateinamen stecken, mit Zielordnern hinzu. Starten Sie " +
                    "den Suchvorgang anschließend erneut.");
        }
        

        //TODO: Hard coded strings wie "Kontoauszug" Nicht im Code belassen, sondern Nutzereingaben ermöglichen
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
