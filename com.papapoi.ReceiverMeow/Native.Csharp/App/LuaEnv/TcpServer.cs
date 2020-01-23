using SimpleTCP;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.LuaEnv
{
    class TcpServer
    {
        //需要发送的包列表
        private static ConcurrentBag<string> toSend = new ConcurrentBag<string>();
        //每个包发送间隔时间（可以自己改）
        private static int packTime = 1000;

        private static SimpleTcpServer server = new SimpleTcpServer();
        public static void Start()
        {
            try
            {
                server.StringEncoder = Encoding.UTF8;
                server.Start(23333);
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "tcp server",
                    "tcp server started!");
                server.DataReceived += (sender, msg) => {
                    LuaEnv.RunLua(
                        $"message=[[{msg.MessageString.Replace("]", "] ")}]] ",
                        "envent/ReceiveTcp.lua");
                };
            }
            catch(Exception e)
            {
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Error, "tcp server", 
                    "tcp server start failed!\r\n"+e.ToString());
            }

            //消息发送队列
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        while (toSend.Count > 0)
                        {
                            string temp;
                            toSend.TryTake(out temp);
                            server.Broadcast(temp);
                            Task.Delay(packTime).Wait();
                        }
                        Task.Delay(200).Wait();//等等，防止卡死
                    }
                }
                catch(Exception e)
                {
                    Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Fatal, "tcp server",
                        "tcp server loop failed!\r\n" + e.Message);
                }
            });
        }

        public static void Send(string msg)
        {
            try
            {
                //server.Broadcast(msg);
                toSend.Add(msg);
            }
            catch (Exception e)
            {
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Error, "tcp server",
                    "tcp server send failed!\r\n" + e.ToString());
            }
        }
    }
}
