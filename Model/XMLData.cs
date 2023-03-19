using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.Model
{
    [Serializable]
    public class XMLData
    {
        public string OrdnerFuerSteuerstruktur { get; set; } = "";

        public bool isOrdnerFuerSteuerstruktur { get; set; } = false;
        public List<SortingData>  SourceFolder { get; set; } = new List<SortingData>();
        public List <SortingData> ExecuteList { get; set; } = new List<SortingData> ();

    }
}
