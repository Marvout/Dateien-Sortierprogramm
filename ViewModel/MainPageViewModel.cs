using Dateien_Sortierprogramm.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;

namespace Dateien_Sortierprogramm.ViewModel
{
    //public class MainPageViewModel : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private string _inputText;
    //    public string InputText
    //    {
    //        get { return _inputText; }
    //        set
    //        {
    //            _inputText = value;
    //            OnPropertyChanged(nameof(InputText));
    //        }
    //    }
    //    public ObservableCollection<SortingData> SourceFolders { get; set; } = new ObservableCollection<SortingData>();

    //    private ICommand _addSourceFolderCommand;
    //    public ICommand AddSourceFolderCommand
    //    {
    //        get
    //        {
    //            if (_addSourceFolderCommand == null)
    //            {
    //                _addSourceFolderCommand = new RelayCommand(
    //                     param => this.CanAddSourceFolder(),
    //                     param => this.AddSourceFolder()
    //                 );
    //            }
    //            return _addSourceFolderCommand;
    //        }
    //    }

    //    private bool CanAddSourceFolder()
    //    {
    //        return !string.IsNullOrEmpty(InputText);
    //    }

    //    private void AddSourceFolder()
    //    {
    //        SourceFolders.Add(new SortingData { Quellordner = InputText });
    //        InputText = string.Empty;
    //    }

    //    private void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    public static List<SortingData> fileContensOfDataGrid { get; set; } = new List<SortingData>();
    //}

}

