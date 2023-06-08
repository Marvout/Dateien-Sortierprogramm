using Dateien_Sortierprogramm.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.Services
{
    public class DummyFolderPathService
    {
        public IEnumerable<Folder> SomeFolderStrings()
        {
            return new Folder[]
            {
                new Folder() { FolderPath = "Testpfad/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad/Ich/Habe/Keine/Ahnung" }
            };
        }
    }
}
