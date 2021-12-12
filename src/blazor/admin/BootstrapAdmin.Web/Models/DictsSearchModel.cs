using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 字典维护自定义高级搜索模型
    /// </summary>
    public class DictsSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 获得/设置 字典标签
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 获得/设置 字典类型
        /// </summary>
        public EnumDictDefine? Define { get; set; }

        /// <summary>
        /// 获得/设置 字典名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 获得/设置 字典代码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFilterAction> GetSearchs()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(Name))
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Name), Name));
            }

            if (!string.IsNullOrEmpty(Code))
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Code), Code));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Category), Category));
            }

            if (Define.HasValue)
            {
                ret.Add(new SearchFilterAction(nameof(Dict.Define), Define.Value.ToString(), FilterAction.Equal));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            Category = null;
            Name = null;
            Define = null;
        }
    }
}
