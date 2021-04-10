using LY_SINTER.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LY_SINTER
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*            User_Level.Authority = 4;
                        Application.Run(new Form_Main());//判断登陆成功时主进程显示主窗口*/

            Register _register = new Register();
            _register.ShowDialog();//显示登陆窗体
            if (_register.DialogResult == DialogResult.OK)
            {
                Application.Run(new Form_Main());//判断登陆成功时主进程显示主窗口
            }
            else
            {
                return;
            }
        }
    }
}