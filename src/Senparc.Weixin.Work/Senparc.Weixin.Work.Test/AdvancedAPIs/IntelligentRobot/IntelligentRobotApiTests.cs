using Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.Test.CommonApis;

namespace Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot.Tests
{
    /// <summary>
    /// 智能机器人API测试 - 仅提供异步接口
    /// 官方文档参考：
    /// https://developer.work.weixin.qq.com/document/path/100719 - 获取企业内机器人列表
    /// https://developer.work.weixin.qq.com/document/path/101027 - 创建机器人
    /// https://developer.work.weixin.qq.com/document/path/101031 - 修改机器人
    /// https://developer.work.weixin.qq.com/document/path/101032 - 查询机器人
    /// https://developer.work.weixin.qq.com/document/path/101033 - 删除机器人
    /// https://developer.work.weixin.qq.com/document/path/100989 - 发送机器人消息
    /// https://developer.work.weixin.qq.com/document/path/101138 - 获取机器人聊天记录
    /// </summary>
    [TestClass()]
    public class IntelligentRobotApiTests : CommonApiTest
    {
        private string _appKey => AccessTokenContainer.BuildingKey(_corpId, _corpSecret);
        private static string _testRobotId = null;

        #region 获取机器人列表测试

        [TestMethod()]
        public async Task GetRobotListAsyncTest()
        {
            var result = await IntelligentRobotApi.GetRobotListAsync(_appKey);
            Assert.IsNotNull(result);
            Console.WriteLine($"获取机器人列表: errcode={result.errcode}, errmsg={result.errmsg}");
            
            if (result.errcode == ReturnCode_Work.请求成功 && result.robot_list != null && result.robot_list.Count > 0)
            {
                _testRobotId = result.robot_list[0].robot_id;
                Console.WriteLine($"机器人数量: {result.robot_list.Count}");
            }
        }

        #endregion

        #region 创建机器人测试

        [TestMethod()]
        public async Task CreateRobotAsyncTest()
        {
            var testName = $"测试机器人_{DateTime.Now:yyyyMMddHHmmss}";
            var result = await IntelligentRobotApi.CreateRobotAsync(_appKey, testName, "这是一个测试机器人");
            Assert.IsNotNull(result);
            Console.WriteLine($"创建机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            
            if (result.errcode == ReturnCode_Work.请求成功)
            {
                _testRobotId = result.robot_id;
                Assert.IsNotNull(result.robot_id);
                Console.WriteLine($"机器人ID: {result.robot_id}");
            }
        }

        #endregion

        #region 修改机器人测试

        [TestMethod()]
        public async Task UpdateRobotAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var newName = $"更新后的机器人_{DateTime.Now:yyyyMMddHHmmss}";
                var result = await IntelligentRobotApi.UpdateRobotAsync(_appKey, _testRobotId, newName, "描述已更新");
                Assert.IsNotNull(result);
                Console.WriteLine($"修改机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            }
        }

        #endregion

        #region 查询机器人详情测试

        [TestMethod()]
        public async Task GetRobotDetailAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = await IntelligentRobotApi.GetRobotDetailAsync(_appKey, _testRobotId);
                Assert.IsNotNull(result);
                Console.WriteLine($"查询机器人: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"机器人名称: {result.name}, 描述: {result.description}");
                }
            }
        }

        #endregion

        #region 发送机器人消息测试

        [TestMethod()]
        public async Task SendRobotTextMessageAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "text",
                    text = new TextMessage { content = "测试文本消息" }
                };

                var result = await IntelligentRobotApi.SendRobotMessageAsync(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"发送文本消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
        }

        [TestMethod()]
        public async Task SendRobotImageMessageAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "image",
                    image = new ImageMessage { media_id = "test_media_id" }
                };

                var result = await IntelligentRobotApi.SendRobotMessageAsync(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"发送图片消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
        }

        [TestMethod()]
        public async Task SendRobotMarkdownMessageAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "markdown",
                    markdown = new MarkdownMessage { content = "## 测试Markdown\n\n这是**测试**消息" }
                };

                var result = await IntelligentRobotApi.SendRobotMessageAsync(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"发送Markdown消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
        }

        [TestMethod()]
        public async Task SendRobotNewsMessageAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "news",
                    news = new NewsMessage
                    {
                        articles = new List<Article>
                        {
                            new Article
                            {
                                title = "测试图文",
                                description = "这是测试描述",
                                url = "https://work.weixin.qq.com",
                                picurl = "https://example.com/pic.jpg"
                            }
                        }
                    }
                };

                var result = await IntelligentRobotApi.SendRobotMessageAsync(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"发送图文消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
        }

        #endregion

        #region 获取聊天记录测试

        [TestMethod()]
        public async Task GetRobotChatRecordAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = await IntelligentRobotApi.GetRobotChatRecordAsync(_appKey, _testRobotId);
                Assert.IsNotNull(result);
                Console.WriteLine($"获取聊天记录: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"记录数量: {result.record_list?.Count ?? 0}, 是否有更多: {result.has_more}");
                }
            }
        }

        [TestMethod()]
        public async Task GetRobotChatRecordWithPaginationAsyncTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = await IntelligentRobotApi.GetRobotChatRecordAsync(_appKey, _testRobotId, cursor: null, limit: 10);
                Assert.IsNotNull(result);
                Console.WriteLine($"获取聊天记录(分页): errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == ReturnCode_Work.请求成功)
                {
                    var recordCount = result.record_list?.Count ?? 0;
                    Console.WriteLine($"第一页记录数量: {recordCount}");
                    Assert.IsTrue(recordCount <= 10, "返回的记录数应该不超过限制值");
                }
            }
        }

        #endregion

        #region 删除机器人测试

        [TestMethod()]
        public async Task DeleteRobotAsyncTest()
        {
            var testName = $"待删除_{DateTime.Now:yyyyMMddHHmmss}";
            var createResult = await IntelligentRobotApi.CreateRobotAsync(_appKey, testName, "待删除的测试机器人");
            
            if (createResult.errcode == ReturnCode_Work.请求成功 && !string.IsNullOrEmpty(createResult.robot_id))
            {
                Console.WriteLine($"已创建测试机器人: {createResult.robot_id}");
                
                var deleteResult = await IntelligentRobotApi.DeleteRobotAsync(_appKey, createResult.robot_id);
                Assert.IsNotNull(deleteResult);
                Console.WriteLine($"删除机器人: errcode={deleteResult.errcode}, errmsg={deleteResult.errmsg}");
            }
        }

        #endregion
    }
}
