using Project.QLC.Framework.Common.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Common.Extension
{
    /// <summary>
    /// 方法名称：GetDesptitionValue
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/7 15:16:48
    /// </summary>
    public static class CustomEnumExtension
    {
        public static string GetDesptitionValue(this Enum enu)
        {
            Type type = enu.GetType();
            FieldInfo fd = type.GetField(enu.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DesptitionAttribute), false);
            string name = string.Empty;
            foreach (DesptitionAttribute attr in attrs)
            {
                name = attr.Value();
            }
            return name;
        }
    }
}
