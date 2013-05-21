using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using ScriptCs;
using ScriptCs.Contracts;
using ScriptCs.Engine.Roslyn;

namespace CodeOwls.PowerShell.ScriptCS.Sessions
{
    class ScriptCSSession : IScriptCSSession
    {
        private readonly IScriptEngine _engine;
        private readonly ScriptPackSession _scriptPackSession;
        private readonly CurrentCmdletScriptPack _currentCmdletScriptPack;
        private readonly CurrentLogger _logger;

        ScriptCSSession( IScriptEngine engine, ScriptPackSession scriptPackSession, CurrentCmdletScriptPack currentCmdletScriptPack, CurrentLogger logger)
        {
            _engine = engine;
            _scriptPackSession = scriptPackSession;
            _currentCmdletScriptPack = currentCmdletScriptPack;
            _logger = logger;
        }

        public IDisposable PushLogger(ILog logger)
        {
            return _logger.CreateActiveLoggerSession(logger);
        }

        public IDisposable PushCmdletContext(ICmdletContext context)
        {
            return _currentCmdletScriptPack.CreateActiveCmdletSession(context);
        }

        public object Execute( string script, string[] references, string[] namespaces )
        {
            return _engine.Execute(script, references, namespaces, _scriptPackSession);
        }
      
        static public ScriptCSSession Create(Cmdlet cmdlet)
        {
            ScriptCSSession session = null;
            var logger = new CurrentLogger();

            using (logger.CreateActiveLoggerSession(new CmdletLogger(cmdlet)))
            {
                IScriptHostFactory factory = new ScriptHostFactory();
                var engine = new RoslynScriptEngine(factory, logger);
                engine.BaseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var currentCmdletScriptPack = new CurrentCmdletScriptPack();
                var scriptPackSession = new ScriptPackSession(new IScriptPack[] { currentCmdletScriptPack });
                scriptPackSession.InitializePacks();                

                session = new ScriptCSSession( engine, scriptPackSession, currentCmdletScriptPack, logger );
            }

            return session;
        }

    }
}
