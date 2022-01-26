// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using PetaPoco;
using System.Reflection;

namespace BootstrapAdmin.DataAccess.PetaPoco;

class BootstrapAdminConventionMapper : ConventionMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pocoType"></param>
    /// <returns></returns>
    public override TableInfo GetTableInfo(Type pocoType)
    {
        var ti = base.GetTableInfo(pocoType);
        ti.AutoIncrement = true;

        // 支持 Oracle 数据库
        ti.SequenceName = $"SEQ_{ti.TableName.ToUpperInvariant()}_ID";

        ti.TableName = pocoType.Name switch
        {
            "Error" => "Exceptions",
            _ => $"{pocoType.Name}s"
        };
        return ti;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pocoProperty"></param>
    /// <returns></returns>
    public override ColumnInfo GetColumnInfo(PropertyInfo pocoProperty) => pocoProperty.DeclaringType?.Name switch
    {
        nameof(Models.User) => GetUserColumnInfo(pocoProperty),
        nameof(Models.Navigation) => GetNavigationColumnInfo(pocoProperty),
        _ => base.GetColumnInfo(pocoProperty)
    };

    private ColumnInfo GetUserColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.User.NewPassword),
            nameof(Models.User.ConfirmPassword),
            nameof(Models.User.Period),
            nameof(Models.User.IsReset)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }

    private ColumnInfo GetNavigationColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.Navigation.HasChildren)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }

    public override Func<object?, object?> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType) => targetProperty.PropertyType.IsEnum && sourceType == typeof(string)
        ? new NumberToEnumConverter(targetProperty.PropertyType).ConvertFromDb
        : base.GetFromDbConverter(targetProperty, sourceType);

    public override Func<object?, object?> GetToDbConverter(PropertyInfo targetProperty) => targetProperty.PropertyType.IsEnum
        ? new NumberToEnumConverter(targetProperty.PropertyType).ConvertToDb
        : base.GetToDbConverter(targetProperty);
}
