using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bootstrap.Admin.Models;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NotFoundLayout
    {
        [NotNull]
        private ErrorModel? Model { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            var id = 404;
            Model = ErrorModel.CreateById(id);
        }
    }
}
