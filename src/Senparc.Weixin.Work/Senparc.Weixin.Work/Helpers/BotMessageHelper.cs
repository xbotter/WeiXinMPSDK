/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：BotMessageHelper.cs
    文件功能描述：企业微信智能机器人消息处理辅助类
    
    
    创建标识：Senparc - 20260224
    创建描述：提供智能机器人Webhook消息的解析和响应辅助方法
    
    官方文档参考：
    https://developer.work.weixin.qq.com/document/path/101028 - 接收机器人消息
    https://developer.work.weixin.qq.com/document/path/101029 - 接收机器人事件
    https://developer.work.weixin.qq.com/document/path/101039 - 智能机器人消息推送
    
    使用说明：
    智能机器人的消息接收处理可以通过以下方式实现：
    
    1. 在控制器中接收POST请求
    2. 使用BotMessageHelper解密和解析消息
    3. 根据消息类型调用相应的处理逻辑
    4. 使用BotMessageHelper生成响应消息
    
    示例代码见：BotMessageHelperTest.cs

----------------------------------------------------------------*/

using System;
using System.IO;
using System.Xml.Linq;
using Senparc.NeuChar;
using Senparc.NeuChar.Helpers;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.Tencent;

namespace Senparc.Weixin.Work.Helpers
{
    /// <summary>
    /// 企业微信智能机器人消息处理辅助类
    /// 用于解析接收到的Webhook消息和生成响应
    /// </summary>
    public static class BotMessageHelper
    {
        /// <summary>
        /// 从Stream中解密并解析机器人消息
        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="postModel">POST模型，包含Token、EncodingAESKey等</param>
        /// <returns>解析后的消息实体</returns>
        public static WorkBotRequestMessageBase ParseBotMessage(Stream inputStream, PostModel postModel)
        {
            XDocument requestDocument;
            
            using (var reader = new StreamReader(inputStream))
            {
                var postDataStr = reader.ReadToEnd();
                var postDataDocument = XDocument.Parse(postDataStr);
                
                // 检查是否加密
                var encryptElement = postDataDocument.Root.Element("Encrypt");
                if (encryptElement != null)
                {
                    // 解密消息
                    string msgXml = null;
                    WXBizMsgCrypt msgCrypt = new WXBizMsgCrypt(postModel.Token, postModel.EncodingAESKey, postModel.CorpId);
                    var result = msgCrypt.DecryptMsg(postModel.Msg_Signature, postModel.Timestamp, postModel.Nonce, postDataStr, ref msgXml);
                    
                    if (result != 0)
                    {
                        throw new Exception($"消息解密失败，错误码：{result}");
                    }
                    
                    requestDocument = XDocument.Parse(msgXml);
                }
                else
                {
                    requestDocument = postDataDocument;
                }
            }
            
            // 解析消息类型
            return ParseBotMessageInternal(requestDocument);
        }

        /// <summary>
        /// 从XDocument解析机器人消息
        /// </summary>
        private static WorkBotRequestMessageBase ParseBotMessageInternal(XDocument doc)
        {
            var msgTypeStr = doc.Root.Element("MsgType")?.Value;
            if (string.IsNullOrEmpty(msgTypeStr))
            {
                throw new Exception("无法获取消息类型（MsgType）");
            }

            WorkBotRequestMessageBase requestMessage;
            var msgType = NeuChar.Helpers.MsgTypeHelper.GetRequestMsgType(doc);
            
            switch (msgType)
            {
                case RequestMsgType.Text:
                    requestMessage = new BotRequestMessageText();
                    break;
                    
                case RequestMsgType.Image:
                    requestMessage = new BotRequestMessageImage();
                    break;
                    
                case RequestMsgType.Event:
                    // 解析事件类型
                    var eventType = doc.Root.Element("Event")?.Value.ToUpper();
                    switch (eventType)
                    {
                        case "ENTER":
                            requestMessage = new BotRequestMessageEvent_Enter();
                            break;
                            
                        case "TEMPLATE_CARD_EVENT":
                            requestMessage = new BotRequestMessageEvent_TemplateCardEvent();
                            break;
                            
                        default:
                            requestMessage = new BotRequestMessageEventBase();
                            break;
                    }
                    break;
                    
                default:
                    // 尝试其他类型
                    if (doc.Root.Element("Text") != null && doc.Root.Element("ImageList") != null)
                    {
                        // 混合消息
                        requestMessage = new BotRequestMessageMixed();
                    }
                    else if (doc.Root.Element("StreamId") != null)
                    {
                        // 流消息
                        requestMessage = new BotRequestMessageStream();
                    }
                    else
                    {
                        throw new Exception($"不支持的消息类型：{msgTypeStr}");
                    }
                    break;
            }
            
            // 填充消息实体
            EntityHelper.FillEntityWithXml((object)requestMessage, doc);
            
            return requestMessage;
        }

        /// <summary>
        /// 创建文本响应消息
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        /// <param name="content">响应内容</param>
        /// <returns>响应消息XML字符串</returns>
        public static string CreateTextResponse(WorkBotRequestMessageBase requestMessage, string content)
        {
            var responseMessage = new ResponseMessageText
            {
                ToUserName = requestMessage.FromUserName,
                FromUserName = requestMessage.ToUserName,
                CreateTime = SystemTime.Now,
                Content = content
            };
            
            return EntityHelper.ConvertEntityToXml(responseMessage).ToString();
        }

        /// <summary>
        /// 加密响应消息
        /// </summary>
        /// <param name="responseXml">响应消息XML</param>
        /// <param name="postModel">POST模型</param>
        /// <returns>加密后的XML字符串</returns>
        public static string EncryptResponse(string responseXml, PostModel postModel)
        {
            var timeStamp = SystemTime.Now.Ticks.ToString();
            var nonce = Guid.NewGuid().ToString("N"); // 使用GUID作为nonce确保唯一性
            
            WXBizMsgCrypt msgCrypt = new WXBizMsgCrypt(postModel.Token, postModel.EncodingAESKey, postModel.CorpId);
            string encryptedXml = null;
            var result = msgCrypt.EncryptMsg(responseXml, timeStamp, nonce, ref encryptedXml);
            
            if (result != 0)
            {
                throw new Exception($"消息加密失败，错误码：{result}");
            }
            
            return encryptedXml;
        }

        /// <summary>
        /// 创建并加密文本响应消息
        /// </summary>
        /// <param name="requestMessage">请求消息</param>
        /// <param name="content">响应内容</param>
        /// <param name="postModel">POST模型</param>
        /// <returns>加密后的响应XML字符串</returns>
        public static string CreateAndEncryptTextResponse(WorkBotRequestMessageBase requestMessage, string content, PostModel postModel)
        {
            var responseXml = CreateTextResponse(requestMessage, content);
            return EncryptResponse(responseXml, postModel);
        }
    }
}
