using Dateien_Sortierprogramm.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.Services
{
    public class DummyLogInfoService
    {
        public IEnumerable<LogInfos> SomeLogInfos()
        {
            return new LogInfos[]
            {
                new LogInfos {File = "TesdateiXY.xml" , FromFolder="C:/Users/hanne/Downloads/", ToFolder="C:/Users/hanne/Documents/"},
                new LogInfos {File = "HalloWelt.pdf" , FromFolder="C:/Users/hanne/HalloWelt/", ToFolder="C:/Users/hanne/Documents/HalloWelt/"},
                new LogInfos {File = "GamingCheats.odt" , FromFolder="C:/Users/hanne/Cheats/", ToFolder="C:/Users/hanne/Cheats/hello/"},
                new LogInfos {File = "TesdateiXY1233.xml" , FromFolder="C:/Users/hanne/Downloads/", ToFolder="C:/Users/hanne/Documents/"},
                new LogInfos {File = "TesdateiXYtz7788.xml" , FromFolder="C:/Users/hanne/Downloads/", ToFolder="C:/Users/hanne/Documents/"}
            };
        }
    }
}
