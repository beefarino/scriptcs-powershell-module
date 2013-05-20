using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeOwls.PowerShell.ScriptCS.Tests
{
    public class InvokeScriptCSCmdlet
    {
        [Fact]
        public void CanExecuteScriptCSCode()
        {
            var results = Invoke("invoke-scriptcs -script '1+1'");

            Assert.NotNull(results);
            Assert.Equal(1, results.Count);
            Assert.Equal(2, (int) results[0].BaseObject);
        }

        [Fact]
        public void HasStickyScriptCSState()
        {
            var results = Invoke(new[]{
                "invoke-scriptcs -script 'var s = 101;'",
                "invoke-scriptcs -script 's'",
            });

            Assert.NotNull(results);
            Assert.Equal(1, results.Count);
            Assert.Equal(101, (int)results[0].BaseObject);
        }

        [Fact]
        public void AutomaticallyDefinesPSCmdletVariable()
        {
            var results = Invoke("invoke-scriptcs -script 'pscmdlet'");

            Assert.NotNull(results);
            Assert.Equal(1, results.Count);
            Assert.IsType( typeof(CurrentCmdletContext), results.First().BaseObject );
        }

        [Fact]
        public void NoOpScriptCSReturnsNull()
        {
            var results = Invoke("invoke-scriptcs -script 'var s = 1;'");

            Assert.NotNull(results);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void RaisesNoErrorOnValidScriptCSCode()
        {
            var errors = InvokeForErrors("invoke-scriptcs -script '1+1'");

            Assert.NotNull(errors);
            Assert.Equal(0, errors.Count);
        }

        [Fact]
        public void RaisesErrorOnScriptCSRuntimeError()
        {
            var errors = InvokeForErrors("invoke-scriptcs -script '1 + new object()'");

            Assert.NotNull(errors);
            Assert.Equal(1, errors.Count);
        }

        [Fact]
        public void RaisesErrorOnScriptCSSyntaxError()
        {
            var errors = InvokeForErrors("invoke-scriptcs -script 'sdf1zxcv ? this is not valid code'");

            Assert.NotNull(errors);
            Assert.Equal(1, errors.Count);
        }

        Collection<PSObject> Invoke(string script)
        {
            using (var rs = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls *.dll | import-module;").Invoke();
                    ps.AddScript(script);

                    var results = ps.Invoke();

                    return results;
                }
            }
        }

        Collection<PSObject> Invoke(IEnumerable<string> script)
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
                                                    ps.Invoke().Where( r=>null != r).ToList().ForEach(results.Add);
                                                });

                    return results;
                }
            }
        }

        PSDataCollection<ErrorRecord> InvokeForErrors(string script)
        {
            using (var rs = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    ps.Runspace = rs;

                    ps.AddScript("ls *.dll | import-module;").Invoke();
                    ps.AddScript(script);

                    ps.Invoke();
                    return ps.Streams.Error;
                }
            }
        }
    }
}
