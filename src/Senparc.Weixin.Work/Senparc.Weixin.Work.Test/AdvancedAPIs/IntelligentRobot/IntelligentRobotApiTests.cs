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
    /// 智能机器人API测试
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
        // 使用AppKey模式（推荐），支持AccessToken自动管理
        private string _appKey
        {
            get { return AccessTokenContainer.BuildingKey(_corpId, _corpSecret); }
        }

        // 静态变量，用于在测试之间共享创建的机器人ID
        private static string _testRobotId = null;

        #region 获取机器人列表测试

        /// <summary>
        /// 【同步方法】获取机器人列表测试
        /// </summary>
        [TestMethod()]
        public void GetRobotListTest()
        {
            var result = IntelligentRobotApi.GetRobotList(_appKey);
            Assert.IsNotNull(result);
            Console.WriteLine($"[同步]获取机器人列表: errcode={result.errcode}, errmsg={result.errmsg}");
            
            if (result.errcode == Entities.ReturnCode_Work.请求成功)
            {
                Assert.IsNotNull(result.robot_list);
                Console.WriteLine($"机器人数量: {result.robot_list?.Count ?? 0}");
                
                // 如果有机器人，输出详细信息
                if (result.robot_list != null && result.robot_list.Count > 0)
                {
                    foreach (var robot in result.robot_list)
                    {
                        Console.WriteLine($"  机器人ID: {robot.robot_id}, 名称: {robot.name}");
                        // 保存第一个机器人ID供后续测试使用
                        if (string.IsNullOrEmpty(_testRobotId))
                        {
                            _testRobotId = robot.robot_id;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 【异步方法】获取机器人列表测试
        /// </summary>
        [TestMethod()]
        public async Task GetRobotListAsyncTest()
        {
            var result = await IntelligentRobotApi.GetRobotListAsync(_appKey);
            Assert.IsNotNull(result);
            Console.WriteLine($"[异步]获取机器人列表: errcode={result.errcode}, errmsg={result.errmsg}");
            
            if (result.errcode == Entities.ReturnCode_Work.请求成功)
            {
                Assert.IsNotNull(result.robot_list);
                Console.WriteLine($"机器人数量: {result.robot_list?.Count ?? 0}");
                
                // 如果有机器人，输出详细信息
                if (result.robot_list != null && result.robot_list.Count > 0)
                {
                    foreach (var robot in result.robot_list)
                    {
                        Console.WriteLine($"  机器人ID: {robot.robot_id}, 名称: {robot.name}");
                    }
                }
            }
        }

        #endregion

        #region 创建机器人测试

        /// <summary>
        /// 【同步方法】创建机器人测试
        /// </summary>
        [TestMethod()]
        public void CreateRobotTest()
        {
            var testName = $"测试机器人_{DateTime.Now:yyyyMMddHHmmss}";
            var result = IntelligentRobotApi.CreateRobot(_appKey, testName, "这是一个自动化测试创建的机器人");
            
            Assert.IsNotNull(result);
            Console.WriteLine($"[同步]创建机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            
            if (result.errcode == Entities.ReturnCode_Work.请求成功)
            {
                Assert.IsNotNull(result.robot_id);
                Assert.IsFalse(string.IsNullOrEmpty(result.robot_id));
                Console.WriteLine($"机器人ID: {result.robot_id}");
                
                // 保存创建的机器人ID供后续测试使用
                _testRobotId = result.robot_id;
            }
        }

        /// <summary>
        /// 【异步方法】创建机器人测试
        /// </summary>
        [TestMethod()]
        public async Task CreateRobotAsyncTest()
        {
            var testName = $"测试机器人_{DateTime.Now:yyyyMMddHHmmss}";
            var result = await IntelligentRobotApi.CreateRobotAsync(_appKey, testName, "这是一个异步测试创建的机器人");
            
            Assert.IsNotNull(result);
            Console.WriteLine($"[异步]创建机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            
            if (result.errcode == Entities.ReturnCode_Work.请求成功)
            {
                Assert.IsNotNull(result.robot_id);
                Assert.IsFalse(string.IsNullOrEmpty(result.robot_id));
                Console.WriteLine($"机器人ID: {result.robot_id}");
            }
        }

        #endregion

        #region 修改机器人测试

        /// <summary>
        /// 【同步方法】修改机器人测试
        /// </summary>
        [TestMethod()]
        public void UpdateRobotTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var newName = $"更新后的机器人_{DateTime.Now:yyyyMMddHHmmss}";
                var result = IntelligentRobotApi.UpdateRobot(_appKey, _testRobotId, newName, "描述已更新");
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]修改机器人: errcode={result.errcode}, errmsg={result.errmsg}");
                Assert.AreEqual(Entities.ReturnCode_Work.请求成功, result.errcode, $"更新失败: {result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【异步方法】修改机器人测试
        /// </summary>
        [TestMethod()]
        public async Task UpdateRobotAsyncTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var newName = $"异步更新的机器人_{DateTime.Now:yyyyMMddHHmmss}";
                var result = await IntelligentRobotApi.UpdateRobotAsync(_appKey, _testRobotId, newName, "异步描述已更新");
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[异步]修改机器人: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        #endregion

        #region 查询机器人详情测试

        /// <summary>
        /// 【同步方法】查询机器人详情测试
        /// </summary>
        [TestMethod()]
        public void GetRobotDetailTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = IntelligentRobotApi.GetRobotDetail(_appKey, _testRobotId);
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]查询机器人: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Assert.AreEqual(_testRobotId, result.robot_id);
                    Assert.IsNotNull(result.name);
                    Console.WriteLine($"机器人名称: {result.name}");
                    Console.WriteLine($"机器人描述: {result.description}");
                    Console.WriteLine($"创建时间: {result.create_time}");
                    Console.WriteLine($"更新时间: {result.update_time}");
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【异步方法】查询机器人详情测试
        /// </summary>
        [TestMethod()]
        public async Task GetRobotDetailAsyncTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = await IntelligentRobotApi.GetRobotDetailAsync(_appKey, _testRobotId);
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[异步]查询机器人: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Assert.AreEqual(_testRobotId, result.robot_id);
                    Assert.IsNotNull(result.name);
                    Console.WriteLine($"机器人名称: {result.name}");
                    Console.WriteLine($"机器人描述: {result.description}");
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        #endregion

        #region 发送机器人消息测试

        /// <summary>
        /// 【同步方法】发送文本消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotTextMessageTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "text",
                    text = new TextMessage
                    {
                        content = "这是一条同步发送的文本测试消息"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送文本消息: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"消息ID: {result.msg_id}");
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【异步方法】发送文本消息测试
        /// </summary>
        [TestMethod()]
        public async Task SendRobotTextMessageAsyncTest()
        {
            // 如果没有测试机器人ID，先创建一个
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
                    text = new TextMessage
                    {
                        content = "这是一条异步发送的文本测试消息"
                    }
                };

                var result = await IntelligentRobotApi.SendRobotMessageAsync(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[异步]发送文本消息: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"消息ID: {result.msg_id}");
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送图片消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotImageMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "image",
                    image = new ImageMessage
                    {
                        media_id = "test_media_id_image"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送图片消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送语音消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotVoiceMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "voice",
                    voice = new VoiceMessage
                    {
                        media_id = "test_media_id_voice"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送语音消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送视频消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotVideoMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "video",
                    video = new VideoMessage
                    {
                        media_id = "test_media_id_video",
                        title = "测试视频",
                        description = "这是一个测试视频"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送视频消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送文件消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotFileMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "file",
                    file = new FileMessage
                    {
                        media_id = "test_media_id_file"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送文件消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送文本卡片消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotTextCardMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "textcard",
                    textcard = new TextCardMessage
                    {
                        title = "测试卡片标题",
                        description = "这是卡片的描述信息",
                        url = "https://work.weixin.qq.com",
                        btntxt = "查看详情"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送文本卡片消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送图文消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotNewsMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
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
                                title = "测试图文标题1",
                                description = "这是第一篇图文的描述",
                                url = "https://work.weixin.qq.com/test1",
                                picurl = "https://example.com/pic1.jpg"
                            },
                            new Article
                            {
                                title = "测试图文标题2",
                                description = "这是第二篇图文的描述",
                                url = "https://work.weixin.qq.com/test2",
                                picurl = "https://example.com/pic2.jpg"
                            }
                        }
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送图文消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】发送Markdown消息测试
        /// </summary>
        [TestMethod()]
        public void SendRobotMarkdownMessageTest()
        {
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var request = new SendRobotMessageRequest
                {
                    robot_id = _testRobotId,
                    userid = "testuser",
                    msgtype = "markdown",
                    markdown = new MarkdownMessage
                    {
                        content = "## 测试Markdown消息\n\n这是一条**测试**消息，包含以下内容：\n\n1. 第一项\n2. 第二项\n3. 第三项\n\n> 这是一条引用"
                    }
                };

                var result = IntelligentRobotApi.SendRobotMessage(_appKey, request);
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]发送Markdown消息: errcode={result.errcode}, errmsg={result.errmsg}");
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        #endregion

        #region 获取聊天记录测试

        /// <summary>
        /// 【同步方法】获取机器人聊天记录测试
        /// </summary>
        [TestMethod()]
        public void GetRobotChatRecordTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = IntelligentRobotApi.GetRobotChatRecord(_appKey, _testRobotId);
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]获取聊天记录: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"记录数量: {result.record_list?.Count ?? 0}");
                    Console.WriteLine($"是否有更多: {result.has_more}");
                    
                    if (result.record_list != null && result.record_list.Count > 0)
                    {
                        foreach (var record in result.record_list)
                        {
                            Console.WriteLine($"  消息ID: {record.msg_id}, 类型: {record.msgtype}, 发送者: {record.from}");
                        }
                    }
                    
                    // 如果有下一页游标，测试分页
                    if (result.has_more && !string.IsNullOrEmpty(result.next_cursor))
                    {
                        Console.WriteLine($"获取下一页，cursor: {result.next_cursor}");
                        var nextPageResult = IntelligentRobotApi.GetRobotChatRecord(_appKey, _testRobotId, result.next_cursor, 50);
                        Assert.IsNotNull(nextPageResult);
                        Console.WriteLine($"第二页记录数量: {nextPageResult.record_list?.Count ?? 0}");
                    }
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【异步方法】获取机器人聊天记录测试
        /// </summary>
        [TestMethod()]
        public async Task GetRobotChatRecordAsyncTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                await CreateRobotAsyncTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                var result = await IntelligentRobotApi.GetRobotChatRecordAsync(_appKey, _testRobotId);
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[异步]获取聊天记录: errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"记录数量: {result.record_list?.Count ?? 0}");
                    Console.WriteLine($"是否有更多: {result.has_more}");
                    
                    if (result.record_list != null && result.record_list.Count > 0)
                    {
                        foreach (var record in result.record_list)
                        {
                            Console.WriteLine($"  消息ID: {record.msg_id}, 类型: {record.msgtype}");
                        }
                    }
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        /// <summary>
        /// 【同步方法】获取机器人聊天记录（带分页参数）测试
        /// </summary>
        [TestMethod()]
        public void GetRobotChatRecordWithPaginationTest()
        {
            // 如果没有测试机器人ID，先创建一个
            if (string.IsNullOrEmpty(_testRobotId))
            {
                CreateRobotTest();
            }

            if (!string.IsNullOrEmpty(_testRobotId))
            {
                // 测试第一页，限制返回10条
                var result = IntelligentRobotApi.GetRobotChatRecord(_appKey, _testRobotId, cursor: null, limit: 10);
                
                Assert.IsNotNull(result);
                Console.WriteLine($"[同步]获取聊天记录(分页): errcode={result.errcode}, errmsg={result.errmsg}");
                
                if (result.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    var recordCount = result.record_list?.Count ?? 0;
                    Console.WriteLine($"第一页记录数量: {recordCount}");
                    Assert.IsTrue(recordCount <= 10, "返回的记录数应该不超过限制值");
                    
                    Console.WriteLine($"是否有更多记录: {result.has_more}");
                    if (!string.IsNullOrEmpty(result.next_cursor))
                    {
                        Console.WriteLine($"下一页游标: {result.next_cursor}");
                    }
                }
            }
            else
            {
                Assert.Inconclusive("无法获取测试机器人ID");
            }
        }

        #endregion

        #region 删除机器人测试

        /// <summary>
        /// 【同步方法】删除机器人测试
        /// </summary>
        [TestMethod()]
        public void DeleteRobotTest()
        {
            // 创建一个专门用于删除测试的机器人
            var testName = $"待删除测试机器人_{DateTime.Now:yyyyMMddHHmmss}";
            var createResult = IntelligentRobotApi.CreateRobot(_appKey, testName, "这是一个将被删除的测试机器人");
            
            if (createResult.errcode == Entities.ReturnCode_Work.请求成功 && !string.IsNullOrEmpty(createResult.robot_id))
            {
                Console.WriteLine($"已创建测试机器人，ID: {createResult.robot_id}");
                
                // 删除刚创建的机器人
                var deleteResult = IntelligentRobotApi.DeleteRobot(_appKey, createResult.robot_id);
                
                Assert.IsNotNull(deleteResult);
                Console.WriteLine($"[同步]删除机器人: errcode={deleteResult.errcode}, errmsg={deleteResult.errmsg}");
                
                if (deleteResult.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"成功删除机器人: {createResult.robot_id}");
                }
            }
            else
            {
                Assert.Inconclusive($"无法创建测试机器人进行删除测试: {createResult.errmsg}");
            }
        }

        /// <summary>
        /// 【异步方法】删除机器人测试
        /// </summary>
        [TestMethod()]
        public async Task DeleteRobotAsyncTest()
        {
            // 创建一个专门用于删除测试的机器人
            var testName = $"待删除测试机器人_{DateTime.Now:yyyyMMddHHmmss}";
            var createResult = await IntelligentRobotApi.CreateRobotAsync(_appKey, testName, "这是一个将被异步删除的测试机器人");
            
            if (createResult.errcode == Entities.ReturnCode_Work.请求成功 && !string.IsNullOrEmpty(createResult.robot_id))
            {
                Console.WriteLine($"已创建测试机器人，ID: {createResult.robot_id}");
                
                // 删除刚创建的机器人
                var deleteResult = await IntelligentRobotApi.DeleteRobotAsync(_appKey, createResult.robot_id);
                
                Assert.IsNotNull(deleteResult);
                Console.WriteLine($"[异步]删除机器人: errcode={deleteResult.errcode}, errmsg={deleteResult.errmsg}");
                
                if (deleteResult.errcode == Entities.ReturnCode_Work.请求成功)
                {
                    Console.WriteLine($"成功删除机器人: {createResult.robot_id}");
                }
            }
            else
            {
                Assert.Inconclusive($"无法创建测试机器人进行删除测试: {createResult.errmsg}");
            }
        }

        #endregion
    }
}
