using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    public class QueryData<T> where T : class
    {
        public int total { get; set; }

        public IEnumerable<T> rows { get; set; }
    }
}