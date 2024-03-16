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
        //TODO: Reihenfolge Anpassbar für OrderElements DataGrid, da sich dadurch vlt die Sortierreihenfolge ergibt?

        //Zu speichernde Daten
        //private string _sortingFilePath = "";
        //public string SortingFilePath
        //{
        //    get => _sortingFilePath;
        //    set
        //    {
        //        if (_sortingFilePath != value)
        //        {
        //            _sortingFilePath = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}
        public ObservableCollection<Folder> lstSourceFolders { get; set; } = new ObservableCollection<Folder>();
        public ObservableCollection<OrderElements> lstOrderElements { get; set; } = new ObservableCollection<OrderElements>();

        // private ObservableCollection<LogInfos> _lstLogInfos = new ObservableCollection<LogInfos>();
        public ObservableCollection<SortingLogInfos> LstLogInfos { get; set; } = new ObservableCollection<SortingLogInfos>();
        //{
        //    get => _lstLogInfos;
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _lstLogInfos = value;
        //            RaisePropertyChanged(nameof(LstLogInfos));
        //        }
        //    }
        //}
        //Daten die nicht gespeichert werden können/müssen
        #region Commands
        [XmlIgnore]
        public ICommand SelectSourceFolderCommand { get; set; }
        [XmlIgnore]
        public ICommand SelectTargetFolderCommand { get; set; }

        [XmlIgnore]
        public ICommand SelectSortingFilePathCommand { get; set; }

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
        public ICommand DeleteDataGridRowFolderCommand { get; set; }
        [XmlIgnore]
        public ICommand DeleteDataGridRowOrderElementsCommand { get; set; }
        #endregion

        //[XmlIgnore]
        //public Visibility IsVisible { get; set; } = Visibility.Visible;

        public event EventHandler Cancel;

        //Konstruktor
        public MainWindowViewModel()
        {

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
            this.DeleteDataGridRowFolderCommand = new DelegateCommand((o) =>
            {
                DeleteDataGridRowFolder();
            });
            this.DeleteDataGridRowOrderElementsCommand = new DelegateCommand((o) =>
            {
                DeleteDataGridRowOrderElements();
            });
            //this.SelectSortingFilePathCommand = new DelegateCommand((o) =>
            //{
            //    SelectSortingFilePath();
            //});
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

        private Folder _selectedItemFolder;
        public Folder SelectedItemFolder
        {
            get => _selectedItemFolder;
            set
            {
                if (_selectedItemFolder != value)
                {
                    _selectedItemFolder = value;
                    RaisePropertyChanged();
                }
            }
        }

        private OrderElements _selectedItemOrderElements;
        public OrderElements SelectedItemOrderElements
        {
            get => _selectedItemOrderElements;
            set
            {
                if (value != _selectedItemOrderElements)
                {
                    _selectedItemOrderElements = value;
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
                    lstSourceFolders.Add(new Folder() { FolderPath = selectedPath + @"\" });
                }

            }
        }

        //private void SelectSortingFilePath()
        //{
        //    var dialog = new CommonOpenFileDialog();
        //    dialog.IsFolderPicker = true;
        //    CommonFileDialogResult result = dialog.ShowDialog();
        //    if (result == CommonFileDialogResult.Ok)
        //    {
        //        string selectedPath = dialog.FileName;
        //        if (!string.IsNullOrEmpty(selectedPath))
        //        {
        //            SortingFilePath = selectedPath + @"\";
        //        }

        //    }
        //}

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
            SortingInformation sortingInformationData = new SortingInformation();
            //Listen
            sortingInformationData.LstSourceFolders = this.lstSourceFolders;
            sortingInformationData.LstFileFormats = this.lstFileFormats;
            sortingInformationData.LstOrderElements = this.lstOrderElements;
            //Checkboxen
            sortingInformationData.CheckBox_All = this.checkBox_All;
            sortingInformationData.CheckBox_PDF = this.checkBox_PDF;
            sortingInformationData.CheckBox_Excel = this.checkBox_Excel;
            sortingInformationData.CheckBox_Word = this.checkBox_Word;
            sortingInformationData.CheckBox_Powerpoint = this.checkBox_Powerpoint;
            sortingInformationData.CheckBox_Text = this.checkBox_Text;
            sortingInformationData.CheckBox_CSV = this.checkBox_CSV;
            sortingInformationData.CheckBox_ZIP = this.checkBox_ZIP;
            sortingInformationData.CheckBox_JPG = this.checkBox_JPG;
            sortingInformationData.CheckBox_PNG = this.checkBox_PNG;
            sortingInformationData.CheckBox_GIF = this.checkBox_GIF;
            sortingInformationData.CheckBox_BMP = this.checkBox_BMP;
            sortingInformationData.CheckBox_MP3 = this.checkBox_MP3;
            sortingInformationData.CheckBox_MP4 = this.checkBox_MP4;
            sortingInformationData.CheckBox_WAV = this.checkBox_WAV;
            sortingInformationData.CheckBox_AVI = this.checkBox_AVI;
            sortingInformationData.CheckBox_MOV = this.checkBox_MOV;
            sortingInformationData.CheckBox_MKV = this.checkBox_MKV;
            sortingInformationData.CheckBox_WMV = this.checkBox_WMV;
            sortingInformationData.CheckBox_RAR = this.checkBox_RAR;
            sortingInformationData.CheckBox_XML = this.checkBox_XML;
            sortingInformationData.CheckBox_JSON = this.checkBox_JSON;

            XmlFileService.CreateXmlFile(sortingInformationData);
        }

        private void LoadData()
        {
            SortingInformation sortingInformationData = XmlFileService.LoadXmlFile();
            if (sortingInformationData == null)
            {
                MessageBox.Show("Datei enthält keine Daten oder falsche Datei wurde ausgewählt.");
                return;
            }
            //Elemente in ViewModel laden. Dazu muss, wenn noch Elemente in den ObservableCollections, die Objekte leer sein
            //Dann wird über eine foreach-Schleife das Objekt wieder befüllt.
            this.lstOrderElements.Clear(); //Vorherige Elemente entfernen
            foreach (var orderelement in sortingInformationData.LstOrderElements)
            {
                this.lstOrderElements.Add(orderelement);
            }
            this.lstSourceFolders.Clear();
            foreach (var orderelements in sortingInformationData.LstSourceFolders)
            {
                this.lstSourceFolders.Add(orderelements);
            }
            //this.SortingFilePath = string.Empty;
            //this.SortingFilePath = vm.SortingFilePath ?? string.Empty;

            //Checkboxen Status laden
            #region Checkboxen Laden
            this.CheckBox_AVI = sortingInformationData.CheckBox_AVI;
            this.CheckBox_BMP = sortingInformationData.CheckBox_BMP;
            this.CheckBox_CSV = sortingInformationData.CheckBox_CSV;
            this.CheckBox_Excel = sortingInformationData.CheckBox_Excel;
            this.CheckBox_GIF = sortingInformationData.CheckBox_GIF;
            this.CheckBox_JSON = sortingInformationData.CheckBox_JSON;
            this.CheckBox_JPG = sortingInformationData.CheckBox_JPG;
            this.CheckBox_MKV = sortingInformationData.CheckBox_MKV;
            this.CheckBox_MOV = sortingInformationData.CheckBox_MOV;
            this.CheckBox_MP3 = sortingInformationData.CheckBox_MP3;
            this.CheckBox_MP4 = sortingInformationData.CheckBox_MP4;
            this.CheckBox_PDF = sortingInformationData.CheckBox_PDF;
            this.CheckBox_PNG = sortingInformationData.CheckBox_PNG;
            this.CheckBox_Powerpoint = sortingInformationData.CheckBox_Powerpoint;
            this.CheckBox_RAR = sortingInformationData.CheckBox_RAR;
            this.CheckBox_Text = sortingInformationData.CheckBox_Text;
            this.CheckBox_WAV = sortingInformationData.CheckBox_WAV;
            this.CheckBox_WMV = sortingInformationData.CheckBox_WMV;
            this.CheckBox_Word = sortingInformationData.CheckBox_Word;
            this.CheckBox_XML = sortingInformationData.CheckBox_XML;
            this.CheckBox_ZIP = sortingInformationData.CheckBox_ZIP;
            #endregion
            //TODO: Auf Jahreszahlen prüfen
            //Prüfen ob Zielordnerpfade mit Jahresangabe für neues Jahr geupdated sollen, grade beim Steuerordner
            //vm = SortingAlgorithm.ChangeAllYearRelevantDirectionsToCurrentYear(xmlDataCollector);
        }

        private void StartSorting()
        {
            List<SortingLogInfos> logInfos = new List<SortingLogInfos>();
            LstLogInfos.Clear();
            logInfos = SortingDataAlgorithm.StartSortingcService(this, lstFileFormats);
            if (logInfos != null && logInfos.Count() != 0)
            {
                foreach (var logInfo in logInfos)
                {
                    LstLogInfos.Add(logInfo);
                }
                LogView logView = new LogView(this);
                logView.Show();
            }
        }

        private void DeleteDataGridRowFolder()
        {
            if (null != SelectedItemFolder)
            {
                lstSourceFolders.Remove(SelectedItemFolder);
            }
        }

        private void DeleteDataGridRowOrderElements()
        {
            if (null != SelectedItemOrderElements)
            {
                lstOrderElements.Remove(SelectedItemOrderElements);
            }
        }
    }
}
