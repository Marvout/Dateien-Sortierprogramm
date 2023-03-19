using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.Model
{
    public class SortingData
    {
        //TODO: Mach nur eine Model Datei und dann im ViewModel Listen aus Dingen die mehrfach erstellt werden
        public string Quellordner { get; set; } = "";
        public string Suchwort { get; set; } = "";
        public string Zielordner { get; set; } = "";

        


    }
}
