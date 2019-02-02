using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Newbe.Mahua.Receiver.Meow.MahuaApis
{
    class Tools
    {
        public static int messageCount = 0;
        public static string now = DateTime.Now.ToString();
        /// <summary>
        /// 判断是否发消息，消息速率限制函数
        /// </summary>
        /// <returns></returns>
        public static bool MessageControl(int count)
        {
            if (now == DateTime.Now.ToString())
            {
                if (messageCount < count)
                {
                    messageCount++;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                now = DateTime.Now.ToString();
                messageCount = 0;
                return false;
            }
        }




        /// <summary>
        /// 获取at某人的整合格式
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string At(string qq)
        {
            return "[CQ:at,qq=" + qq + "]";
        }


        /// <summary>
        /// 统计字符串中某字符出现的次数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static int CharNum(string str, string search)
        {
            string[] resultString = Regex.Split(str, search, RegexOptions.IgnoreCase);
            return resultString.Length;
        }

        /// <summary>
        /// 运行lua脚本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RunLua(string text, string headRun = "")
        {
            LuaTimeout lua = new LuaTimeout();
            lua.code = text;
            lua.headRun = headRun;
            lua.CallWithTimeout(3000);
            return lua.result;
        }
    }


    /// <summary>
    /// 带超时处理的lua运行类
    /// </summary>
    class LuaTimeout
    {
        public string result = "lua脚本运行超时，请检查代码";
        public string code;
        public string headRun = "";
        NLua.Lua lua = new NLua.Lua();
        public void Run(string code)
        {
            lua["lua_run_result_var"] = "";
            try
            {
                lua.DoFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "lua/head.lua");
                lua.DoString(Encoding.UTF8.GetBytes(headRun));
                lua.DoString(Encoding.UTF8.GetBytes(code));
                if (Tools.CharNum(lua["lua_run_result_var"].ToString(), "\n") > 40)
                    result = "行数超过了20行，限制一下吧";
                else if (lua["lua_run_result_var"].ToString().Length > 2000)
                    result = "字数超过了2000，限制一下吧";
                else
                    result = lua["lua_run_result_var"].ToString();
            }
            catch (Exception e)
            {
                string err = e.Message;
                int l = err.IndexOf("lua/");
                if (l >= 0)
                    err = err.Substring(l);
                result = "代码崩掉啦\r\n" + err;
            }
        }

        public void CallWithTimeout(int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                Run(code);
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                try
                {
                    lua.Dispose();
                }
                catch { }
            }
        }

        public static string Hex2String(string mHex)
        {
            mHex = Regex.Replace(mHex, "[^0-9A-Fa-f]", "");
            if (mHex.Length % 2 != 0)
                mHex = mHex.Remove(mHex.Length - 1, 1);
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return Encoding.Default.GetString(vBytes);
        }
    }
}
