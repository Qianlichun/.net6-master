using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Repository.BaseRepository
{
    /// <summary>
    /// 方法名称：IBaseRepository
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 9:36:55
    /// </summary>
    public interface IBaseRepository<T> : ISimpleClient<T> where T:class,new()
    {
    }
}
