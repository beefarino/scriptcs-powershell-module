using System;
using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.ScriptCS.Sessions;

namespace CodeOwls.PowerShell.ScriptCS.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "ScriptCSSession")]
    public class NewScriptCSSessionCmdlet : Cmdlet
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
            Session.ToList().ForEach(Create);
        }

        private void Create(string obj)
        {
            if (!VerifySessionDoesNotExist(obj))
            {
                return;
            }

            var session = _manager.GetOrCreate(obj, this);
            // WriteObject(session);
        }

        private bool VerifySessionDoesNotExist(string sessionName)
        {
            if (null != _manager.Get(sessionName) && !Force.IsPresent)
            {
                var errorRecord = new ErrorRecord(
                    new ArgumentException(
                        "The specified ScriptCS session [" + sessionName + "] already exists.  Use the -Force parameter to override this error and recreate the session.",
                        "Session"
                        ),
                    "ScriptCS.SessionAlreadyExists",
                    ErrorCategory.ResourceExists,
                    Session
                    );

                WriteError(errorRecord);
                return false;
            }
            return true;
        }
    }
}