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
            try
            {
                throw new Exception("Just for Test", new Exception("Inner Exception"));
            }
            catch (Exception ex)
            {
                var nv = new NameValueCollection();
                nv.Add("ErrorPage", "UnitTest_Page");
                nv.Add("UserIp", "::1");
                nv.Add("UserId", "UnitTest");
                ExceptionHelper.Log(ex, nv);
            }
        }
    }
}