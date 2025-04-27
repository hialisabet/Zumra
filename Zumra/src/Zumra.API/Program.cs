using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Zumra.API.Settings;
using Zumra.Application;
using Zumra.Infrastructure;
using Zumra.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<SwaggerSettings>(
    builder.Configuration.GetSection("Swagger"));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var info = builder.Configuration.GetSection("Swagger");
    c.SwaggerDoc(info["Version"], new OpenApiInfo
    {
        Title = info["Title"],
        Version = info["Version"]
    });

    c.CustomSchemaIds(t => t.FullName);
    c.UseAllOfToExtendReferenceSchemas();
    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();