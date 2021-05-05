using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        [NotNull]
        private NavigationManager? Navigation { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            Navigation.NavigateTo("/Account/Login", true);
        }
    }
}
