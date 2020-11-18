using Bootstrap.Admin.Pages.Components;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 消息通知组件
    /// </summary>
    public class NotificationsBase : ComponentBase
    {
        /// <summary>
        /// 获得 授权服务
        /// </summary>
        [Inject]
        protected AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        /// <summary>
        /// 获得/设置 编辑类型实例
        /// </summary>
        protected User DataContext { get; set; } = new User();

        /// <summary>
        /// 获得/设置 用户登录名
        /// </summary>
        protected string? UserName { get; set; }

        /// <summary>
        /// 获得/设置 Table 实例
        /// </summary>

        protected TableBase<User>? Table { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (AuthenticationStateProvider != null)
            {
                var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                UserName = state?.User.Identity!.Name;
            }
        }

        /// <summary>
        /// 数据查询方法
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected QueryData<User> Query(QueryPageOptions options)
        {
            var data = UserHelper.RetrieveNewUsers();
            return new QueryData<User>()
            {
                Items = data,
                PageIndex = 1,
                PageItems = data.Count(),
                TotalCount = data.Count()
            };
        }

        /// <summary>
        /// 批准新用户方法
        /// </summary>
        protected void Approve(string? userId)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(UserName))
            {
                UserHelper.Approve(userId, UserName);
                Table?.Query();
            }
        }

        /// <summary>
        /// 拒绝新用户方法
        /// </summary>
        protected void Reject(string? userId)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(UserName))
            {
                UserHelper.Reject(userId, UserName);
                Table?.Query();
            }
        }
    }
}
