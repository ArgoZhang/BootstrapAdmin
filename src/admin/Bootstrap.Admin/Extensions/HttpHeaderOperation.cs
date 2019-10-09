using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Bootstrap.Admin
{
    /// <summary>
    /// IOperationFilter 实现类
    /// </summary>
    public class HttpHeaderOperation : IOperationFilter
    {
        /// <summary>
        /// 应用方法 增加 Authorization 授权头设置
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length == 0)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "Authorization",  //添加Authorization头部参数
                    In = ParameterLocation.Header,
                    Required = false
                });
            }
        }
    }
}
