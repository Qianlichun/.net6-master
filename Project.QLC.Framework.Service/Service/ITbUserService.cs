using Project.QLC.Framework.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Service.Service
{
    /// <summary>
    /// 方法名称：ITbUserService
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/3 15:03:27
    /// </summary>
    public interface ITbUserService
    {
        public Task<TB_USER> GetUser(long id);
    }
}
