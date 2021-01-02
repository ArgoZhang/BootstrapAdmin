using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Web;
using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 显示名称扩展方法类
    /// </summary>
    public static class DisplayNamesExtensions
    {

        private static readonly ConcurrentDictionary<(Type ModelType, string FieldName), string> _displayNameCache = new ConcurrentDictionary<(Type, string), string>();

        /// <summary>
        /// 向系统中加入实体类显示名称字典
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDisplayNames(this IServiceCollection services)
        {
            _displayNameCache.TryAdd((typeof(BootstrapDict), nameof(BootstrapDict.Category)), "字典标签");
            _displayNameCache.TryAdd((typeof(BootstrapDict), nameof(BootstrapDict.Name)), "字典名称");
            _displayNameCache.TryAdd((typeof(BootstrapDict), nameof(BootstrapDict.Code)), "字典代码");
            _displayNameCache.TryAdd((typeof(BootstrapDict), nameof(BootstrapDict.Define)), "字典类型");

            _displayNameCache.TryAdd((typeof(User), nameof(User.UserName)), "登录名称");
            _displayNameCache.TryAdd((typeof(User), nameof(User.DisplayName)), "显示名称");

            _displayNameCache.TryAdd((typeof(Group), nameof(Group.GroupCode)), "部门编码");
            _displayNameCache.TryAdd((typeof(Group), nameof(Group.GroupName)), "部门名称");
            _displayNameCache.TryAdd((typeof(Group), nameof(Group.Description)), "部门描述");

            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Name)), "菜单名称");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.ParentName)), "父级菜单");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Order)), "菜单序号");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Icon)), "菜单图标");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Url)), "菜单路径");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Category)), "菜单类别");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.CategoryName)), "菜单类别");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Target)), "目标");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.IsResource)), "菜单类型");
            _displayNameCache.TryAdd((typeof(BootstrapMenu), nameof(BootstrapMenu.Application)), "所属应用");

            // 缓存显示名称
            _displayNameCache.TryAdd((typeof(CacheItem), nameof(CacheItem.Key)), "缓存 Key");
            _displayNameCache.TryAdd((typeof(CacheItem), nameof(CacheItem.Value)), "缓存值");
            _displayNameCache.TryAdd((typeof(CacheItem), nameof(CacheItem.Interval)), "缓存时长（秒）");
            _displayNameCache.TryAdd((typeof(CacheItem), nameof(CacheItem.ElapsedSeconds)), "已过时长（秒）");
            _displayNameCache.TryAdd((typeof(CacheItem), nameof(CacheItem.Desc)), "缓存说明");

            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.ConnectionId)), "会话Id");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.UserName)), "登录名称");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.DisplayName)), "显示名称");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.FirstAccessTime)), "登录时间");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.LastAccessTime)), "访问时间");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.Method)), "请求方式");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.Ip)), "主机");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.Location)), "登录地点");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.Browser)), "浏览器");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.OS)), "操作系统");
            _displayNameCache.TryAdd((typeof(OnlineUser), nameof(OnlineUser.RequestUrl)), "访问地址");

            return services;
        }

        /// <summary>
        /// 尝试获取指定 Model 指定属性值的显示名称
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool TryGetValue((Type ModelType, string FieldName) cacheKey, out string? displayName) => _displayNameCache.TryGetValue(cacheKey, out displayName);

        /// <summary>
        /// 获得或者添加指定 Model 的指定属性值得显示名称
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static string GetOrAdd((Type ModelType, string FieldName) cacheKey, Func<(Type, string), string> valueFactory) => _displayNameCache.GetOrAdd(cacheKey, valueFactory);
    }
}
