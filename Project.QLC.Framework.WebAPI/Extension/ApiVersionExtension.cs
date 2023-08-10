using System;
using System.Collections.Generic;

namespace Project.QLC.Framework.WebAPI.Extension
{
    /// <summary>
    /// 方法名称：ApiVersionExtension
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 10:53:38
    /// </summary>
    public static class ApiVersionExtension
    {
        //添加注册框架版本api控制的支持
        public static void ApiVersionExt(this WebApplicationBuilder builder)
        {
            //添加api版本支持
            builder.Services.AddApiVersioning(o =>
            {
                //是否在想的header信息中返回api版本信息
                o.ReportApiVersions = true;
                //默认的api版本
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                //未指定api版本时，设置api版本为默认的版本
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            //配置api版本信息
            builder.Services.AddVersionedApiExplorer(o =>
            {
                //api版本分组名称
                o.GroupNameFormat = "'v'VVVV";
                //未指定api版本时，设置api版本为默认版本
                o.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }
}