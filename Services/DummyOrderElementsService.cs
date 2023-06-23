using Dateien_Sortierprogramm.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dateien_Sortierprogramm.Services
{
    class DummyOrderElementsService
    {
        public IEnumerable<OrderElements> SomeOrderElementsStrings()
        {
            return new OrderElements[]
            {
                new OrderElements() {SearchTerm ="Hallo1",  TargetFolderPath= "Testpfad1/Ich/Habe/Keine/Ahnung"},
                new OrderElements() {SearchTerm ="Hallo2",  TargetFolderPath= "Testpfad2/Ich/Habe/Keine/Ahnung"},
                new OrderElements() {SearchTerm ="Hallo3",  TargetFolderPath= "Testpfad3/Ich/Habe/Keine/Ahnung"},
                new OrderElements() {SearchTerm ="Hallo4",  TargetFolderPath= "Testpfad4/Ich/Habe/Keine/Ahnung"},
                new OrderElements() {SearchTerm ="Hallo5",  TargetFolderPath= "Testpfad5/Ich/Habe/Keine/Ahnung"},
            };
        }

    }
}
