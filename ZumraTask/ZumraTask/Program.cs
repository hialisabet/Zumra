using Microsoft.EntityFrameworkCore;
using MediatR;
using ZumraTask.Application.Interfaces;
using ZumraTask.Infrastructure.Persistence;
using ZumraTask.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext → SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. CQRS / DI
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

// 3. Controllers + Validation
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opts =>
    {
        // automatic 400 on model validation failure
        opts.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(context.ModelState);
    });

// 4. Swagger
builder.Services.AddSwaggerGen(c =>
{
    var info = builder.Configuration.GetSection("Swagger");
    c.SwaggerDoc(info["Version"], new() { Title = info["Title"], Version = info["Version"] });
});

var app = builder.Build();

// 5. Apply any pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZumraTask API v1"));

app.MapControllers();
app.Run();
