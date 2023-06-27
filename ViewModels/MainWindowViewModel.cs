using System;
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
using System.Windows.Controls;
using System.Runtime.CompilerServices;

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

        //Checkboxen
        private List<string> lstFileFormats = new List<string>();
        private bool checkBox_PDF;

        public bool CheckBox_PDF
        {
            get => checkBox_PDF;
            set
            {
                if (checkBox_PDF != value)
                {
                    checkBox_PDF = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_PDF, ".pdf");
                   
                }
            }
        }
        private bool checkBox_Excel;
        public bool CheckBox_Excel
        {
            get => checkBox_Excel;
            set
            {
                if (checkBox_Excel != value)
                {
                    checkBox_Excel = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_Excel, ".xls");
                    CheckboxChanged(checkBox_Excel, ".xlsx");
                   
                }
            }
        }

        private bool checkBox_Word;
        public bool CheckBox_Word
        {
            get => checkBox_Word;
            set
            {
                if (checkBox_Word != value)
                {
                    checkBox_Word = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_Word, ".doc");
                    CheckboxChanged(checkBox_Word, ".docx");

                }
            }
        }

        private bool checkBox_Powerpoint;
        public bool CheckBox_Powerpoint
        {
            get => checkBox_Powerpoint;
            set
            {
                if (checkBox_Powerpoint != value)
                {
                    checkBox_Powerpoint = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_Powerpoint, ".ppt");
                    CheckboxChanged(checkBox_Powerpoint, ".pptx");
                }
            }
        }

        private bool checkBox_Text;
        public bool CheckBox_Text
        {
            get => checkBox_Text;
            set
            {
                if (checkBox_Text != value)
                {
                    checkBox_Text = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_Text, ".txt");
                }
            }
        }

        private bool checkBox_CSV;
        public bool CheckBox_CSV
        {
            get => checkBox_CSV;
            set
            {
                if (checkBox_CSV != value)
                {
                    checkBox_CSV = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_CSV, ".csv");
                }
            }
        }

        private bool checkBox_ZIP;
        public bool CheckBox_ZIP
        {
            get => checkBox_ZIP;
            set
            {
                if (checkBox_ZIP != value)
                {
                    checkBox_ZIP = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_ZIP, ".zip");
                }
            }
        }

        private bool checkBox_JPG;
        public bool CheckBox_JPG
        {
            get => checkBox_JPG;
            set
            {
                if (checkBox_JPG != value)
                {
                    checkBox_JPG = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_JPG, ".jpg");
                    CheckboxChanged(checkBox_JPG, ".jpeg");
                }
            }
        }

        private bool checkBox_PNG;
        public bool CheckBox_PNG
        {
            get => checkBox_PNG;
            set
            {
                if (checkBox_PNG != value)
                {
                    checkBox_PNG = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_PNG, ".png");
                }
            }
        }

        private bool checkBox_GIF;
        public bool CheckBox_GIF
        {
            get => checkBox_GIF;
            set
            {
                if (checkBox_GIF != value)
                {
                    checkBox_GIF = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_GIF, ".gif");
                }
            }
        }

        private bool checkBox_BMP;
        public bool CheckBox_BMP
        {
            get => checkBox_BMP;
            set
            {
                if (checkBox_BMP != value)
                {
                    checkBox_BMP = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_BMP, ".bmp");
                }
            }
        }

        private bool checkBox_MP3;
        public bool CheckBox_MP3
        {
            get => checkBox_MP3;
            set
            {
                if (checkBox_MP3 != value)
                {
                    checkBox_MP3 = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_MP3, ".mp3");
                }
            }
        }

        private bool checkBox_MP4;
        public bool CheckBox_MP4
        {
            get => checkBox_MP4;
            set
            {
                if (checkBox_MP4 != value)
                {
                    checkBox_MP4 = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_MP4, ".mp4");
                }
            }
        }

        private bool checkBox_WAV;
        public bool CheckBox_WAV
        {
            get => checkBox_WAV;
            set
            {
                if (checkBox_WAV != value)
                {
                    checkBox_WAV = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_WAV, ".wav");
                }
            }
        }

        private bool checkBox_AVI;
        public bool CheckBox_AVI
        {
            get => checkBox_AVI;
            set
            {
                if (checkBox_AVI != value)
                {
                    checkBox_AVI = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_AVI, ".avi");
                }
            }
        }

        private bool checkBox_MOV;
        public bool CheckBox_MOV
        {
            get => checkBox_MOV;
            set
            {
                if (checkBox_MOV != value)
                {
                    checkBox_MOV = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_MOV, ".mov");
                }
            }
        }

        private bool checkBox_MKV;
        public bool CheckBox_MKV
        {
            get => checkBox_MKV;
            set
            {
                if (checkBox_MKV != value)
                {
                    checkBox_MKV = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_MKV, ".mkv");
                }
            }
        }

        private bool checkBox_WMV;
        public bool CheckBox_WMV
        {
            get => checkBox_WMV;
            set
            {
                if (checkBox_WMV != value)
                {
                    checkBox_WMV = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_WMV, ".wmv");
                }
            }
        }

        private bool checkBox_RAR;
        public bool CheckBox_RAR
        {
            get => checkBox_RAR;
            set
            {
                if (checkBox_RAR != value)
                {
                    checkBox_RAR = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_RAR, ".rar");
                }
            }
        }

        private bool checkBox_XML;
        public bool CheckBox_XML
        {
            get => checkBox_XML;
            set
            {
                if (checkBox_XML != value)
                {
                    checkBox_XML = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_XML, ".xml");
                }
            }
        }

        private bool checkBox_JSON;
        public bool CheckBox_JSON
        {
            get => checkBox_JSON;
            set
            {
                if (checkBox_JSON != value)
                {
                    checkBox_JSON = value;
                    RaisePropertyChanged();
                    CheckboxChanged(checkBox_JSON, ".json");
                }
            }
        }
        private void CheckboxChanged(bool? isChecked, string fileformat)
        {
            if (isChecked == true)
            {
                lstFileFormats.Add(fileformat);
            }
            if (isChecked == false)
            {
                lstFileFormats.Remove(fileformat);
            }
        }

       

        //private Methods


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
            SortingDataAlgorithm.StartSortingcService(this, lstFileFormats);
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
