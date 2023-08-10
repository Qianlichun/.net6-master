using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Project.QLC.Framework.Common.Helper;
using Project.QLC.Framework.Model.Entity;
using System;
using System.Collections.Generic;

namespace Project.QLC.Framework.WebAPI.Controllers
{
    /// <summary>
    /// 方法名称：BaseController
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 15:40:38
    /// </summary>
    public class BaseController : Controller
    {
        protected TB_USER userInfo = new TB_USER();

        /// <summary>
        /// 执行时操作
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["authorization"];
            userInfo = TokenHelper.JWTDecode(token) ?? new TB_USER();
        }
    }
}