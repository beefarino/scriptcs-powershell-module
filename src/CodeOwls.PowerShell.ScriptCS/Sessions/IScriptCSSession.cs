using System;
using Common.Logging;

namespace CodeOwls.PowerShell.ScriptCS.Sessions
{
    internal interface IScriptCSSession
    {
        IDisposable PushLogger(ILog logger);
        IDisposable PushCmdletContext(ICmdletContext context);
        object Execute( string script, string[] references, string[] namespaces );
    }
}