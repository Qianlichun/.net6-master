using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.QLC.Framework.WebAPI.Extension
{
    /// <summary>
    /// 方法名称：JwtExtension
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 13:53:00
    /// </summary>
    public static class JwtExtension
    {
        public static void AddJwtExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //验证发布者
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        //验证接收者
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        //验证是否过期
                        ValidateLifetime = true,
                        //验证私钥
                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });
        }
    }
}