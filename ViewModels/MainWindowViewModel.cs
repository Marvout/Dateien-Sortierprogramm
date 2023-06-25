﻿using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using Dateien_Sortierprogramm.Data;
using Dateien_Sortierprogramm.Services;
using WPFBasics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Dateien_Sortierprogramm.ViewModels
{
    [Serializable]
    public class MainWindowViewModel : NotifyableBaseObject
    {
        //TODO: Wenn ich manuelle Ordnerpfad in Zielordner Textbox eingebe, dann wird häufig das \ vergessen am Schluss
        //TODO: Reihenfolge Anpassbar für OrderElements DataGrid, da sich dadurch vlt die Sortierreihenfolge ergibt?

        //Zu speichernde Daten
        public ObservableCollection<Folder> lstSourceFolders { get; set; } = new ObservableCollection<Folder>();
        public ObservableCollection<OrderElements> lstOrderElements { get; set; } = new ObservableCollection<OrderElements>();

        //Daten die nicht gespeichert werden können/müssen
        [XmlIgnore]
        public ICommand SelectSourceFolderCommand { get; set; }
        [XmlIgnore]
        public ICommand SelectTargetFolderCommand { get; set; }
        [XmlIgnore]
        public ICommand CreateOrderElementCommand { get; set; }
        [XmlIgnore]
        public ICommand SaveDataCommand { get; set; }
        [XmlIgnore]
        public ICommand LoadDataCommand { get; set; }
        [XmlIgnore]
        public ICommand StartSortingServiceCommand { get; set; }
        [XmlIgnore]
        public Visibility IsVisible { get; set; } = Visibility.Visible;
        public MainWindowViewModel()
        {
            //DummyServices
            bool isDummyServices = false;
            if (isDummyServices)
            {
                DummyFolderPathService serviceFolderPaths = new DummyFolderPathService();
                foreach (var folderpath in serviceFolderPaths.SomeFolderStrings())
                {
                    lstSourceFolders.Add(folderpath);
                }
                DummyOrderElementsService serviceOrderElements = new DummyOrderElementsService();
                foreach (var orderelement in serviceOrderElements.SomeOrderElementsStrings())
                {
                    lstOrderElements.Add(orderelement);
                }
            }



            //Delegate Initialisierung
            this.SelectSourceFolderCommand = new DelegateCommand((o) =>
            {
                SelectSourceFolder();

            });
            this.SelectTargetFolderCommand = new DelegateCommand((o) =>
            {
                SelectTargetFolder();
            });

            this.CreateOrderElementCommand = new DelegateCommand((o) =>
            {
                AddingOrderElementsToGrid();
            });
            this.SaveDataCommand = new DelegateCommand((o) =>
            {
                SaveData();
            });
            this.LoadDataCommand = new DelegateCommand((o) =>
            {
                LoadData();
            });
            this.StartSortingServiceCommand = new DelegateCommand((o) =>
            {
                StartSorting();
            });
        }



        //Properties
        private string _searchTermUI;
        public string SearchTermUI
        {
            get => _searchTermUI;
            set
            {
                if (_searchTermUI != value)
                {
                    _searchTermUI = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _targetFolderUI;
        public string TargetFolderUI
        {
            get => _targetFolderUI;
            set
            {
                if (_targetFolderUI != value)
                {
                    _targetFolderUI = value;
                    RaisePropertyChanged();
                }
            }
        }


        //public Methods
        private void SelectSourceFolder()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    lstSourceFolders.Add(new Folder() { FolderPath = selectedPath + "\\" });
                    this.IsVisible = Visibility.Collapsed;
                }

            }
        }

        private void SelectTargetFolder()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    //Hier wird nur in Property geschrieben und nicht in Liste,da
                    //der Ordnerpfad nur ein Teil von den OrderElements ist
                    //Erste wenn Nutzer Hinzufügen drückt, werden beide Elemente
                    //in die Liste lstOrderElements geschrieben
                    this.TargetFolderUI = selectedPath + "\\";
                }

            }
        }

        private void AddingOrderElementsToGrid()
        {
            if (!String.IsNullOrWhiteSpace(SearchTermUI) && !String.IsNullOrWhiteSpace(TargetFolderUI))
            {
                lstOrderElements.Add(new OrderElements() { SearchTerm = SearchTermUI, TargetFolderPath = TargetFolderUI });
            }
            SearchTermUI = string.Empty;
            TargetFolderUI = string.Empty;
        }


        private void SaveData()
        {
            XmlFileService.CreateXmlFile(this);
        }

        private void LoadData()
        {
            MainWindowViewModel vm = XmlFileService.LoadXmlFile(this);
            if (vm == null)
            {
                MessageBox.Show("Datei enthält keine Daten oder falsche Datei wurde ausgewählt.");
                return;
            }
            //Elemente in ViewModel laden. Dazu muss, wenn noch Elemente in den ObservableCollections, die Objekte leer sein
            //Dann wird über eine foreach-Schleife das Objekt wieder befüllt.
            this.lstOrderElements.Clear(); //Vorherige Elemente entfernen
            foreach (var orderelement in vm.lstOrderElements)
            {
                this.lstOrderElements.Add(orderelement);
            }
            this.lstSourceFolders.Clear();
            foreach (var orderelements in vm.lstSourceFolders)
            {
                this.lstSourceFolders.Add(orderelements);
            }


            //TODO: Auf Jahreszahlen prüfen
            //Prüfen ob Zielordnerpfade mit Jahresangabe für neues Jahr geupdated sollen, grade beim Steuerordner
            //vm = SortingAlgorithm.ChangeAllYearRelevantDirectionsToCurrentYear(xmlDataCollector);
        }

        private void StartSorting()
        {
            SortingDataAlgorithm.StartSortingcService(this);
        }

        //Von ChatGPT erstellte Daten
        //private void SearchFiles()
        //{
        //    FileSearchService fileSearchService = new FileSearchService();
        //    List<string> fileFormats = fileSearchService.GetSelectedFileFormats(checkboxItems);
        //    List<string> foundFiles = fileSearchService.SearchFiles(searchPath, fileFormats);
        //    // Weitere Logik zur Verarbeitung der gefundenen Dateien...
        //}




        //von ChatGPT erstellter Code für Checkboxen
        //using System;
        //using System.Collections.Generic;
        //using System.IO;
        //using System.Linq;

        //namespace FileSearchExample
        //    {
        //        class Program
        //        {
        //            static void Main(string[] args)
        //            {
        //                // Passe den Pfad an, in dem du nach den Dateien suchen möchtest
        //                string searchPath = @"C:\Path\To\Search";

        //                // Erstelle eine Liste von Checkboxen
        //                List<CheckboxItem> checkboxItems = new List<CheckboxItem>
        //            {
        //                new CheckboxItem { Checkbox = checkBox1, FileFormat = ".txt" },
        //                new CheckboxItem { Checkbox = checkBox2, FileFormat = ".pdf" },
        //                // Weitere Checkboxen hinzufügen
        //            };

        //                // Erstelle eine Liste für die ausgewählten Dateiformate
        //                List<string> fileFormats = checkboxItems
        //                    .Where(item => item.Checkbox.Checked)
        //                    .Select(item => item.FileFormat)
        //                    .ToList();

        //                // Suche nach Dateien im angegebenen Pfad mit den ausgewählten Dateiformaten
        //                var foundFiles = Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
        //                    .Where(file => fileFormats.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase));

        //                // Gebe die gefundenen Dateien aus
        //                foreach (var file in foundFiles)
        //                {
        //                    Console.WriteLine(file);
        //                }

        //                Console.ReadLine();
        //            }
        //        }

        //        // Hilfsklasse für die Checkboxen und Dateiformate
        //        class CheckboxItem
        //        {
        //            public CheckBox Checkbox { get; set; }
        //            public string FileFormat { get; set; }
        //        }
        //    }


    }
}
