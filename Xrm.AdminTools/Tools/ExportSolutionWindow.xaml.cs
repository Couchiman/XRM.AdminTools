using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XRM.AdminTools.DTO;

namespace XRM.AdminTools
{
    /// <summary>
    /// Lógica de interacción para ExportSolutionWindow.xaml
    /// </summary>
    /// 

  
    public partial class ExportSolutionWindow : Window
    {

        public List<Solution> exportSolutions { get; set; }

         private SolutionExportConfiguration SolutionExportConfiguration { get; set; }

        public CrmServiceClient service { get; set; }

        public ExportSolutionWindow()
        {
            InitializeComponent();
            _ProgressDialog.DoWork += new DoWorkEventHandler(_ProgressDialog_DoWork);
        }

        private int solutionCount = 0;
        private int counter = 0;
        private int progress = 0;
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {

        

            if (exportSolutions != null)
            {

                if (_ProgressDialog.IsBusy)
                    MessageBox.Show(this, "Exporting Solutions... Please Wait", "Export Solutions");
                else
                {
                    SolutionExportConfiguration = new SolutionExportConfiguration();
                    SolutionExportConfiguration.isManaged = (bool)rdManaged.IsChecked;
                    SolutionExportConfiguration.ExportAutoNumberingSettings = (bool)chkAutoNumbering.IsChecked;
                    SolutionExportConfiguration.ExportRelationshipRoles = (bool)chkRelationshipRoles.IsChecked;
                    SolutionExportConfiguration.ExportOutlookSynchronizationSettings = (bool)chkExportOutlookSynchronizationSettings.IsChecked;
                    SolutionExportConfiguration.ExportMarketingSettings = (bool)chkExportMarketingSettings.IsChecked;
                    SolutionExportConfiguration.ExportEmailTrackingSettings = (bool)chkExportEmailTrackingSettings.IsChecked;
                    SolutionExportConfiguration.ExportCalendarSettings = (bool)chkExportCalendarSettings.IsChecked;
                    SolutionExportConfiguration.ExportGeneralSettings = (bool)chkExportGeneralSettings.IsChecked;
                    SolutionExportConfiguration.ExportCustomizationSettings = (bool)chkExportCustomizationSettings.IsChecked;
                    SolutionExportConfiguration.ExportExternalApplications = (bool)chkExportExternalApplications.IsChecked;
                    SolutionExportConfiguration.ExportSales = (bool)chkExportSales.IsChecked;
                    SolutionExportConfiguration.ExportIsvConfig = (bool)chkExportIsvConfig.IsChecked;
                    SolutionExportConfiguration.Suffix = (bool)rdFully.IsChecked;
                    SolutionExportConfiguration.ExportDateFolder = (bool)chkExportDateFolder.IsChecked ?  DateTime.Now.ToString("dd-MMM-yyyy"):"";
                    SolutionExportConfiguration.Path = txtExportPath.Text;

                    solutionCount = exportSolutions.Count;
                    counter = 0;
                    progress = 0;

                    _ProgressDialog.Show(); // Sh
                }
                
            }
            
        }

        private ProgressDialog _ProgressDialog = new ProgressDialog()
        {
            
            WindowTitle = "Exporting Solutions",
            Text = "Exporting Selected Solutions",
            Description = "Processing...",
            ShowTimeRemaining = true,
        };

        private void _ProgressDialog_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_ProgressDialog.CancellationPending) return;

           
          
            foreach (Solution en in exportSolutions)
            {

                ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
                exportSolutionRequest.Managed = SolutionExportConfiguration.isManaged;
                exportSolutionRequest.ExportAutoNumberingSettings = SolutionExportConfiguration.ExportAutoNumberingSettings;
                exportSolutionRequest.ExportRelationshipRoles = SolutionExportConfiguration.ExportRelationshipRoles;
                exportSolutionRequest.ExportOutlookSynchronizationSettings = SolutionExportConfiguration.ExportOutlookSynchronizationSettings;
                exportSolutionRequest.ExportMarketingSettings = SolutionExportConfiguration.ExportMarketingSettings;
                exportSolutionRequest.ExportEmailTrackingSettings = SolutionExportConfiguration.ExportEmailTrackingSettings;
                exportSolutionRequest.ExportGeneralSettings = SolutionExportConfiguration.ExportGeneralSettings;
                exportSolutionRequest.ExportCustomizationSettings = SolutionExportConfiguration.ExportCustomizationSettings;
                 exportSolutionRequest.ExportCalendarSettings = SolutionExportConfiguration.ExportCalendarSettings;
                exportSolutionRequest.ExportExternalApplications = SolutionExportConfiguration.ExportExternalApplications;
                exportSolutionRequest.ExportSales = SolutionExportConfiguration.ExportSales;
                exportSolutionRequest.ExportIsvConfig = SolutionExportConfiguration.ExportIsvConfig;
                exportSolutionRequest.SolutionName = en.uniquename;

                
                counter++;
                _ProgressDialog.ReportProgress(progress, en.uniquename, string.Format(System.Globalization.CultureInfo.CurrentCulture, "Processing: {0}%", progress));
                progress = (int)Math.Round(((decimal)counter / solutionCount) * 100, 0);
                 

               ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)service.Execute(exportSolutionRequest);
                byte[] exportXml = exportSolutionResponse.ExportSolutionFile;

                string outputDir = SolutionExportConfiguration.Path +"\\"+ (SolutionExportConfiguration.ExportDateFolder);

                if (!Directory.Exists(outputDir))
                {

                    Directory.CreateDirectory(outputDir);


                }

                string filename = en.uniquename + (SolutionExportConfiguration.Suffix ? "_" + en.version.Replace(".", "_") : "_") +(SolutionExportConfiguration.isManaged ? "_managed" : "")  + ".zip";
                File.WriteAllBytes(outputDir + "\\" + filename, exportXml);


            }

    
        }
        private void btnPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                txtExportPath.Text = dialog.SelectedPath;
            }
        }
    }
}
