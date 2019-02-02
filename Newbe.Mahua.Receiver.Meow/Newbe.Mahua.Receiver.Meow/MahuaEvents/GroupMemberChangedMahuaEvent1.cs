using Newbe.Mahua.MahuaEvents;
using System;
using Newbe.Mahua.Receiver.Meow.MahuaApis;

namespace Newbe.Mahua.Receiver.Meow.MahuaEvents
{
    /// <summary>
    /// 群成员变更事件
    /// </summary>
    public class GroupMemberChangedMahuaEvent1
        : IGroupMemberChangedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public GroupMemberChangedMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessGroupMemberChanged(GroupMemberChangedContext context)
        {
            if(context.GroupMemberChangedType.ToString() == "Increased") //进群
            {
                //_mahuaApi.SendGroupMessage(context.FromGroup, "欢迎" + Tools.At(context.JoinedOrLeftQq) + "进群！请仔细阅读群公告哦~");
            }
            else if (context.GroupMemberChangedType.ToString() == "Decreased")//退群
            {
                
            }
        }
    }
}
