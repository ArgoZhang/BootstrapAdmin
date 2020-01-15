using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 显示名称扩展方法类
    /// </summary>
    public static class DisplayNamesExtensions
    {

        private static ConcurrentDictionary<(Type ModelType, string FieldName), string> _displayNameCache = new ConcurrentDictionary<(Type, string), string>();

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

            _displayNameCache.TryAdd((typeof(BootstrapUser), nameof(BootstrapUser.UserName)), "登录名称");
            _displayNameCache.TryAdd((typeof(BootstrapUser), nameof(BootstrapUser.DisplayName)), "显示名称");

            _displayNameCache.TryAdd((typeof(Group), nameof(Group.GroupCode)), "部门编码");
            _displayNameCache.TryAdd((typeof(Group), nameof(Group.GroupName)), "部门名称");
            _displayNameCache.TryAdd((typeof(Group), nameof(Group.Description)), "部门描述");
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
