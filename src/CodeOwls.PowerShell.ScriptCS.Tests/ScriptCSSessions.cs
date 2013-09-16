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

    }
}
