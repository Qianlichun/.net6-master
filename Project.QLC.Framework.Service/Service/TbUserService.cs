using Project.QLC.Framework.Model.Entity;
using Project.QLC.Framework.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Service.Service
{
    /// <summary>
    /// 方法名称：TbUserService
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/3 15:04:30
    /// </summary>
    public class TbUserService : ITbUserService
    {
        private readonly ITbUserRepository _tbUserRepository;

        public TbUserService(ITbUserRepository tbUserRepository)
        {
            _tbUserRepository = tbUserRepository;
        }

        public async Task<TB_USER> GetUser(long id)
        {
            var result= await _tbUserRepository.GetByIdAsync(id);
            return result;
        }
    }
}
