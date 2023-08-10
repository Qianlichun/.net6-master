using Microsoft.AspNetCore.Mvc;
using Project.QLC.Framework.WebAPI.Extension;
using Project.QLC.Framework.WebAPI.Register;
using Project.QLC.Framework.Common.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Events;
using System.Configuration;

//var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
//logger.Info("init main");
//try
//{


var builder = WebApplication.CreateBuilder(args);

Log.Logger = SeriLogRegister.SeriLogRegisterStart(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging(p =>
{
    p.ClearProviders();
});

//builder.Host.UseNLog();
//全局添加前缀
//builder.Services.AddControllers(o =>
//{
//    o.Conventions.Insert(0, new RouteConvention(new RouteAttribute("api/")));
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.ApiVersionExt();
builder.AddSwaggerGenExt();
builder.AddJwtExtension();

builder.RegisterAutofac(builder.Configuration, Log.Logger);

var app = builder.Build();

app.UseSwaggerExt();
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.AddUseNlog();
app.Run();
//}
//catch (Exception ex)
//{
//    logger.Error(ex, "Stopped program because of exception");
//    throw;
//}
//finally {
//    NLog.LogManager.Shutdown();
//}

