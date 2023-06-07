﻿using System;
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
using Dateien_Sortierprogramm.Data;
using Dateien_Sortierprogramm.ViewModels;

namespace Dateien_Sortierprogramm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //txt_FolderForTaxStructure.Visibility = Visibility.Hidden;
            //lbl_TaxFolderStructure.Visibility = Visibility.Hidden;
            //btn_AddTaxStructreFolder.Visibility = Visibility.Hidden;

        }

        //private void btn_Add_Click(object sender, RoutedEventArgs e)
        //{
        //    //Hinzufügen von XMLData Objekt
        //    InitializexmlDataCollectionObject();

        //    if (ViewModel.ValidationService.PathValidation(txt_TargetFolder.Text) && ViewModel.ValidationService.TermValidation(txt_Keyword.Text))
        //    {
        //        OrderElements filecontent = new OrderElements();
        //        filecontent.Suchwort = txt_Keyword.Text;
        //        filecontent.Zielordner = txt_TargetFolder.Text;


        //        /*TODO: Prüfung einbauen, dass keine Suchwörter doppelt vorkommen können (bei einer langen Liste vielleicht ganz hilfreich)
        //       Hash Array könnte hierbei die Lösung sein, da es nur einmal das ganze Speichert, egal wie oft man es eingibt*/

        //        //Hinzufügen zu dataGrid
        //        //MainPageViewModel.fileContensOfDataGrid.Add(filecontent);
        //        //dg_Sorting.Items.Add(filecontent);

        //        //Hinzufügen zu XMLData Objekt
        //        //xmlDataCollector.SourceFolder.Add(txt_sourceFolder.Text);
        //        xmlDataCollector.ExecuteList.Add(filecontent);
        //        //Refresh DataGrid Sorting
        //        dg_Sorting.ItemsSource = null;
        //        dg_Sorting.ItemsSource = xmlDataCollector.ExecuteList;
        //    }
        //}

        //private void btn_Save_Click(object sender, RoutedEventArgs e)
        //{

        //    if (!String.IsNullOrEmpty(txt_FolderForTaxStructure.Text) && chb_isTaxfolderStructreDesired.IsChecked == true)
        //    {
        //        xmlDataCollector.OrdnerFuerSteuerstruktur = txt_FolderForTaxStructure.Text;
        //        xmlDataCollector.isOrdnerFuerSteuerstruktur = true;
        //    }

        //    XMLSerializerService.CreateXMLFile(xmlDataCollector);
        //}

        //private void btn_Load_Click(object sender, RoutedEventArgs e)
        //{
        //    OrderElements fileContent = new OrderElements();

        //    xmlDataCollector = XMLSerializerService.LoadXMLFile();
        //    if (xmlDataCollector != null)
        //    {

        //        if (xmlDataCollector.isOrdnerFuerSteuerstruktur == true)
        //        {
        //            chb_isTaxfolderStructreDesired.IsChecked = true;
        //            txt_FolderForTaxStructure.Text = xmlDataCollector.OrdnerFuerSteuerstruktur;
        //            chb_isTaxfolderStructreDesired_Click(sender, e);
        //        }
        //        else if (xmlDataCollector.isOrdnerFuerSteuerstruktur == false)
        //        {
        //            chb_isTaxfolderStructreDesired.IsChecked = false;
        //            chb_isTaxfolderStructreDesired_Click(sender, e);
        //        }
        //        MessageBox.Show("Datei erfolgreich geladen.");
        //    }
        //    else
        //        MessageBox.Show("Datei enthält keine Daten oder falsche Datei wurde ausgewählt.");

        //    //Prüfen ob Zielordnerpfade mit Jahresangabe für neues Jahr geupdated sollen, grade beim Steuerordner
        //    xmlDataCollector = SortingAlgorithm.ChangeAllYearRelevantDirectionsToCurrentYear(xmlDataCollector);

        //    //Refresh DataGrid SourceFolders
        //    dg_SourceFolders.ItemsSource = null;
        //    dg_SourceFolders.ItemsSource = xmlDataCollector.SourceFolder;
        //    //Refresh DataGrid Sorting
        //    dg_Sorting.ItemsSource = null;
        //    dg_Sorting.ItemsSource = xmlDataCollector.ExecuteList;
        //}

        //private void btn_Start_Click(object sender, RoutedEventArgs e)
        //{

        //    if (xmlDataCollector == null)
        //    {
        //        MessageBox.Show("Bitte zuerst eine Datei laden oder mindestens einen Quellordner und ein Suchwort mit Zielordner anlegen.");
        //        return;
        //    }
        //    if (!ViewModel.ValidationService.PathValidation(xmlDataCollector))
        //        return;

        //    if (String.IsNullOrEmpty(txt_FolderForTaxStructure.Text) && chb_isTaxfolderStructreDesired.IsChecked == true)
        //    {
        //        MessageBox.Show("Bitte einen Ordnerpfad für die Steuer Ordnerstruktur auswählen.");
        //        return;
        //    }
        //    else if (!String.IsNullOrEmpty(txt_FolderForTaxStructure.Text) && chb_isTaxfolderStructreDesired.IsChecked == true)
        //    {
        //        xmlDataCollector.OrdnerFuerSteuerstruktur = txt_FolderForTaxStructure.Text;
        //        xmlDataCollector.isOrdnerFuerSteuerstruktur = true;
        //        SortingAlgorithm.MoveFileTo(xmlDataCollector, true);
        //    }
        //    else
        //        SortingAlgorithm.MoveFileTo(xmlDataCollector, false);
        //}

        //private void btn_Add_Sources_Click(object sender, RoutedEventArgs e)
        //{
        //    InitializexmlDataCollectionObject();
        //    OrderElements sourceFolder = new OrderElements();
        //    sourceFolder.Quellordner = XMLSerializerService.AddFolderDirectory();

        //    if (ViewModel.ValidationService.PathValidation(sourceFolder.Quellordner))
        //    {
        //        xmlDataCollector.SourceFolder.Add(sourceFolder);
        //        dg_SourceFolders.ItemsSource = null;
        //        dg_SourceFolders.ItemsSource = xmlDataCollector.SourceFolder;
        //    }
        //}

        //private void InitializexmlDataCollectionObject()
        //{
        //    if (xmlDataCollector == null)
        //    {
        //        xmlDataCollector = new XMLData();
        //    }
        //}

        //private void btn_AddTargetFolder_Click(object sender, RoutedEventArgs e)
        //{
        //    txt_TargetFolder.Text = XMLSerializerService.AddFolderDirectory();
        //}




        //private void chb_isTaxfolderStructreDesired_Click(object sender, RoutedEventArgs e)
        //{
        //    if (chb_isTaxfolderStructreDesired.IsChecked == true)
        //    {
        //        txt_FolderForTaxStructure.Visibility = Visibility.Visible;
        //        lbl_TaxFolderStructure.Visibility = Visibility.Visible;
        //        btn_AddTaxStructreFolder.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        txt_FolderForTaxStructure.Visibility = Visibility.Hidden;
        //        lbl_TaxFolderStructure.Visibility = Visibility.Hidden;
        //        btn_AddTaxStructreFolder.Visibility = Visibility.Hidden;

        //    }
        //}

        //private void btn_AddTaxStructreFolder_Click(object sender, RoutedEventArgs e)
        //{
        //    txt_FolderForTaxStructure.Text = XMLSerializerService.AddFolderDirectory();
        //}

    }
}
