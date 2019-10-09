using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class HttpHeaderOperationTest
    {
        [Fact]
        public void Apply_Ok()
        {
            var oper = new HttpHeaderOperation();
            var api = new OpenApiOperation();
            var desc = new ApiDescription();
            var mi = typeof(HttpHeaderOperationTest).GetMethod("Apply_Ok");
            var context = new OperationFilterContext(desc, null, null, mi);
            oper.Apply(api, context);
        }
    }
}
