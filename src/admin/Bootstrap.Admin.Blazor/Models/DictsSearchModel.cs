using Bootstrap.Security;

namespace Bootstrap.Admin.Blazor.Models
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
        public int? Define { get; set; }

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
                ret.Add(new SearchFilterAction(nameof(BootstrapDict.Name), Name));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                ret.Add(new SearchFilterAction(nameof(BootstrapDict.Category), Category));
            }

            if (Define.HasValue)
            {
                ret.Add(new SearchFilterAction(nameof(BootstrapDict.Define), Define, FilterAction.Equal));
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
