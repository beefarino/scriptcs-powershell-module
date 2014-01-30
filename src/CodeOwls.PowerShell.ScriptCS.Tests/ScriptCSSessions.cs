using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace CodeOwls.PowerShell.ScriptCS.Tests
{
    public class ScriptCSSessions : ScriptCSCmdletTestBase
    {
        [Fact]
        public void SessionsAreSticky()
        {
            var results = Invoke(new[]{
                "invoke-scriptcs -session 'asdf' -script 'var s = 121;'",
                "invoke-scriptcs -session 'asdf' -script 's'",
            });

            Assert.NotNull(results);
            Assert.Equal(1, results.NotNull().Count());
            Assert.Equal(121, (int)results.NotNull().First().BaseObject);
        }

        [Fact]
        public void ModifyingOneSessionDoesNotModifyAnother()
        {
            var results = Invoke(new[]{
                "invoke-scriptcs -session 'asdf' -script 'var s = 121;'",
                "invoke-scriptcs -session 'asdf' -script 's'",
            });

            Assert.NotNull(results);
            Assert.Equal(1, results.NotNull().Count());
            Assert.Equal(121, (int)results.NotNull().First().BaseObject);

            results = Invoke(new[]{
                "invoke-scriptcs -session 'Nasdf' -script 's'",
            });

            Assert.Empty(results);
        }

        [Fact]
        public void CanRemoveExistingSession()
        {
            var results = Invoke(new[]{
                "new-scriptcssession -session '4asdf' | out-null" ,
                "get-scriptcssession -session '4asdf'" ,
            });

            Assert.Equal(1, results.Count);
            results = Invoke(new[]{
                "remove-scriptcssession -session 4asdf -force",
            });

            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void CanRemoveNonexistingSessionWithoutError()
        {
            var results = Invoke(new[]{
                "get-scriptcssession -session 'doesnotexist'" ,
            });

            // no error
        }

        [Fact]
        public void GetScriptCSSessionFetchesExistingSessions()
        {
            var results = Invoke(new[]{
                "new-scriptcssession -session '1asdf' | out-null" ,
                "get-scriptcssession",
            });

            Assert.NotNull(results);
            Assert.Equal(1, results.NotNull().Count());            
        }

        [Fact]
        public void GetScriptCSSessionFetchesExistingSessionsByName()
        {
            var results = Invoke(new[]{
                "new-scriptcssession -session '2asdf' | out-null" ,
                "get-scriptcssession -session '2asdf'",
            });

            Assert.NotNull(results);
            Assert.Equal(1, results.NotNull().Count());
        }

        [Fact]
        public void GetScriptCSSessionDoesNotFetchNonExistingSessions()
        {
            var results = Invoke(new[]{
                "new-scriptcssession -session '3asdf' | out-null" ,
                "get-scriptcssession -session 'Nasdf'",
            });

            Assert.Equal(0, results.Count);
        }

    }
}
