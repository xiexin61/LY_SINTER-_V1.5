using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
    class GetIpAddress
    {
        public class GetApi
        {

            /// <summary>
            /// 获取本机地址
            /// </summary>
            /// <returns></returns>
            public string GetIp_Power()
            {
                string hostName = Dns.GetHostName();
                IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
                var addressV = iPHostEntry.AddressList.FirstOrDefault(q => q.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);//ip4地址
                if (addressV != null)
                {
                    return addressV.ToString();
                }
                return "*****";

            }


        }
    }
}
