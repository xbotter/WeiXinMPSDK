using Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Senparc.WeixinTests;

namespace Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot.Tests
{
    /// <summary>
    /// 智能机器人API测试
    /// </summary>
    [TestClass()]
    public class IntelligentRobotApiTests : BaseTest
    {
        // 需要替换为实际的AccessToken和机器人ID进行测试
        string accessToken = "<Your AccessToken>";
        string robotId = "<Your Robot ID>";

        [TestMethod()]
        public async Task GetRobotListAsyncTest()
        {
            try
            {
                var result = await IntelligentRobotApi.GetRobotListAsync(accessToken);
                Console.WriteLine($"获取机器人列表: errcode={result.errcode}, errmsg={result.errmsg}");
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"机器人数量: {result.robot_list?.Count ?? 0}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                // 如果是因为未配置真实的AccessToken，不标记为失败
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public async Task CreateRobotAsyncTest()
        {
            try
            {
                var result = await IntelligentRobotApi.CreateRobotAsync(
                    accessToken, 
                    "测试机器人", 
                    "这是一个测试机器人"
                );
                Console.WriteLine($"创建机器人: errcode={result.errcode}, errmsg={result.errmsg}, robot_id={result.robot_id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public async Task UpdateRobotAsyncTest()
        {
            try
            {
                var result = await IntelligentRobotApi.UpdateRobotAsync(
                    accessToken,
                    robotId,
                    "更新后的机器人名称"
                );
                Console.WriteLine($"修改机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public async Task GetRobotDetailAsyncTest()
        {
            try
            {
                var result = await IntelligentRobotApi.GetRobotDetailAsync(accessToken, robotId);
                Console.WriteLine($"查询机器人: errcode={result.errcode}, errmsg={result.errmsg}");
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"机器人名称: {result.name}");
                    Console.WriteLine($"机器人描述: {result.description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public async Task SendRobotMessageAsyncTest()
        {
            try
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = robotId,
                    userid = "testuser",
                    msgtype = "text",
                    text = new TextMessage
                    {
                        content = "这是一条测试消息"
                    }
                };

                var result = await IntelligentRobotApi.SendRobotMessageAsync(accessToken, request);
                Console.WriteLine($"发送消息: errcode={result.errcode}, errmsg={result.errmsg}, msg_id={result.msg_id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public async Task GetRobotChatRecordAsyncTest()
        {
            try
            {
                var result = await IntelligentRobotApi.GetRobotChatRecordAsync(accessToken, robotId);
                Console.WriteLine($"获取聊天记录: errcode={result.errcode}, errmsg={result.errmsg}");
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"记录数量: {result.record_list?.Count ?? 0}");
                    Console.WriteLine($"是否有更多: {result.has_more}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public async Task DeleteRobotAsyncTest()
        {
            try
            {
                var result = await IntelligentRobotApi.DeleteRobotAsync(accessToken, robotId);
                Console.WriteLine($"删除机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试异常: {ex.Message}");
                if (!accessToken.StartsWith("<"))
                {
                    Assert.Fail();
                }
            }
        }
    }
}
