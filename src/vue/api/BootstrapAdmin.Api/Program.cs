using Microsoft.OpenApi.Models;
using PetaPoco;
using PetaPoco.Providers;

var builder = WebApplication.CreateBuilder(args);

// Configure the service container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BootstrapAdmin API"
    });

    //Set the comments path for the swagger json and ui.  
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, "Bootstrap.Admin.xml");
    //options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
});
builder.Services.AddBootstrapAdminService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.DocumentTitle = "BootstrapAdmin API V1";
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
