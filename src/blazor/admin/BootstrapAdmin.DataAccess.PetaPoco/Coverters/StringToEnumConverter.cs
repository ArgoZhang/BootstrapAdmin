namespace BootstrapAdmin.DataAccess.PetaPoco.Coverters;

/// <summary>
/// 字符串转枚举转换器
/// </summary>
class StringToEnumConverter
{
    private Type TargetType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetType"></param>
    public StringToEnumConverter(Type targetType) => TargetType = targetType;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object? ConvertFromDb(object? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        object? ret;
        if (value != null && Enum.TryParse(TargetType, value.ToString(), true, out var v))
        {
            ret = v;
        }
        else
        {
            throw new InvalidCastException($"{value} 无法转化为 {TargetType.Name} 成员");
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object? ConvertToDb(object? value)
    {
        object? ret;
        var field = value?.ToString();
        if (!string.IsNullOrEmpty(field) && Enum.TryParse(TargetType, field, out var v))
        {
            ret = v;
        }
        else
        {
            throw new InvalidCastException($"{TargetType.Name} 未定义 {field} 成员");
        }
        return ret;
    }
}
