using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.ScriptCS.Sessions;

namespace CodeOwls.PowerShell.ScriptCS.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "ScriptCSSession", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    public class RemoveScriptCSSessionCmdlet : Cmdlet
    {
        private ScriptCSSessionManager _manager;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [Alias("Name")]
        public string[] Session { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        protected override void BeginProcessing()
        {
            _manager = new ScriptCSSessionManager();
        }

        protected override void ProcessRecord()
        {
            var sessionNamesToRemove = _manager.GetMatchingSessionNames(Session);
            sessionNamesToRemove.ToList().ForEach(Remove);
        }

        private void Remove(string s)
        {
            if (ShouldRemove(s))
            {
                _manager.Remove(s);
            }
        }

        private bool ShouldRemove(string s)
        {
            return (Force.IsPresent || ShouldProcess(s, "Remove ScriptCS Session"));
        }
    }
}