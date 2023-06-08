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
                //MessageBox.Show("Hello World");
               
            });
        }


        public void TestHelloMessageBox()
        {
            MessageBox.Show("Hello World");
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Ordner auswählen",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Ordner auswählen",
                Filter = "Ordner |*."
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedPath = Path.GetDirectoryName(dialog.FileName);

                // Fügen Sie den ausgewählten Pfad zur SourceFolders-Liste hinzu
                SourceFolders.Add(new Folder() {FolderPath = selectedPath });
            }
        }
        
    }
}
