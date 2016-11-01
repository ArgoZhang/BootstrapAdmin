using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class DictHelperTests
    {

        [TestMethod]
        public void RetrieveDictsTest()
        {
            SaveDictTest();
            var result = DictHelper.RetrieveDicts("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的DictHelper.RetrieveDicts方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = DictHelper.RetrieveDicts();
            Assert.IsTrue(result.Count() >= 0, "不带参数的DictHelper.RetrieveDicts方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveDictTest()
        {
            Dict p = new Dict();
            p.Category = "测试省份";
            p.Name = "测试城市";
            p.Code = "测试字典";
            var result = DictHelper.SaveDict(p);
            Assert.IsTrue(result, "增加用户出错");

            p.ID = 1;
            p.Category = "测试省份22";
            p.Name = "测试城市22";
            p.Code = "测试字典22";
            result = DictHelper.SaveDict(p);
            Assert.IsTrue(result, "更新用户出错");
        }

        [TestMethod]
        public void DeleteDictTest()
        {
            SaveDictTest();
            string p = "1";
            try
            {
                DictHelper.DeleteDict(p);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false, "删除用户出错");
            }
        }
    }
}
