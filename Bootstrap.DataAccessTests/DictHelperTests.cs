using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class DictHelperTests
    {

        private Dict Dict { get; set; }

        [TestInitialize]
        public void Initialized()
        {
            Dict = new Dict() { Category = "__测试字典__", Name = "__测试子字典1__", Code = "2",Define = 0 };
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "Delete from Dicts where Category = '__测试菜单__'"))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }

        [TestMethod]
        public void RetrieveDictsTest()
        {
            Assert.IsTrue(DictHelper.RetrieveDicts().Count() > 1, "不带参数的DictHelper.RetrieveDicts方法调用失败");
        }

        [TestMethod]
        public void SaveDictTest()
        {
            // 测试插入字典记录方法 ID = 0
            Assert.IsTrue(DictHelper.SaveDict(Dict), "插入字典记录操作失败，请检查 DictHelper.SaveDict 方法");
            var dicts = DictHelper.RetrieveDicts();
            Assert.IsTrue(dicts.Count() > 0, "插入字典记录操作失败，请检查 DictHelper.SaveDict 方法");

            // 测试更新字典记录方法 ID != 0
            var dict = dicts.FirstOrDefault(d => d.Category == Dict.Category);
            dict.Name = "__测试子字典2__";
            Assert.IsTrue(DictHelper.SaveDict(dict), string.Format("更新字典记录ID = {0} 操作失败，请检查 DictHelper.SaveDict 方法", dict.ID));
            var dest = DictHelper.RetrieveDicts(dict.ID);
            Assert.IsTrue(dest.Count() == 1, "带参数的DictHelper.RetrieveDicts方法调用失败");
            Assert.AreEqual(dict.Name, dest.First().Name, string.Format("更新字典记录ID = {0} 操作失败，请检查 DictHelper.SaveDict 方法", dict.ID));
        }

        [TestMethod]
        public void DeleteDictTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == Dict.Category);
            if (dict == null) DictHelper.SaveDict(Dict);
            dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == Dict.Category);
            Assert.IsTrue(DictHelper.DeleteDict(dict.ID.ToString()), "DictHelper.DeleteDict 方法调用失败");
        }
    }
}
