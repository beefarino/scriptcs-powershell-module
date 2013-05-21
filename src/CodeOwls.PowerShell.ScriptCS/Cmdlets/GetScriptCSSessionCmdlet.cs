using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CodeOwls.PowerShell.ScriptCS.Sessions;

namespace CodeOwls.PowerShell.ScriptCS.Cmdlets
{
    [Cmdlet( VerbsCommon.Get, "ScriptCSSession" )]
    public class GetScriptCSSessionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        [Alias("Name")]
        public string[] Session { get; set; }

        protected override void ProcessRecord()
        {
            var manager = new ScriptCSSessionManager();

            var sessionNamesToReturn = manager.GetMatchingSessionNames(Session);
            sessionNamesToReturn.ToList().ForEach(WriteObject);
        }
    }
}
