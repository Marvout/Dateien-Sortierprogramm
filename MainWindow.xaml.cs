using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Resolvers;
using Dateien_Sortierprogramm.Model;
using Dateien_Sortierprogramm.ViewModel;

namespace Dateien_Sortierprogramm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XMLData xmlDataCollector { get; set; }

        private readonly SortingData viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new SortingData();
            txt_FolderForTaxStructure.Visibility = Visibility.Hidden;
            lbl_TaxFolderStructure.Visibility = Visibility.Hidden;
            btn_AddTaxStructreFolder.Visibility = Visibility.Hidden;

        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            //Hinzufügen von XMLData Objekt
            InitializexmlDataCollectionObject();

            if (ViewModel.Validation.PathValidation(txt_TargetFolder.Text) && ViewModel.Validation.TermValidation(txt_Keyword.Text))
            {
                SortingData filecontent = new SortingData();
                filecontent.Suchwort = txt_Keyword.Text;
                filecontent.Zielordner = txt_TargetFolder.Text;


                /*TODO: Prüfung einbauen, dass keine Suchwörter doppelt vorkommen können (bei einer langen Liste vielleicht ganz hilfreich)
               Hash Array könnte hierbei die Lösung sein, da es nur einmal das ganze Speichert, egal wie oft man es eingibt*/

                //Hinzufügen zu dataGrid
                //MainPageViewModel.fileContensOfDataGrid.Add(filecontent);
                //dg_Sorting.Items.Add(filecontent);

                //Hinzufügen zu XMLData Objekt
                //xmlDataCollector.SourceFolder.Add(txt_sourceFolder.Text);
                xmlDataCollector.ExecuteList.Add(filecontent);
                dg_Sorting.ItemsSource = null;
                dg_Sorting.ItemsSource = xmlDataCollector.ExecuteList;
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
           
            if (!String.IsNullOrEmpty(txt_FolderForTaxStructure.Text) && chb_isTaxfolderStructreDesired.IsChecked == true)
            {
                xmlDataCollector.OrdnerFuerSteuerstruktur = txt_FolderForTaxStructure.Text;
                xmlDataCollector.isOrdnerFuerSteuerstruktur = true;
            }
            
            XMLSerializer.CreateXMLFile(xmlDataCollector);
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            SortingData fileContent = new SortingData();

            xmlDataCollector = XMLSerializer.LoadXMLFile();
            if (xmlDataCollector != null)
            {
                dg_SourceFolders.ItemsSource = null;
                dg_SourceFolders.ItemsSource = xmlDataCollector.SourceFolder;
                dg_Sorting.ItemsSource = null;
                dg_Sorting.ItemsSource = xmlDataCollector.ExecuteList;
                if(xmlDataCollector.isOrdnerFuerSteuerstruktur == true)
                {
                    chb_isTaxfolderStructreDesired.IsChecked = true;
                    txt_FolderForTaxStructure.Text = xmlDataCollector.OrdnerFuerSteuerstruktur;
                    chb_isTaxfolderStructreDesired_Click(sender,e);
                }
               else if (xmlDataCollector.isOrdnerFuerSteuerstruktur == false)
                {
                    chb_isTaxfolderStructreDesired.IsChecked = false;
                    chb_isTaxfolderStructreDesired_Click(sender, e);
                }
                MessageBox.Show("Datei erfolgreich geladen.");
            }
            else
                MessageBox.Show("Datei enthält keine Daten oder falsche Datei wurde ausgewählt.");
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
           
            //TODO: prüfen, dass mindestens ein Source Ordner vorhanden ist und mindesten ein Suchpaar.
            if (!ViewModel.Validation.PathValidation(xmlDataCollector))
                return;

            if (String.IsNullOrEmpty(txt_FolderForTaxStructure.Text) && chb_isTaxfolderStructreDesired.IsChecked == true)
            {
                MessageBox.Show("Bitte einen Ordnerpfad für die Steuer Ordnerstruktur auswählen.");
                return;
            }
            else if (!String.IsNullOrEmpty(txt_FolderForTaxStructure.Text) && chb_isTaxfolderStructreDesired.IsChecked == true)
            {
                xmlDataCollector.OrdnerFuerSteuerstruktur = txt_FolderForTaxStructure.Text;
                xmlDataCollector.isOrdnerFuerSteuerstruktur = true;
                SortingAlgorithm.MoveFileTo(xmlDataCollector, true);
            }
            else
                SortingAlgorithm.MoveFileTo(xmlDataCollector, false);
        }

        private void btn_Add_Sources_Click(object sender, RoutedEventArgs e)
        {
            InitializexmlDataCollectionObject();
            SortingData sourceFolder = new SortingData();
            sourceFolder.Quellordner = XMLSerializer.AddFolderDirectory();

            if (ViewModel.Validation.PathValidation(sourceFolder.Quellordner))
            {
                xmlDataCollector.SourceFolder.Add(sourceFolder);
                dg_SourceFolders.ItemsSource = null;
                dg_SourceFolders.ItemsSource = xmlDataCollector.SourceFolder;
            }
        }

        private void InitializexmlDataCollectionObject()
        {
            if (xmlDataCollector == null)
            {
                xmlDataCollector = new XMLData();
            }
        }

        private void btn_AddTargetFolder_Click(object sender, RoutedEventArgs e)
        {
            txt_TargetFolder.Text = XMLSerializer.AddFolderDirectory();
        }




        private void chb_isTaxfolderStructreDesired_Click(object sender, RoutedEventArgs e)
        {
            if (chb_isTaxfolderStructreDesired.IsChecked == true)
            {
                txt_FolderForTaxStructure.Visibility = Visibility.Visible;
                lbl_TaxFolderStructure.Visibility = Visibility.Visible;
                btn_AddTaxStructreFolder.Visibility = Visibility.Visible;
            }
            else
            {
                txt_FolderForTaxStructure.Visibility = Visibility.Hidden;
                lbl_TaxFolderStructure.Visibility = Visibility.Hidden;
                btn_AddTaxStructreFolder.Visibility = Visibility.Hidden;

            }
        }

        private void btn_AddTaxStructreFolder_Click(object sender, RoutedEventArgs e)
        {
            txt_FolderForTaxStructure.Text = XMLSerializer.AddFolderDirectory();
        }

    }
}
