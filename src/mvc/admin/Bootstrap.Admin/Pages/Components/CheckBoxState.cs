// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// CheckBox 组件状态枚举值
    /// </summary>
    public enum CheckBoxState
    {
        /// <summary>
        /// 未选中
        /// </summary>
        UnChecked,
        /// <summary>
        /// 选中
        /// </summary>
        Checked,
        /// <summary>
        /// 混合模式
        /// </summary>
        Mixed
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CheckBoxStateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string ToCss(this CheckBoxState state)
        {
            var ret = "false";
            switch (state)
            {
                case CheckBoxState.Checked:
                    ret = "true";
                    break;
                case CheckBoxState.Mixed:
                    ret = "mixed";
                    break;
                case CheckBoxState.UnChecked:
                    break;
            }
            return ret;
        }
    }
}
