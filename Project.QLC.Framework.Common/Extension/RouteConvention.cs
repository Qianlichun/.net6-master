using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Common.Extension
{
    /// <summary>
    /// 方法名称：全局路由前缀配置
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 10:27:37
    /// </summary>
    public class RouteConvention : IApplicationModelConvention
    {
        /// <summary>
        /// 路由前缀变量
        /// </summary>
        private readonly AttributeRouteModel _centralPrefix;

        /// <summary>
        /// 调用时传入指定的路由前缀
        /// </summary>
        /// <param name="routeTemplateProvider"></param>
        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        /// <summary>
        /// 添加路由前缀程序入口
        /// </summary>
        /// <param name="application"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(ApplicationModel application)
        {
            //遍历所有的控制器
            foreach (var controller in application.Controllers)
            {
                //1.已经标记了RouteAttribute的控制器
                var matchedSelectors = controller.Selectors.Where(o => o.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);
                    }
                }

                //2.没有标记RouteAttribute的控制器
                var unMatchedSelectors = controller.Selectors.Where(o => o.AttributeRouteModel == null).ToList();
                if (unMatchedSelectors.Any())
                {
                    foreach (var selectorModel in unMatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = _centralPrefix;
                    }
                }
            }
        }
    }
}
