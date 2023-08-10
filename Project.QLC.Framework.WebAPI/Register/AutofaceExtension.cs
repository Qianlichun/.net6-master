using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using SqlSugar;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.IdentityModel.Tokens.Jwt;
using Project.QLC.Framework.Common.Helper;

namespace Project.QLC.Framework.WebAPI.Register
{
    /// <summary>
    /// 方法名称：AutofaceExtension
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 9:31:49
    /// </summary>
    public static class AutofaceExtension
    {
        public static void RegisterAutofac(this WebApplicationBuilder applicationBuilder, IConfiguration configuration, Serilog.ILogger logger)
        {
            applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//通过工厂替换，把Autofac整合进来
            applicationBuilder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                #region 注册每个控制器和抽象之间的关系
                {
                    var controllerBaseType = typeof(ControllerBase);
                    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType);
                }
                #endregion

                #region 通过接口和实现类所在程序集注册 
                {
                    Assembly interfaceAssembly = Assembly.Load("Project.QLC.Framework.Repository");
                    Assembly serviceAssembly = Assembly.Load("Project.QLC.Framework.Service");
                    containerBuilder.RegisterAssemblyTypes(interfaceAssembly, serviceAssembly).AsImplementedInterfaces();
                }
                #endregion

                #region 注册SqlSugar 
                {
                    containerBuilder.Register<ISqlSugarClient>(context =>
                    {
                        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                        {
                            ConnectionString = configuration["ConnectionStrings:mysql"],
                            DbType = SqlSugar.DbType.MySql,
                            InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                            IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了

                        });
                        db.Aop.OnLogExecuting = (sql, par) =>
                        {
                            //Console.WriteLine($"Sql语句:{UtilMethods.GetNativeSql(sql, par)}");
                            logger.Information($"Sql语句:{UtilMethods.GetNativeSql(sql, par)}");
                        };
                        return db;
                    });
                }
                #endregion


                #region jwt相关
                //用于jwt的各种操作
                containerBuilder.RegisterType<JwtSecurityTokenHandler>().InstancePerLifetimeScope();
                //自己写的支持泛型存入jwt  便于扩展
                containerBuilder.RegisterType<TokenHelper>().InstancePerLifetimeScope();

                #endregion
            });
        }
    }
}