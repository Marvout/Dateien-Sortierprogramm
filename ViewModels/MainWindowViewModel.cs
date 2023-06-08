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



namespace Dateien_Sortierprogramm.ViewModels
{
    [Serializable]
    public class MainWindowViewModel : NotifyableBaseObject
    {
        public ObservableCollection<Folder> SourceFolders { get; set; } = new ObservableCollection<Folder>();
        public ICommand AddSourceFolderCommand { get; set; }
        public ICommand TestHelloCommand { get; set; }
        public MainWindowViewModel()
        {
            //DummyFolderPathService service = new DummyFolderPathService();
            //foreach (var folderpath in service.SomeFolderStrings())
            //{
            //    SourceFolders.Add(folderpath);
            //}
            this.TestHelloCommand = new DelegateCommand((o) =>
            {
                TestHelloMessageBox();

            });
        }


        public void TestHelloMessageBox()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    SourceFolders.Add(new Folder() { FolderPath = selectedPath });
                }

            }
        }

    }
}
