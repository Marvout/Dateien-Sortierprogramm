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
using Dateien_Sortierprogramm.UIs;

namespace Dateien_Sortierprogramm.ViewModels
{
    [Serializable]
    public partial class MainWindowViewModel : NotifyableBaseObject
    {
        //TODO: Wenn ich manuelle Ordnerpfad in Zielordner Textbox eingebe, dann wird häufig das \ vergessen am Schluss
        //TODO: Reihenfolge Anpassbar für OrderElements DataGrid, da sich dadurch vlt die Sortierreihenfolge ergibt?

        //Zu speichernde Daten
        public ObservableCollection<Folder> lstSourceFolders { get; set; } = new ObservableCollection<Folder>();
        public ObservableCollection<OrderElements> lstOrderElements { get; set; } = new ObservableCollection<OrderElements>();

        private ObservableCollection<LogInfos> _lstLogInfos = new ObservableCollection<LogInfos>();
        public ObservableCollection<LogInfos> LstLogInfos
        {
            get => _lstLogInfos;
            set
            {
                if (value != null)
                {
                    _lstLogInfos = value;
                    RaisePropertyChanged(nameof(LstLogInfos));
                }
            }
        }    
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
        public ICommand CloseLogWindowCommand { get; set; }
        [XmlIgnore]
        public Visibility IsVisible { get; set; } = Visibility.Visible;

        public event EventHandler Cancel;
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
                DummyLogInfoService serviceLogInfos = new DummyLogInfoService();
                foreach (var logInfo in serviceLogInfos.SomeLogInfos())
                {
                    LstLogInfos.Add(logInfo);
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

            //Checkboxen Status laden
            #region Checkboxen Laden
            CheckBox_AVI = vm.CheckBox_AVI;
            CheckBox_BMP = vm.CheckBox_BMP;
            CheckBox_CSV = vm.CheckBox_CSV;
            CheckBox_Excel = vm.CheckBox_Excel;
            CheckBox_GIF = vm.CheckBox_GIF;
            CheckBox_JSON = vm.CheckBox_JSON;
            CheckBox_JPG = vm.CheckBox_JPG;
            CheckBox_MKV = vm.CheckBox_MKV;
            CheckBox_MOV = vm.CheckBox_MOV;
            CheckBox_MP3 = vm.CheckBox_MP3;
            CheckBox_MP4 = vm.CheckBox_MP4;
            CheckBox_PDF = vm.CheckBox_PDF;
            CheckBox_PNG = vm.CheckBox_PNG;
            CheckBox_Powerpoint = vm.CheckBox_Powerpoint;
            CheckBox_RAR = vm.CheckBox_RAR;
            CheckBox_Text = vm.CheckBox_Text;
            CheckBox_WAV = vm.CheckBox_WAV;
            CheckBox_WMV = vm.CheckBox_WMV;
            CheckBox_Word = vm.CheckBox_Word;
            CheckBox_XML = vm.CheckBox_XML;
            CheckBox_ZIP = vm.CheckBox_ZIP;
            #endregion
            //TODO: Auf Jahreszahlen prüfen
            //Prüfen ob Zielordnerpfade mit Jahresangabe für neues Jahr geupdated sollen, grade beim Steuerordner
            //vm = SortingAlgorithm.ChangeAllYearRelevantDirectionsToCurrentYear(xmlDataCollector);

            //TODO: x e für Löschen von Einträgen erstellen
        }

        private void StartSorting()
        {
            //DummyLogInfoService serviceLogInfos = new DummyLogInfoService();
            //foreach (var logInfo in serviceLogInfos.SomeLogInfos())
            //{
            //    lstLogInfo.Add(logInfo);
            //}
            SortingDataAlgorithm sortingDataAlgorithm = new SortingDataAlgorithm();
            foreach (var logInfo in sortingDataAlgorithm.StartSortingcService(this, lstFileFormats))
            {
                LstLogInfos.Add(logInfo);
            }
            LogView logView = new LogView(this);
            logView.Show();
        }
    }
}
