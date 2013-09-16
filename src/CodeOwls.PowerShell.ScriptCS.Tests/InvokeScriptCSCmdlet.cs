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
    public class InvokeScriptCSCmdlet : ScriptCSCmdletTestBase
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
            Assert.Equal(1, results.NotNull().Count());
            Assert.Equal(101, (int)results.NotNull().First().BaseObject);
        }

        [Fact]
        public void AutomaticallyDefinesPSCmdletVariable()
        {
            var results = Invoke("invoke-scriptcs -script 'pscmdlet'");

            Assert.NotNull(results);
            Assert.Equal(1, results.Count);
            Assert.IsType( typeof(CodeOwls.PowerShell.ScriptCS.CurrentCmdletContext), results.First().BaseObject );
        }

        [Fact]
        public void NoOpScriptCSReturnsNull()
        {
            var results = Invoke("invoke-scriptcs -script 'var s = 1;'");

            Assert.NotNull(results);
            Assert.Equal(0, results.NotNull().Count());
        }

        [Fact]
        public void RaisesNoErrorOnValidScriptCSCode()
        {
            var errors = InvokeForErrors("invoke-scriptcs -script '1+1'");

            Assert.NotNull(errors);
            Assert.Equal(0, errors.NotNull().Count());
        }

        [Fact]
        public void RaisesErrorOnScriptCSRuntimeError()
        {
            var errors = InvokeForErrors("invoke-scriptcs -script '1 + new object()'");

            Assert.NotNull(errors);
            Assert.Equal(1, errors.NotNull().Count());
        }

        [Fact]
        public void RaisesErrorOnScriptCSSyntaxError()
        {
            var errors = InvokeForErrors("invoke-scriptcs -script 'sdf1zxcv ? this is not valid code'");

            Assert.NotNull(errors);
            Assert.Equal(1, errors.NotNull().Count());
        }

        [Fact]
        public void ScriptCSThrownExceptionsBecomePowerShellErrors()
        {
            var errors =
                InvokeForErrors(
                    "invoke-scriptcs -script 'throw new ApplicationException(\"this is an exception thrown by ScriptCS\");'");
            Assert.NotNull(errors);
            Assert.Equal(1, errors.NotNull().Count());
            Assert.True( errors[0].Exception.Message.Contains( "exception thrown by ScriptCS"));
        }
    }
}
