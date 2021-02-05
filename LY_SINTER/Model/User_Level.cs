using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
     /// <summary>
    /// 登录页面声明数据 @LT
    /// </summary>
    class User_Level
    {
        /// <summary>
        /// 权限等级
        /// </summary>
        private static int authority = 0;
        public static int Authority
        {
            get { return authority; }
            set { authority = value; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        private static string user_name = "";
        public static string User_name
        {
            get { return user_name; }
            set { user_name = value; }
        }


        /// <summary>
        /// 下发权限
        /// </summary>
        private static bool Jurisdiction = false;
        public static bool _Jurisdiction
        {
            get { return Jurisdiction; }
            set { Jurisdiction = value; }
        }
    }
}
