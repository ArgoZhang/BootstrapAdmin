using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class LoginLogModel : ITableSearchModel
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName("起止时间")]
    public DateTimeRangeValue? LogTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DisplayName("请求IP")]
    public string? IP { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public LoginLogModel()
    {
        Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFilterAction> GetSearchs()
    {
        var ret = new List<IFilterAction>();

        if (!string.IsNullOrEmpty(IP))
        {
            ret.Add(new SearchFilterAction(nameof(LoginLog.Ip), IP));
        }

        if (LogTime != null)
        {
            ret.Add(new SearchFilterAction(nameof(LoginLog.LoginTime), LogTime.Start, FilterAction.LessThanOrEqual));
            ret.Add(new SearchFilterAction(nameof(LoginLog.LoginTime), LogTime.End, FilterAction.GreaterThanOrEqual));
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        LogTime = new DateTimeRangeValue
        {
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(-7)
        };
        IP = null;
    }
}
