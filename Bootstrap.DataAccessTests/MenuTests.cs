using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass()]
    public class MenuTests
    {
        [TestMethod()]
        public void RetrieveMenusTest()
        {
            var result = TerminalHelper.RetrieveTerminals("1");
            Assert.Equals(result.Count(), 1);
            result = TerminalHelper.RetrieveTerminals("");
            Assert.Equals(result.Count(), 2);
        }
    }
}