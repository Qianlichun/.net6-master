using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Model.Dto
{
    /// <summary>
    /// 方法名称：UserDto
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/3 16:54:56
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? TICK_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? NICK_NAME_DT { get; set; }
    }
}
