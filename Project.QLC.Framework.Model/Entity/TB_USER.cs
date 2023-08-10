using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Model.Entity
{
    /// <summary>
    /// 方法名称：TB_USER
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 9:39:10
    /// </summary>
    public partial class TB_USER
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PASSWORD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? NICK_NAME { get; set; }
    }
}
