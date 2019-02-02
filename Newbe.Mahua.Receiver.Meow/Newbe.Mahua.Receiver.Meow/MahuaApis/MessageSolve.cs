using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newbe.Mahua.MahuaEvents;
using Newbe.Mahua.Receiver.Meow.MahuaApis;
using Newbe.Mahua.Receiver.Meow;
using System.Web;

namespace Newbe.Mahua.Receiver.Meow.MahuaApis
{
    class MessageSolve
    {
        public static string GetReplay(string fromqq,string msg, IMahuaApi _mahuaApi, string fromgroup = "common")
        {
            if (Tools.MessageControl(5))
                return "";

            string result = "";
            if (msg.IndexOf("#lua") == 0 && msg.Length > 4)
            {
                result += Tools.At(fromqq) + "\r\n" + Tools.RunLua(HttpUtility.HtmlDecode(msg.Substring(4)),
                    string.Format("fromqq=\"{0}\"\r\nfromgroup=\"{1}\"\r\n", fromqq, fromgroup));
            }

            return result;
        }
    }
}
