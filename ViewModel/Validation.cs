using Dateien_Sortierprogramm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Dateien_Sortierprogramm.ViewModel
{
    public class Validation //was soll hier passieren ?
    {
        public static bool PathValidation(string pathstring)
        {
            if (Directory.Exists(pathstring))
            {
                return true;
            }
            MessageBox.Show(pathstring + "\nPfad existiert nicht! Bitte sicherstellen, dass ein korrekter Pfad eingegeben wurde. Wichtig: Es muss ein \\ als letztes Zeichen stehen.");
            return false;
        }
        public static bool PathValidation(XMLData pathstrings)
        {
            int j = 0;

            for (int i = 0; i < pathstrings.ExecuteList.Count; i++)
            {


                string targetpath = pathstrings.ExecuteList[i].Zielordner;
                
                if (!Directory.Exists(targetpath))
                {
                    MessageBox.Show("Für das Suchwort: \n" + pathstrings.ExecuteList[i].Suchwort + 
                        " existiert der Pfad: \n" + targetpath + 
                        "\n nicht! Bitte sicherstellen, dass ein korrekter Pfad eingegeben wurde. \n" +
                        "Wichtig: Es muss ein \\ als letztes Zeichen stehen.");
                    j++;
                }
            }
            if (j == 0)
                return true;
            else
                return false;
        }

        public static bool TermValidation(string termstring)
        {
            if (!String.IsNullOrWhiteSpace(termstring))
            {
                return true;
            }
            MessageBox.Show("Kein Buchstabe oder nur die Leertaste wurde eingegeben. Bitte ein Wort eingeben.");
            return false;
        }
    }
}
