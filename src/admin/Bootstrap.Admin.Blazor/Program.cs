using BootstrapBlazor.DataAcces.PetaPoco;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using PetaPoco.Providers;
using PetaPoco;
using Bootstrap.Admin.Blazor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddCacheManager();
builder.Services.AddDbAdapter();
builder.Services.AddBootstrapBlazor();
builder.Services.AddRequestLocalization<IOptions<BootstrapBlazorOptions>>((localizerOption, blazorOption) =>
{
    var supportedCultures = blazorOption.Value.GetSupportedCultures();

    localizerOption.SupportedCultures = supportedCultures;
    localizerOption.SupportedUICultures = supportedCultures;
});
builder.Services.AddTableDataService();
builder.Services.AddPetaPoco(option =>
{
    // 配置数据信息
    // 使用 SQLite 数据以及从配置文件中获取数据库连接字符串
    option.UsingProvider<SQLiteDatabaseProvider>()
          .UsingConnectionString(builder.Configuration.GetConnectionString("ba"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value);

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
