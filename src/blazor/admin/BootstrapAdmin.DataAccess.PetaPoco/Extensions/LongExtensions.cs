namespace BootstrapAdmin.DataAccess.PetaPoco.Extensions;

/// <summary>
/// 
/// </summary>
public static class LongExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static int ToInt32(this long source)
    {
        int ret;
        _ = int.TryParse(source.ToString(), out ret);
        return ret;
    }
}
