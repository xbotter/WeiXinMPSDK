using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.Helpers;
using Senparc.WeixinTests;

namespace Senparc.Weixin.Work.Test.Helpers
{
    [TestClass()]
    public class BotMessageHelperTest : BaseTest
    {
        [TestMethod()]
        public void ParseBotTextMessageTest()
        {
            var xmlText = @"<xml>
    <ToUserName><![CDATA[robot_123]]></ToUserName>
    <FromUserName><![CDATA[user_456]]></FromUserName>
    <CreateTime>1640000000</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[你好]]></Content>
    <MsgId>1234567890</MsgId>
</xml>";

            var postModel = new PostModel()
            {
                Token = "test_token",
                EncodingAESKey = "test_key",
                CorpId = "test_corp"
            };

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlText)))
            {
                var requestMessage = BotMessageHelper.ParseBotMessage(stream, postModel);
                Assert.IsNotNull(requestMessage);
                Assert.IsTrue(requestMessage is BotRequestMessageText);
            }
        }
    }
}
