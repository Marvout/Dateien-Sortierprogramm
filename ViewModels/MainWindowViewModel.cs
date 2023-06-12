using Dateien_Sortierprogramm.Data;
using Dateien_Sortierprogramm.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPFBasics;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace Dateien_Sortierprogramm.ViewModels
{
    [Serializable]
    public class MainWindowViewModel : NotifyableBaseObject
    {
        public ObservableCollection<Folder> SourceFolders { get; set; } = new ObservableCollection<Folder>();
        public ObservableCollection<OrderElement> OrderElements { get; set; } = new ObservableCollection<OrderElement>();
        public ICommand SelectSourceFolderCommand { get; set; }
        public ICommand SelectTargetFolderCommand { get; set; }
        public ICommand CreateOrderElementCommand { get; set; }

        public Visibility IsVisible { get; set; } = Visibility.Visible;
        public MainWindowViewModel()
        {
            //DummyFolderPathService service = new DummyFolderPathService();
            //foreach (var folderpath in service.SomeFolderStrings())
            //{
            //    SourceFolders.Add(folderpath);
            //}
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
        }


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


        public void SelectSourceFolder()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    SourceFolders.Add(new Folder() { FolderPath = selectedPath + "\\" });
                    this.IsVisible = Visibility.Collapsed;
                }

            }
        }

        public void SelectTargetFolder()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                   this.TargetFolderUI= selectedPath;
                }

            }
        }

        public void AddingOrderElementsToGrid()
        {
            if(!String.IsNullOrWhiteSpace(SearchTermUI) && !String.IsNullOrWhiteSpace(TargetFolderUI))
            {
                OrderElements.Add(new OrderElement() { SearchTerm = SearchTermUI, TargetFolderPath = TargetFolderUI });
            }
            SearchTermUI = string.Empty;
            TargetFolderUI = string.Empty;
        }



    }
}
