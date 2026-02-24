/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：IntelligentRobotResult.cs
    文件功能描述：智能机器人接口返回结果
    
    
    创建标识：Senparc - 20260224

----------------------------------------------------------------*/

using System.Collections.Generic;
using Senparc.Weixin.Entities;

namespace Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot
{
    /// <summary>
    /// 获取机器人列表返回结果
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/100719
    /// </summary>
    public class GetRobotListResult : WorkJsonResult
    {
        /// <summary>
        /// 机器人列表
        /// </summary>
        public List<RobotInfo> robot_list { get; set; }
    }

    /// <summary>
    /// 机器人信息
    /// </summary>
    public class RobotInfo
    {
        /// <summary>
        /// 机器人ID
        /// </summary>
        public string robot_id { get; set; }

        /// <summary>
        /// 机器人名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 机器人描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 机器人头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 机器人创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 机器人更新时间
        /// </summary>
        public long update_time { get; set; }
    }

    /// <summary>
    /// 创建机器人返回结果
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/101027
    /// </summary>
    public class CreateRobotResult : WorkJsonResult
    {
        /// <summary>
        /// 机器人ID
        /// </summary>
        public string robot_id { get; set; }
    }

    /// <summary>
    /// 查询机器人返回结果
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/101032
    /// </summary>
    public class GetRobotDetailResult : WorkJsonResult
    {
        /// <summary>
        /// 机器人ID
        /// </summary>
        public string robot_id { get; set; }

        /// <summary>
        /// 机器人名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 机器人描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 机器人头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 机器人创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 机器人更新时间
        /// </summary>
        public long update_time { get; set; }
    }

    /// <summary>
    /// 发送机器人消息返回结果
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/100989
    /// </summary>
    public class SendRobotMessageResult : WorkJsonResult
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string msg_id { get; set; }
    }

    /// <summary>
    /// 获取机器人聊天记录返回结果
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/101138
    /// </summary>
    public class GetRobotChatRecordResult : WorkJsonResult
    {
        /// <summary>
        /// 聊天记录列表
        /// </summary>
        public List<ChatRecord> record_list { get; set; }

        /// <summary>
        /// 是否还有更多记录
        /// </summary>
        public bool has_more { get; set; }

        /// <summary>
        /// 下一页游标
        /// </summary>
        public string next_cursor { get; set; }
    }

    /// <summary>
    /// 聊天记录
    /// </summary>
    public class ChatRecord
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string msg_id { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public long send_time { get; set; }
    }
}
