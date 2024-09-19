using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.Data
{
    [Serializable]
    public class SortingInformation
    {
        public ObservableCollection<Folder> LstSourceFolders = new ObservableCollection<Folder>();
        public ObservableCollection<OrderElements> LstOrderElements = new ObservableCollection<OrderElements>();
        public List<string> LstFileFormats = new List<string>();

        public bool CheckBox_PDF {  get; set; }

        public bool CheckBox_Excel{  get; set; }

        public bool CheckBox_Word{  get; set; }

        public bool CheckBox_Powerpoint{  get; set; }

        public bool CheckBox_Text{  get; set; }

        public bool CheckBox_CSV{  get; set; }

        public bool CheckBox_ZIP{  get; set; }

        public bool CheckBox_JPG{  get; set; }

        public bool CheckBox_PNG{  get; set; }

        public bool CheckBox_GIF{  get; set; }

        public bool CheckBox_BMP{  get; set; }

        public bool CheckBox_MP3{  get; set; }

        public bool CheckBox_MP4{  get; set; }

        public bool CheckBox_WAV{  get; set; }

        public bool CheckBox_AVI{  get; set; }

        public bool CheckBox_MOV{  get; set; }

        public bool CheckBox_MKV{  get; set; }

        public bool CheckBox_WMV{  get; set; }

        public bool CheckBox_RAR{  get; set; }

        public bool CheckBox_XML{  get; set; }

        public bool CheckBox_JSON{  get; set; }

        public bool CheckBox_All{  get; set; }
    }
}
