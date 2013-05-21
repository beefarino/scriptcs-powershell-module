using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace CodeOwls.PowerShell.ScriptCS.Sessions
{
    class ScriptCSSessionManager
    {
        static readonly ScriptCSSessionMap Map = new ScriptCSSessionMap();

        private readonly ScriptCSSessionMap _map;

        public ScriptCSSessionManager()
        {
            _map = Map;
        }

        public ScriptCSSessionManager( ScriptCSSessionMap map )
        {
            _map = map;
        }

        public IScriptCSSession Get(string name)
        {
            IScriptCSSession session = null;
            _map.TryGetValue(name, out session);
            return session;
        }

        public IScriptCSSession GetOrCreate( string name, Cmdlet cmdlet )
        {
            var session = Get(name);
            if (null == session)
            {
                session = ScriptCSSession.Create(cmdlet);
                _map.Add( name, session );
            }

            return session;
        }

        public bool Remove(string name)
        {
            return _map.Remove(name);
        }

        public bool Exists(string name)
        {
            return _map.ContainsKey(name);
        }

        public IDictionary<string, IScriptCSSession> GetAll()
        {
            return new ReadOnlyDictionary<string, IScriptCSSession>( _map );
        }        
    }
}
