using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace CodeOwls.PowerShell.ScriptCS.Tests
{
    public class ScriptCSCmdletTestBase
    {
        protected Collection<PSObject> Invoke(string script)
        {
            Collection<PSObject> results = new Collection<PSObject>();
            using (var rs = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls *.dll | import-module;").Invoke();
                    ps.AddScript(script);

                    ps.Invoke().ToList().ForEach(results.Add);

                    return results;
                }
            }
        }

        protected Collection<PSObject> Invoke(IEnumerable<string> script)
        {
            Collection<PSObject> results = new Collection<PSObject>();
            using (var rs = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls *.dll | import-module;").Invoke();
                    script.ToList().ForEach(s =>
                                                {
                                                    ps.AddScript(s);
                                                    ps.Invoke().ToList().ForEach(results.Add);
                                                });

                    return results;
                }
            }
        }

        protected List<ErrorRecord> InvokeForErrors(string script)
        {
            List<ErrorRecord> errors = null;
            using (var rs = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls *.dll | import-module;").Invoke();
                    ps.AddScript(script);

                    ps.Invoke();
                    errors  = new List<ErrorRecord>( ps.Streams.Error );
                }
            }
            return errors;
        }
    }
}