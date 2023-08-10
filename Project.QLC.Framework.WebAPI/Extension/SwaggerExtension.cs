using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Project.QLC.Framework.WebAPI.Filter;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Project.QLC.Framework.WebAPI.Extension
{
    /// <summary>
    /// 方法名称：SwaggerExtension
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 10:31:54
    /// </summary>
    public static class SwaggerExtension
    {
        public static void AddSwaggerGenExt(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                #region 组件支持版本展示
                var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    o.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Contact = new OpenApiContact
                        {
                            Name = "钱小小",
                            Email = "1464942639@qq.com"
                        },
                        Description = "测试文档 .NET6 WEBAPI 文档",
                        Title = "测试文档 .NET6 WEBAPI 文档",
                        Version = description.ApiVersion.ToString()
                    });
                }
                // 在swagger文档显示的api地址中将版本号信息参数替换为实际的版本号
                o.DocInclusionPredicate((version, apiDescription) =>
                {
                    if (!version.Equals(apiDescription.GroupName))
                        return false;

                    IEnumerable<string>? values = apiDescription!.RelativePath.Split('/')
                    .Select(v => v.Replace("v{version}", apiDescription.GroupName));

                    apiDescription.RelativePath = string.Join("/", values);
                    return true;
                });
                #endregion

                #region 配置展示注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // xml文档绝对路径
                var file = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ture 显示控制器层注释
                o.IncludeXmlComments(file, true);
                o.OrderActionsBy(x => x.RelativePath);
                #endregion

                #region 扩展传入token 
                //添加安全定义
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入token，格式为Bearer XXXXX",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                //添加安全要求
                o.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference(){
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
                #endregion

                //扩展文件上传按钮
                o.OperationFilter<FileUploadFilter>();
            });
        }


        public static void UseSwaggerExt(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                #region 调用第三方程序包支持版本控制
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                //默认加载最新api文件
                //provider.ApiVersionDescriptions.Reverse()
                //默认加载第一版本的api文档
                foreach (var dpt in provider.ApiVersionDescriptions)
                {
                    o.SwaggerEndpoint($"/swagger/{dpt.GroupName}/swagger.json", $"钱小小 API {dpt.GroupName.ToUpperInvariant()}");
                }
                #endregion
            });
        }

    }
}