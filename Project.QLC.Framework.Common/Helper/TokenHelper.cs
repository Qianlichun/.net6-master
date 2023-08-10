using Dm;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project.QLC.Framework.Model.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Project.QLC.Framework.Common.Helper
{
    /// <summary>
    /// 方法名称：TokenHelper
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 13:56:14
    /// </summary>
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public TokenHelper(IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _configuration = configuration;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }
        /// <summary>
        /// 创建加密JwtToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateJwtToken<T>(T user)
        {
            var signingAlogorithm = SecurityAlgorithms.HmacSha256;
            var claimList = CreateClaimList(user);
            //Signature
            //取出私钥并以utf8编码字节输出
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            //使用非对称算法对私钥进行加密
            var signingKey = new SymmetricSecurityKey(secretByte);
            //使用HmacSha256来验证加密后的私钥生成数字签名
            var signingCredentials = new SigningCredentials(signingKey, signingAlogorithm);
            //生成Token
            var Token = new JwtSecurityToken(
            issuer: _configuration["Authentication:Issuer"], //发布者
            audience: _configuration["Authentication:Audience"], //接收者
            claims: claimList, //存放的用户信息
            notBefore: DateTime.UtcNow, //发布时间
            expires: DateTime.UtcNow.AddDays(1), //有效期设置为1天
            signingCredentials //数字签名
            );
            //生成字符串token
            var TokenStr = new JwtSecurityTokenHandler().WriteToken(Token);
            return JwtBearerDefaults.AuthenticationScheme + " " + TokenStr;
        }

        public T GetToken<T>(string Token)
        {
            Type t = typeof(T);

            object objA = Activator.CreateInstance(t);
            var b = _jwtSecurityTokenHandler.ReadJwtToken(Token);
            foreach (var item in b.Claims)
            {
                PropertyInfo _Property = t.GetProperty(item.Type);
                if (_Property != null && _Property.CanRead)
                {
                    _Property.SetValue(objA, item.Value, null);
                }

            }
            return (T)objA;
        }
        /// <summary>
        /// 创建包含用户信息的CalimList
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        private List<Claim> CreateClaimList<T>(T authUser)
        {
            var Class = typeof(TB_USER);
            List<Claim> claimList = new List<Claim>();
            foreach (var item in Class.GetProperties())
            {
                if (item.Name == "UPass")
                {
                    continue;
                }
                claimList.Add(new Claim(item.Name, Convert.ToString(item.GetValue(authUser))));
            }
            return claimList;
        }

        /// <summary>
        /// 解析JWT
        /// </summary>
        /// <param name="token"></param>
        public static TB_USER? JWTDecode(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                var auth = token.Split(" ")[1].Split('.');
                var user = JsonConvert.DeserializeObject<TB_USER>(Base64UrlEncoder.Decode(auth[1]));
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}