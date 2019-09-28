//using Microsoft.AspNetCore.Authorization;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System.Collections.Generic;

//namespace Bootstrap.Admin
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class HttpHeaderOperation : IOperationFilter
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="operation"></param>
//        /// <param name="context"></param>
//        public void Apply(OpenApiOperation operation, OperationFilterContext context)
//        {
//            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

//            if (context.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length == 0)
//            {
//                operation.Parameters.Add(new OpenApiParameter()
//                {
//                    Name = "Authorization",  //添加Authorization头部参数
//                    In = ParameterLocation.Header,
//                    Required = false
//                });
//            }
//        }
//    }
//}
