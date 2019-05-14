using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName)
        {
            var dicts = DictHelper.RetrieveDicts().Where(m => m.Category == "菜单");

            var menus = DbManager.Menus.Find(FilterDefinition<BootstrapMenu>.Empty).ToList();
            menus.ForEach(m =>
            {
                m.CategoryName = dicts.FirstOrDefault(d => d.Code == m.Category)?.Name;
                if (m.ParentId != "0") m.ParentName = menus.FirstOrDefault(p => p.Id == m.ParentId)?.Name;
            });
            return menus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(BootstrapMenu p)
        {
            if (string.IsNullOrEmpty(p.Id))
            {
                p.Id = null;
                DbManager.Menus.InsertOne(p);
                return true;
            }
            else
            {
                var update = Builders<BootstrapMenu>.Update.Set(md => md.ParentId, p.ParentId)
                    .Set(md => md.Name, p.Name)
                    .Set(md => md.Order, p.Order)
                    .Set(md => md.Icon, p.Icon)
                    .Set(md => md.Url, p.Url)
                    .Set(md => md.Category, p.Category)
                    .Set(md => md.Target, p.Target)
                    .Set(md => md.IsResource, p.IsResource)
                    .Set(md => md.Application, p.Application);
                DbManager.Menus.UpdateOne(md => md.Id == p.Id, update);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Delete(IEnumerable<string> value)
        {
            var list = new List<WriteModel<BootstrapMenu>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<BootstrapMenu>(Builders<BootstrapMenu>.Filter.Eq(g => g.Id, id)));
            }
            DbManager.Menus.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<object> RetrieveMenusByRoleId(string roleId) => DbManager.Roles.Find(md => md.Id == roleId).FirstOrDefault().Menus.Select(m => new { Id = m });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public override bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            DbManager.Roles.FindOneAndUpdate(md => md.Id == roleId, Builders<Role>.Update.Set(md => md.Menus, menuIds));
            return true;
        }
    }
}
