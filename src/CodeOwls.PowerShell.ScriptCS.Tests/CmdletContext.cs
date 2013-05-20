using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace CodeOwls.PowerShell.ScriptCS.Tests
{
    public class CmdletContext
    {
        [Fact]
        public void DefaultCmdletContextDoesNothing()
        {
            var pack = new CurrentCmdletScriptPack();
            

            AssertNullCmdletContextBehavior(pack);
        }


        [Fact]
        public void CanPushCustomCmdletContext()
        {
            var pack = new CurrentCmdletScriptPack();
            var cmdletContext = pack.GetContext() as ScriptCS.CurrentCmdletContext;
            Assert.NotNull(cmdletContext);

            var mock = new Mock<ICmdletContext>();
            mock.Setup(c => c.GetVariableValue("test"))
                   .Returns("ok")
                   .Verifiable();

            ICmdletContext context = mock.Object;

            object result = null;
            Assert.Equal(cmdletContext, pack.GetContext());
            using (pack.CreateActiveCmdletSession(context))
            {
                Assert.Equal<object>( context, pack.GetContext() );
                result = cmdletContext.GetVariableValue("test");
            }
            
            Assert.Equal("ok", result);
            mock.Verify();
        }

        [Fact]
        public void CanPopCustomCmdletContext()
        {
            var mock = new Mock<ICmdletContext>();
            mock.Setup(c => c.GetVariableValue("test"))
                   .Returns("ok")
                   .Verifiable();

            ICmdletContext context = mock.Object;

            var pack = new CurrentCmdletScriptPack();
            var cmdletContext = pack.GetContext() as ScriptCS.CurrentCmdletContext;
            Assert.NotNull(cmdletContext);

            using (pack.CreateActiveCmdletSession(context))
            {
                var result = cmdletContext.GetVariableValue("test");
                Assert.Equal("ok", result);
                mock.Verify();
            }

            AssertNullCmdletContextBehavior( pack );
        }

        private static void AssertNullCmdletContextBehavior(CurrentCmdletScriptPack pack)
        {
            var cmdletContext = pack.GetContext() as ScriptCS.CurrentCmdletContext;
            Assert.NotNull(cmdletContext); 
            
            var path = cmdletContext.GetUnresolvedProviderPathFromPSPath("nullpath");
            Assert.Equal(null, path);

            path = cmdletContext.GetResourceString("basename", "resourceid");
            Assert.Equal(null, path);

            ProviderInfo pi = null;
            var paths = cmdletContext.GetResolvedProviderPathFromPSPath("nullpath", out pi);
            Assert.Equal(null, paths);
            Assert.Equal(null, pi);

            var pathLocation = cmdletContext.CurrentProviderLocation("nullpath");
            Assert.Null(pathLocation);

            Assert.Null(cmdletContext.CurrentPSTransaction);
            Assert.Null(cmdletContext.CommandRuntime);
            Assert.Null(cmdletContext.Events);
            Assert.Null(cmdletContext.CommandRuntime);
            Assert.Equal(CommandOrigin.Runspace, cmdletContext.CommandOrigin);
            Assert.Null(cmdletContext.Host);
            Assert.Null(cmdletContext.Input);
            Assert.Null(cmdletContext.InvokeCommand);
            Assert.Null(cmdletContext.InvokeProvider);
            Assert.Null(cmdletContext.JobRepository);
            Assert.Null(cmdletContext.MyInvocation);
            Assert.Null(cmdletContext.ParameterSetName);
            Assert.Null(cmdletContext.SessionState);
            Assert.False(cmdletContext.Stopping);

            var result = cmdletContext.ShouldContinue("query", "caption");
            Assert.False(result);
            result = cmdletContext.ShouldContinue("query", "caption", ref result, ref result);
            Assert.False(result);
            result = cmdletContext.ShouldProcess("target");
            Assert.False(result);
            result = cmdletContext.ShouldProcess("target", "action");
            Assert.False(result);
            result = cmdletContext.ShouldProcess("target", "action", "warn");
            Assert.False(result);
            ShouldProcessReason reason;
            result = cmdletContext.ShouldProcess("target", "action", "warn", out reason);
            Assert.False(result);
            Assert.Equal(ShouldProcessReason.None, reason);

            cmdletContext.ThrowTerminatingError(new ErrorRecord(new ApplicationException(), "id", ErrorCategory.CloseError, null));

            Assert.False(cmdletContext.TransactionAvailable());

            cmdletContext.WriteCommandDetail("asdf");
            cmdletContext.WriteDebug("asdf");
            cmdletContext.WriteError(new ErrorRecord(new ApplicationException(), "id", ErrorCategory.CloseError, null));
            cmdletContext.WriteObject(new object());
            cmdletContext.WriteProgress(new ProgressRecord(0, "activity", "desc"));
            cmdletContext.WriteVerbose("asdf");
            cmdletContext.WriteWarning("asdf");

            var value = cmdletContext.GetVariableValue("test");
            Assert.Equal(null, value);
        }
    }
}
