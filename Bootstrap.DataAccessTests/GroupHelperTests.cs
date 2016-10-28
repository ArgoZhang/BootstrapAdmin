using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class GroupHelperTests
    {
        [TestMethod]
        public void RetrieveGroupsTest()
        {
            var result = GroupHelper.RetrieveGroups("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的GroupHelper.RetrieveGroups方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = GroupHelper.RetrieveGroups();
            Assert.IsTrue(result.Count() >= 0, "不带参数的GroupHelper.RetrieveGroups方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveGroupTest()
        {
            Group p = new Group();
            p.GroupName = "测试群组2";
            p.Description = "测试群组2";

            var result = GroupHelper.SaveGroup(p);
            Assert.IsTrue(result, "增加用户出错");

            p.ID = 4;
            p.GroupName = "测试群组22";
            p.Description = "测试群组22";
            result = GroupHelper.SaveGroup(p);
            Assert.IsTrue(result, "更新用户出错");

        }

        [TestMethod]
        public void DeleteGroupTest()
        {
            string p = "2";
            try
            {
                GroupHelper.DeleteGroup(p);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false, "删除用户出错");
            }
        }

        [TestMethod()]
        public void RetrieveGroupsByUserIdTest()
        {
            var result = GroupHelper.RetrieveGroupsByUserId(1);
            Assert.IsTrue(result.Count() > 0, "根据用户查询群组失败");
        }
        [TestMethod()]
        public void SaveGroupsByUserIdTest()
        {
            var result = GroupHelper.SaveGroupsByUserId(1, "1,2");
            Assert.IsTrue(result == true, "保存用户群组关系失败");
        }

    }
}
