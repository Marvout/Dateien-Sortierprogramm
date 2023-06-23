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
                new Folder() { FolderPath = "Testpfad1/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad2/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad3/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad4/Ich/Habe/Keine/Ahnung" },
                new Folder() { FolderPath = "Testpfad5/Ich/Habe/Keine/Ahnung" }
            };
        }
    }
}
