using System;
using System.Collections.Generic;

namespace Project.QLC.Framework.WebAPI.Extension
{
    /// <summary>
    /// 方法名称：NlogExtension
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/4 13:59:31
    /// </summary>
    public static class NlogExtension
    {
        public static void AddUseNlog(this WebApplication application) {
            application.UseHttpsRedirection();
            application.UseStaticFiles();
            application.UseRouting();
            application.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
        }
    }
}