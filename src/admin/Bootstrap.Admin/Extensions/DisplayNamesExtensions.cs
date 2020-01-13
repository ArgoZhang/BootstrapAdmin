using Bootstrap.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
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
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool TryGetValue((Type ModelType, string FieldName) cacheKey, out string? displayName) => _displayNameCache.TryGetValue(cacheKey, out displayName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static string GetOrAdd((Type ModelType, string FieldName) cacheKey, Func<(Type, string), string> valueFactory) => _displayNameCache.GetOrAdd(cacheKey, valueFactory);
    }
}
