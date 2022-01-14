using BootstrapAdmin.Web.Core;
using PetaPoco;
using BootstrapAdmin.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class LoginService : ILogin
{
    private IDatabase Database { get; }

    public LoginService(IDatabase database) => Database = database;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="loginTime"></param>
    /// <param name="IP"></param>
    /// <param name="address"></param>
    /// <param name="browser"></param>
    /// <param name="userAgent"></param>
    /// <param name="OS"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public Task<bool> Log(string userName, DateTime loginTime, string IP, string address, string? browser, string userAgent, string OS, bool result)
    {
        var loginUser = new LoginLog()
        {
            UserName = userName,
            LoginTime = DateTime.Now,
            Ip = IP,
            City = address,
            OS = OS,
            Browser = browser,
            UserAgent = userAgent,
            Result = result ? "登录成功" : "登录失败"
        };

        Database.Insert(loginUser);

        return Task.FromResult(true);
    }
}
