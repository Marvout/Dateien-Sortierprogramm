using Dateien_Sortierprogramm.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBasics;

namespace Dateien_Sortierprogramm.ViewModels
{
    [Serializable]
    public class MainWindowViewModel : NotifyableBaseObject
    {
        public ObservableCollection<OrderElements> ocSortingEntriesFromGrid { get; set; } = new ObservableCollection<OrderElements>();
        public ObservableCollection<string> ocSourceFolders { get; set; } = new ObservableCollection<string>();

        public ICommand AddSource { get; set; }
        public MainWindowViewModel()
        {
            this.AddSource = new DelegateCommand((o) =>
                {
                    this.ocSourceFolders.Add("Hello");
                });
        }
    }
}
