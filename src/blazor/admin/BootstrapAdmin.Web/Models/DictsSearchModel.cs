using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DictsSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EnumDictDefine? Define { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

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
