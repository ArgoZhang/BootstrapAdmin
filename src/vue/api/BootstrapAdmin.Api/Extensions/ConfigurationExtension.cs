// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Api.Extensions;

/// <summary>
/// 
/// </summary>
public static class ConfigurationExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static T GetOption<T>(this IConfiguration configuration, Func<T> valueFactory) where T : class
    {
        T val = null;
        Type typeFromHandle = typeof(T);
        IConfigurationSection section = configuration.GetSection(typeFromHandle.Name);
        if (!section.Exists())
        {
            section = configuration.GetSection(typeFromHandle.FullName);
        }

        if (section.Exists())
        {
            val = section.Get<T>();
        }

        if (val == null)
        {
            val = valueFactory();
        }

        return val;
    }
}
