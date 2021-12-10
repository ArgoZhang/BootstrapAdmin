﻿using System;
using System.Linq;

namespace Bootstrap.Admin.Pages.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TItem Clone<TItem>(this TItem item)
        {
            var ret = item;
            if (item != null)
            {
                var type = item.GetType();
                if (typeof(ICloneable).IsAssignableFrom(type))
                {
                    var clv = type.GetMethod("Clone")?.Invoke(type, null);
                    if (clv != null) ret = (TItem)clv;
                }
                if (type.IsClass)
                {
                    ret = Activator.CreateInstance<TItem>();
                    var valType = ret?.GetType();
                    if (valType != null)
                    {
                        type.GetProperties().ToList().ForEach(p =>
                        {
                            var v = p.GetValue(item);
                            valType.GetProperty(p.Name)?.SetValue(ret, v);
                        });
                    }
                }
            }
            return ret;
        }
    }
}
