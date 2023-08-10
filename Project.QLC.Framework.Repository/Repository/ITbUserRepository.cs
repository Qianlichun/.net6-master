using Project.QLC.Framework.Model.Entity;
using Project.QLC.Framework.Repository.BaseRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Repository.Repository
{
    /// <summary>
    /// 方法名称：IUserRepository
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 9:38:35
    /// </summary>
    public interface ITbUserRepository:ISimpleClient<TB_USER>,IBaseRepository<TB_USER>
    {
    }
}
