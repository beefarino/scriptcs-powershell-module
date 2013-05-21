using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CodeOwls.PowerShell.ScriptCS.Sessions;

namespace CodeOwls.PowerShell.ScriptCS.Cmdlets
{
    static class SessionManagerExtensions
    {
        public static IEnumerable<IScriptCSSession> GetMatchingSessions(this ScriptCSSessionManager manager,
                                                                            string[] sessionNames)
        {
            if (null == sessionNames)
            {
                return from item in manager.GetAll() select item.Value;
            }
            
            var wildcards =
                sessionNames.ToList()
                            .ConvertAll(
                                n =>
                                new WildcardPattern(n, WildcardOptions.CultureInvariant | WildcardOptions.IgnoreCase));
            return (from item in manager.GetAll()
                    where wildcards.Any(w => w.IsMatch(item.Key))
                    select item.Value);

        }

        public static IEnumerable<string> GetMatchingSessionNames(this ScriptCSSessionManager manager,
                                                                            string[] sessionNames)
        {
            if (null == sessionNames)
            {
                return from item in manager.GetAll() select item.Key;
            }

            var wildcards =
                sessionNames.ToList()
                            .ConvertAll(
                                n =>
                                new WildcardPattern(n, WildcardOptions.CultureInvariant | WildcardOptions.IgnoreCase));
            return (from item in manager.GetAll()
                    where wildcards.Any(w => w.IsMatch(item.Key))
                    select item.Key);

        }
    }
}
