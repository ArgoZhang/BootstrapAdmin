using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryData<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new T[0];

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public int PageItems { get; set; } = 20;
    }
}
