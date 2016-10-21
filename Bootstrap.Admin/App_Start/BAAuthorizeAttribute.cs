using Longbow.Web.Mvc;
using System;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    class BAAuthorizeAttribute : LgbAuthorizeAttribute
    {

    }
}