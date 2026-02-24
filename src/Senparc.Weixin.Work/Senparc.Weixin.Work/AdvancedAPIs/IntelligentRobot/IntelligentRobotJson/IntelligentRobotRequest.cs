/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：IntelligentRobotRequest.cs
    文件功能描述：智能机器人接口请求数据
    
    
    创建标识：Senparc - 20260224

----------------------------------------------------------------*/

using System.Collections.Generic;

namespace Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot
{
    /// <summary>
    /// 创建机器人请求数据
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/101027
    /// </summary>
    public class CreateRobotRequest
    {
        /// <summary>
        /// 机器人名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 机器人描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 机器人头像MediaId
        /// </summary>
        public string avatar_mediaid { get; set; }
    }

    /// <summary>
    /// 修改机器人请求数据
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/101031
    /// </summary>
    public class UpdateRobotRequest
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
        /// 机器人头像MediaId
        /// </summary>
        public string avatar_mediaid { get; set; }
    }

    /// <summary>
    /// 发送机器人消息请求数据
    /// 官方文档：https://developer.work.weixin.qq.com/document/path/100989
    /// </summary>
    public class SendRobotMessageRequest
    {
        /// <summary>
        /// 机器人ID
        /// </summary>
        public string robot_id { get; set; }

        /// <summary>
        /// 接收者用户ID
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public TextMessage text { get; set; }

        /// <summary>
        /// 图片消息内容
        /// </summary>
        public ImageMessage image { get; set; }

        /// <summary>
        /// 语音消息内容
        /// </summary>
        public VoiceMessage voice { get; set; }

        /// <summary>
        /// 视频消息内容
        /// </summary>
        public VideoMessage video { get; set; }

        /// <summary>
        /// 文件消息内容
        /// </summary>
        public FileMessage file { get; set; }

        /// <summary>
        /// 文本卡片消息内容
        /// </summary>
        public TextCardMessage textcard { get; set; }

        /// <summary>
        /// 图文消息内容
        /// </summary>
        public NewsMessage news { get; set; }

        /// <summary>
        /// Markdown消息内容
        /// </summary>
        public MarkdownMessage markdown { get; set; }
    }

    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMessage
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
    }

    /// <summary>
    /// 图片消息
    /// </summary>
    public class ImageMessage
    {
        /// <summary>
        /// 图片媒体文件id
        /// </summary>
        public string media_id { get; set; }
    }

    /// <summary>
    /// 语音消息
    /// </summary>
    public class VoiceMessage
    {
        /// <summary>
        /// 语音媒体文件id
        /// </summary>
        public string media_id { get; set; }
    }

    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMessage
    {
        /// <summary>
        /// 视频媒体文件id
        /// </summary>
        public string media_id { get; set; }

        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string description { get; set; }
    }

    /// <summary>
    /// 文件消息
    /// </summary>
    public class FileMessage
    {
        /// <summary>
        /// 文件媒体文件id
        /// </summary>
        public string media_id { get; set; }
    }

    /// <summary>
    /// 文本卡片消息
    /// </summary>
    public class TextCardMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 点击后跳转的链接
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 按钮文字
        /// </summary>
        public string btntxt { get; set; }
    }

    /// <summary>
    /// 图文消息
    /// </summary>
    public class NewsMessage
    {
        /// <summary>
        /// 图文消息列表
        /// </summary>
        public List<Article> articles { get; set; }
    }

    /// <summary>
    /// 图文消息文章
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 点击后跳转的链接
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 图文消息的图片链接
        /// </summary>
        public string picurl { get; set; }
    }

    /// <summary>
    /// Markdown消息
    /// </summary>
    public class MarkdownMessage
    {
        /// <summary>
        /// markdown内容
        /// </summary>
        public string content { get; set; }
    }
}
