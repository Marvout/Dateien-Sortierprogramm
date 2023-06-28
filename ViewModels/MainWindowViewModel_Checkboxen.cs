using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.ViewModels
{
    public partial class MainWindowViewModel
    {
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
            else if (isChecked == false)
            {
                lstFileFormats.Remove(fileformat);
            }
        }

        private bool checkBox_All;
        public bool CheckBox_All
        {
            get => checkBox_All;
            set
            {
                if(checkBox_All != value)
                {
                    checkBox_All = value;
                    RaisePropertyChanged();
                    ChangeCheckbox_All(checkBox_All);
                }
            }
        }

        private void ChangeCheckbox_All(bool isChecked)
        {
             
               

            if(isChecked == true)
            {
                CheckBox_AVI = true;
                CheckBox_BMP = true;
                CheckBox_CSV = true;
                CheckBox_Excel = true;
                CheckBox_GIF = true;
                CheckBox_JSON = true;
                CheckBox_JPG = true;
                CheckBox_MKV = true;
                CheckBox_MOV = true;
                CheckBox_MP3 = true;
                CheckBox_MP4 = true;
                CheckBox_PDF = true;
                CheckBox_PNG = true;
                CheckBox_Powerpoint = true;
                CheckBox_RAR  = true;
                CheckBox_Text = true;
                CheckBox_WAV = true;
                CheckBox_WMV = true;
                CheckBox_Word = true;
                CheckBox_XML = true;
                CheckBox_ZIP = true;
            }
            else if (isChecked == false)
            {
                CheckBox_AVI = false;
                CheckBox_BMP = false;
                CheckBox_CSV = false;
                CheckBox_Excel = false;
                CheckBox_GIF = false;
                CheckBox_JSON = false;
                CheckBox_JPG = false;
                CheckBox_MKV = false;
                CheckBox_MOV = false;
                CheckBox_MP3 = false;
                CheckBox_MP4 = false;
                CheckBox_PDF = false;
                CheckBox_PNG = false;
                CheckBox_Powerpoint = false;
                CheckBox_RAR = false;
                CheckBox_Text = false;
                CheckBox_WAV = false;
                CheckBox_WMV = false;
                CheckBox_Word = false;
                CheckBox_XML = false;
                CheckBox_ZIP = false;

            }
        }
    }
}
