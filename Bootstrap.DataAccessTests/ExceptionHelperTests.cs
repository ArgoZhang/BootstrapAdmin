using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass()]
    public class ExceptionHelperTests
    {
        [TestMethod()]
        public void LogTest()
        {
            var ex = new Exception("Just for Test");
            var nv = new NameValueCollection();
            nv.Add("ErrorPage", "UnitTest_Page");
            nv.Add("UserIp", "::1");
            ExceptionHelper.Log(ex, nv);
        }
    }
}