using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Common.CustomAttribute
{
    /// <summary>
    /// 方法名称：DesptitionAttribute
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/7 15:15:06
    /// </summary>
    public class DesptitionAttribute : Attribute
    {
        private string _value;
        public DesptitionAttribute(string value)
        {
            _value = value;
        }

        public string Value()
        {
            return _value;
        }
    }
}
