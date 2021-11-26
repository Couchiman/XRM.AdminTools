using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.ComponentModel.DataAnnotations;
namespace XRM.AdminTools
{


  

    public enum AsyncOperationStatusCode
    {
        [Display(Name = "Completed - Succeded")]
        Succeded = 330,
        [Display(Name = "Completed - Failed")]
        Failed =331,
        [Display(Name = "Completed - Canceled")]
        Canceled = 332,
        [Display(Name = "Locked - In Progress")]
        InProgress = 220,
        [Display(Name = "Locked - Pausing")]
        Pausing = 221,
        [Display(Name = "Locked - Canceling")]
        Canceling = 222,
        [Display(Name = "Suspended - Canceling")]  
        Waiting = 110,
        [Display(Name = "Ready - Waiting For Resources")]
        WaitingForResources = 00
    }
     
}
