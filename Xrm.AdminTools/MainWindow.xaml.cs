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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shapes;
using XRM.AdminTools.LoginWindow;
using MessageBox = System.Windows.MessageBox;
using XRM.AdminTools.DTO;

namespace XRM.AdminTools
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CrmServiceClient svcClient { get; set; }

  

        public MainWindow()
        {
            InitializeComponent();

            }

        private void BtnConectar_Click(object sender, RoutedEventArgs e)
        {

            #region Login Control
            // Establish the Login control
            CrmLogin ctrl = new CrmLogin();
            // Wire Event to login response. 
            ctrl.ConnectionToCrmCompleted += ctrl_ConnectionToCrmCompleted;
            // Show the dialog. 
            ctrl.ShowDialog();

            // Handel return. 
            if (ctrl.CrmConnectionMgr != null && ctrl.CrmConnectionMgr.CrmSvc != null && ctrl.CrmConnectionMgr.CrmSvc.IsReady)
            {
                MessageBox.Show("Connected!", "Conecction Status", MessageBoxButton.OK, MessageBoxImage.Information);
                btnImportSolution.IsEnabled = true;
                btnWorkFlowKiller.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Connection Failed!", "Conecction Status", MessageBoxButton.OK, MessageBoxImage.Error);
                btnImportSolution.IsEnabled = false;
                btnWorkFlowKiller.IsEnabled = true;
            }

            #endregion

            #region CRMServiceClient
            if (ctrl.CrmConnectionMgr != null && ctrl.CrmConnectionMgr.CrmSvc != null && ctrl.CrmConnectionMgr.CrmSvc.IsReady)
            {
                this.svcClient = ctrl.CrmConnectionMgr.CrmSvc;
                if (svcClient.IsReady)
                {
                     
                    List<Solution> listOfSolutions = loadSolutionGrid(svcClient);

                    grdSoluciones.Items.Clear();
                    grdSoluciones.IsReadOnly = true;
                    grdSoluciones.CanUserResizeColumns = true;
                    grdSoluciones.CanUserResizeRows = false;
                    grdSoluciones.CanUserSortColumns = true;
                    grdSoluciones.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    grdSoluciones.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    grdSoluciones.SelectionMode = DataGridSelectionMode.Extended;
                    grdSoluciones.ItemsSource = listOfSolutions;



                }
                //                    // Get data from CRM . 
                //                    string FetchXML =
                //                        @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                //                        <entity name='account'>
                //                            <attribute name='name' />
                //                            <attribute name='primarycontactid' />
                //                            <attribute name='telephone1' />
                //                            <attribute name='accountid' />
                //                            <order attribute='name' descending='false' />
                //                          </entity>
                //                        </fetch>";

                //                    var Result = svcClient.GetEntityDataByFetchSearchEC(FetchXML);
                //                    if (Result != null)
                //                    {
                //                        MessageBox.Show(string.Format("Found {0} records\nFirst Record name is {1}", Result.Entities.Count, Result.Entities.FirstOrDefault().GetAttributeValue<string>("name")));
                //                    }


                //                    // Core API using SDK OOTB 
                //                    CreateRequest req = new CreateRequest();
                //                    Entity accENt = new Entity("account");
                //                    accENt.Attributes.Add("name", "TESTFOO");
                //                    req.Target = accENt;
                //                    CreateResponse res = (CreateResponse)svcClient.OrganizationServiceProxy.Execute(req);
                //                    //CreateResponse res = (CreateResponse)svcClient.ExecuteCrmOrganizationRequest(req, "MyAccountCreate");
                //                    MessageBox.Show(res.id.ToString());



                //                    // Using Xrm.Tooling helpers. 
                //                    Dictionary<string, CrmDataTypeWrapper> newFields = new Dictionary<string, CrmDataTypeWrapper>();
                //                    // Create a new Record. - Account 
                //                    newFields.Add("name", new CrmDataTypeWrapper("CrudTestAccount", CrmFieldType.String));
                //                    Guid guAcctId = svcClient.CreateNewRecord("account", newFields);

                //                    MessageBox.Show(string.Format("New Record Created {0}", guAcctId));
                //}
            }
            #endregion


        }

        /// <summary>
        /// Raised when the login form process is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrl_ConnectionToCrmCompleted(object sender, EventArgs e)
        {
            if (sender is CrmLogin)
            {
                this.Dispatcher.Invoke(() =>
                {
                    ((CrmLogin)sender).Close();
                });
            }
        }

        private void ExportarSeleccionadoMenuItem_Click(object sender, RoutedEventArgs e)
        {


            if (grdSoluciones.SelectedItems.Count > 0 && svcClient.IsReady)
            {
                ExportSolutionWindow ctrl = new ExportSolutionWindow();
                ctrl.exportSolutions = grdSoluciones.SelectedItems.Cast<Solution>().ToList();
                ctrl.service = svcClient;

                ctrl.Show();
            }
            else
            {
                MessageBox.Show("No Solutions selected", "Exporting Solutions", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnImportSolution_Click(object sender, RoutedEventArgs e)
        {
            if (svcClient.IsReady) {
                ImportSolutionWindow ctrl = new ImportSolutionWindow();
                ctrl.service = svcClient;
                ctrl.Show();
            }
           
        }


        private void BtnWorkFlowKiller_Click(object sender, RoutedEventArgs e)
        {
           // if (svcClient.IsReady)
           // {
                WorkflowStatusChangerWindow ctrl = new WorkflowStatusChangerWindow();
                ctrl.service = svcClient;
                ctrl.Show();
           // }
        }


        public List<Solution> loadSolutionGrid(CrmServiceClient svcClient)
        {
            var fetchData = new
            {
                ismanaged = "0",
                uniquename = "Active",
                uniquename2 = "Default",
                uniquename3 = "Basic",
                friendlyname = "Common Data Services Default Solution"
            };
            var fetchXml = $@"
            <fetch distinct='true' no-lock='true'>
              <entity name='solution'>
                <attribute name='description' />
                <attribute name='friendlyname' />
                <attribute name='publisherid' />
                <attribute name='solutiontype' />
                <attribute name='version' />
                <attribute name='versionnumber' />
                <attribute name='uniquename' />
                <attribute name='installedon' />
                <attribute name='createdon' />
                <attribute name='solutionid' />
                <filter type='and'>
                  <condition attribute='ismanaged' operator='eq' value='{fetchData.ismanaged/*0*/}'/>
                  <filter type='and'>
                    <condition attribute='uniquename' operator='neq' value='{fetchData.uniquename/*Active*/}'/>
                    <condition attribute='uniquename' operator='neq' value='{fetchData.uniquename2/*Default*/}'/>
                    <condition attribute='uniquename' operator='neq' value='{fetchData.uniquename3/*Basic*/}'/>
                    <condition attribute='friendlyname' operator='neq' value='{fetchData.friendlyname/*Common Data Services Default Solution*/}'/>
                  </filter>
                </filter>
                <link-entity name='publisher' from='publisherid' to='publisherid' link-type='outer' alias='publicador'>
                  <attribute name='customizationoptionvalueprefix' />
                  <attribute name='customizationprefix' />
                </link-entity>
              </entity>
            </fetch>";


            EntityCollection entityCollection = svcClient.GetEntityDataByFetchSearchEC(fetchXml);
            List<Solution> listaSoluciones = null;

            if (entityCollection.Entities != null)
            {
                listaSoluciones = new Solution(entityCollection).Solutions;
            }


            return listaSoluciones;

        }

    }
}
 
