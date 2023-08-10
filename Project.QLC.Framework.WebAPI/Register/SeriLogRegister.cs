using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;

namespace Project.QLC.Framework.WebAPI.Register
{
    /// <summary>
    /// 方法名称：SeriLogRegister
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/10 16:25:28
    /// </summary>
    public static class SeriLogRegister
    {
        public static LoggerConfiguration SeriLogRegisterStart(IConfiguration configuration)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Information)
            .Enrich.FromLogContext() //注册上下文
            .WriteTo.Console() //输出到控制台
            .WriteTo.Logger(conf =>
            {
                //.MinimumLevel.Debug()
                if (configuration["ElasticSearch:isEable"] == "true")
                {
                    conf.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
                    {
                        MinimumLogEventLevel = LogEventLevel.Information,
                        AutoRegisterTemplate = true, //是否ES日志自动注册一个索引模板。
                        OverwriteTemplate = true, //是否覆盖ES日志默认模板，ES8默认不支持写入，需要加此配置
                        FailureCallback = e => Console.WriteLine("ES发送失败！" + e.MessageTemplate),  //日志发送失败触发事件
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,//ES模板版本
                        BatchPostingLimit = 50,//一次性发送日志数量
                        TypeName = null,//ES日志模板
                        EmitEventFailure = EmitEventFailureHandling.RaiseCallback,//设置了当失败时调用FailureCallback
                        IndexFormat = configuration["ElasticSearch:IndexFormat"]
                    });
                }
                conf.WriteTo.File($"Logs/Information/{date}/_.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                fileSizeLimitBytes: 1024 * 5,//日志分割大小单位byte
                retainedFileCountLimit: 10,//留置文件数量
                rollOnFileSizeLimit: true,//启用日志分割
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information); //单个日志文件
                conf.WriteTo.File($"Logs/Error/{date}/_.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                fileSizeLimitBytes: 1024 * 5,
                retainedFileCountLimit: 10,
                rollOnFileSizeLimit: true,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error); //单个日志文件
            });
        }
    }
}