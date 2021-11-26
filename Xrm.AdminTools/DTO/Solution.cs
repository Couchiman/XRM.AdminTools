using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRM.AdminTools.DTO
{
    public class Solution
    {
       
        public string description { get; set; }

        public string friendlyname { get; set; }

        public Guid publisherid { get; set; }
        public int customizationoptionvalueprefix { get; set; }
        public string customizationprefix { get; set; }

        public int solutiontype { get; set; }

        public string version { get; set; }
        public Int64 versionnumber { get; set; }

        public string uniquename { get; set; }

        public DateTime installedon { get; set; }
  

        public Guid solutionid { get; set; }

        public List<Solution> Solutions { get; set; }

        public Solution()
        { 

        }
        public Solution(EntityCollection entityCollection)
        {
            Solution solucion=null;
            Solutions = new List<Solution>();

            foreach (Entity e in entityCollection.Entities)
            {
                solucion = new Solution();
                solucion.publisherid = e.GetAttributeValue<EntityReference>("publisherid").Id;
                
                solucion.description = e.GetAttributeValue<string>("description");
                solucion.friendlyname = e.GetAttributeValue<string>("friendlyname");
                
                solucion.solutiontype =  e.Attributes.Contains("solutiontype")  ? e.GetAttributeValue<OptionSetValue>("solutiontype").Value:0;
                solucion.version = e.GetAttributeValue<string>("version");
                solucion.versionnumber = e.GetAttributeValue<Int64>("versionnumber");
                solucion.uniquename = e.GetAttributeValue<string>("uniquename");
                solucion.installedon = e.GetAttributeValue<DateTime>("installedon");
                solucion.customizationoptionvalueprefix = (int)e.GetAttributeValue<AliasedValue>("publicador.customizationoptionvalueprefix").Value;
                solucion.customizationprefix = (string)e.GetAttributeValue<AliasedValue>("publicador.customizationprefix").Value;
               
                solucion.solutionid = e.GetAttributeValue<EntityReference>("publisherid").Id;

                Solutions.Add(solucion);
            }

            
        }
    }


}
 