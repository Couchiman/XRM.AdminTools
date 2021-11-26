using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRM.AdminTools.DTO
{
    public class SolutionExportConfiguration
    {

        public string Path { get; set; }
        public bool Suffix { get; set; }
        public bool isManaged { get; set; }

        public bool ExportAutoNumberingSettings { get; set; }

        public bool ExportRelationshipRoles { get; set; }

       public bool ExportOutlookSynchronizationSettings { get; set; }
        public bool ExportMarketingSettings { get; set; }

        public bool ExportEmailTrackingSettings { get; set; }
        public bool ExportCalendarSettings { get; set; }

        public bool ExportGeneralSettings { get; set; }

        public bool ExportCustomizationSettings { get; set; }

        public bool ExportSales { get; set; }

        public bool ExportExternalApplications { get; set; }


        public bool ExportIsvConfig { get; set; }

        public string ExportDateFolder { get; set; }

    }
}
