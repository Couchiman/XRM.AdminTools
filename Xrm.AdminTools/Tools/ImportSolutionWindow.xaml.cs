using Microsoft.Crm.Sdk.Messages;
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



using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace XRM.AdminTools
{
    /// <summary>
    /// Lógica de interacción para ImportSolutionWindow.xaml
    /// </summary>
    /// 

    
    public partial class ImportSolutionWindow : Window
    {
        public CrmServiceClient service { get; set; }

        List<Files> files;
        private int solutionCount = 0;
        private int counter = 0;
        private int progress = 0;

      
        public ImportSolutionWindow()
        {
            InitializeComponent();
            _ProgressDialogImport.DoWork += new DoWorkEventHandler(_ImportProgressDialog_DoWork);

        }

        private void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Solution files (*.zip)|*.zip|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            dtgFilesToImport.Items.Clear();
            files = null;


            if (_ProgressDialogImport.IsBusy)
                MessageBox.Show(this, "Import Solutions... Please Wait", "Import Solutions");
            else
            {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    files = new List<Files>();
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        Files f = new Files();
                        f.Name = filename;
                        files.Add(f);
                        
                          
                    }

                    dtgFilesToImport.ItemsSource = files;
                    solutionCount = files.Count;
                    

                     
                }
            }


        }

        private ProgressDialog _ProgressDialogImport = new ProgressDialog()
        {

            WindowTitle = "Importing Solutions",
            Text = "Importing Selected Solutions",
            Description = "Processing...",
            ShowTimeRemaining = true,
        };

        private void _ImportProgressDialog_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_ProgressDialogImport.CancellationPending) return;


            foreach (Files file in files)
            {
                counter++;
                _ProgressDialogImport.ReportProgress(progress, null, string.Format(System.Globalization.CultureInfo.CurrentCulture, "Processing: {0}%", progress));
                progress = (int)Math.Round(((decimal)counter / solutionCount) * 100, 0);


                byte[] fileBytes = File.ReadAllBytes(file.Name);

                var impSolReq = new ImportSolutionRequest()
                {
                    CustomizationFile = fileBytes


                };
                service.Execute(impSolReq);
            }

        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            progress = 0;
            _ProgressDialogImport.Show();
        }

        public class Files
        {
            public string Name { get; set; }
        }
    }
}
