using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XRM.AdminTools
{
    /// <summary>
    /// Lógica de interacción para WorkflowKillerWindow.xaml
    /// </summary>
    /// 


    public partial class WorkflowStatusChangerWindow : Window
    {

        public CrmServiceClient service { get; set; }

        public WorkflowStatusChangerWindow()
        {
            InitializeComponent();

           
        }

        EntityCollection Results = null;
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

            
            string searchCriteria = string.Empty;

           
            if (string.IsNullOrEmpty(txtFetch.Text))
                searchCriteria = BuildSearch();
            else
                searchCriteria = txtFetch.Text;

            Results = service.GetEntityDataByFetchSearchEC(searchCriteria);
            if (Results != null)
             {
                txtRecordCount.Text = Results.TotalRecordCount.ToString();

                dtgResults.Items.Clear();
                dtgResults.IsReadOnly = true;
                dtgResults.CanUserResizeColumns = true;
                dtgResults.CanUserResizeRows = false;
                dtgResults.CanUserSortColumns = true;
                dtgResults.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                dtgResults.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                dtgResults.SelectionMode = DataGridSelectionMode.Extended;
                dtgResults.ItemsSource = Results.Entities.ToList();

            }


        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstNewStatusCode.SelectedValue != null)
                {


                    foreach (var asyncOperation in Results.Entities)
                    {
                        int statecode = (int)((int)lstNewStatusCode.SelectedValue / 100);
                        int statuscode = (int)lstNewStatusCode.SelectedValue - (statecode * 100);

                        Entity operation = new Entity("asyncoperation")
                        {

                            Id = asyncOperation.Id,
                            ["statecode"] = new OptionSetValue(statecode),
                            ["statuscode"] = new OptionSetValue(statuscode)
                        };
                        service.Update(operation);

                    }
                }
                else
                {
                    MessageBox.Show("Please Select a New Status Code", "Apply Status", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Exception has ocurred " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
        private String BuildSearch()
        {
        
            var fetchXml = $@"
            <fetch distinct='true' no-lock='true' returntotalrecordcount='true'>
              <entity name='asyncoperation'>
                <attribute name='asyncoperationid' />
                <attribute name='name' />
                <attribute name='startedon' />
                <attribute name='workflowstagename' />
                <attribute name='statecode' />
                <attribute name='messagename' />
                <attribute name='statuscode' />
                <filter> 
                <condition attribute='operationtype' operator='eq' value='10' />";

           
            if (lstStatusCode.SelectedValue != null)
                fetchXml = fetchXml + "  <condition attribute='status' operator='eq' value='" + lstStatusCode.SelectedValue + " />";

            if ((bool)chkErrors.IsChecked)
                fetchXml = fetchXml + "  < condition attribute='errorcode' operator='not-null' />";

             fetchXml = fetchXml + "</filter>";

             
            
            fetchXml=fetchXml + "</entity></fetch>";

            return fetchXml;
        }
    }
}
