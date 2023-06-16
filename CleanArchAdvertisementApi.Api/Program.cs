using CleanArchAdvertisementApi.Api.Middleware;
using CleanArchAdvertisementApi.Infrastructure;
using CleanArchAdvertisementApi.Sql.Context;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Configure Log4net.
XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.RegisterServices();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AdvertisementContext>(options =>
    options.UseNpgsql(connection));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapWhen(context =>
        context.Request.Path.StartsWithSegments("/imgResize") &&
        context.Request.Query.ContainsKey("id") &&
        context.Request.Query.ContainsKey("height") &&
        context.Request.Query.ContainsKey("width"), builder =>
        {
            builder.UseMiddleware<ResizeImageMiddleware>();
        });

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();
