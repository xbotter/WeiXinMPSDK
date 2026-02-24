# 企业微信智能机器人 Webhook 消息处理指南

## 概述

BotMessageHelper 提供了一个简洁的方式来处理企业微信智能机器人的 Webhook 消息推送。无需继承复杂的 MessageHandler 基类，只需使用静态辅助方法即可完成消息的接收、解析和响应。

## 官方文档

- [接收机器人消息](https://developer.work.weixin.qq.com/document/path/101028)
- [接收机器人事件](https://developer.work.weixin.qq.com/document/path/101029)
- [智能机器人消息推送](https://developer.work.weixin.qq.com/document/path/101039)

## 支持的消息类型

### 普通消息
- **文本消息** (`BotRequestMessageText`) - 用户发送的文本内容
- **图片消息** (`BotRequestMessageImage`) - 用户发送的图片
- **混合消息** (`BotRequestMessageMixed`) - 文本+图片组合
- **流消息** (`BotRequestMessageStream`) - 流式消息

### 事件消息
- **进入对话事件** (`BotRequestMessageEvent_Enter`) - 用户首次进入机器人对话
- **模板卡片事件** (`BotRequestMessageEvent_TemplateCardEvent`) - 模板卡片交互事件

## 快速开始

### 1. 在 ASP.NET Core 控制器中接收 Webhook

```csharp
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.Helpers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/bot")]
public class BotWebhookController : ControllerBase
{
    private readonly IConfiguration _configuration;

    [HttpPost("webhook")]
    public IActionResult ReceiveMessage(
        [FromQuery] string msg_signature,
        [FromQuery] string timestamp,
        [FromQuery] string nonce)
    {
        try
        {
            // 构建 PostModel（从配置中读取）
            var postModel = new PostModel
            {
                Msg_Signature = msg_signature,
                Timestamp = timestamp,
                Nonce = nonce,
                Token = _configuration["WeChat:Robot:Token"],
                EncodingAESKey = _configuration["WeChat:Robot:EncodingAESKey"],
                CorpId = _configuration["WeChat:Robot:CorpId"]
            };

            // 解析消息（自动处理解密）
            var requestMessage = BotMessageHelper.ParseBotMessage(Request.Body, postModel);
            
            // 处理消息
            var responseContent = ProcessMessage(requestMessage);
            
            // 生成并加密响应
            var encryptedResponse = BotMessageHelper.CreateAndEncryptTextResponse(
                requestMessage,
                responseContent,
                postModel
            );
            
            return Content(encryptedResponse, "text/xml");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理机器人消息失败");
            return StatusCode(500);
        }
    }

    private string ProcessMessage(WorkBotRequestMessageBase message)
    {
        // 根据消息类型分发处理
        return message switch
        {
            BotRequestMessageText textMsg => HandleTextMessage(textMsg),
            BotRequestMessageImage imageMsg => "已收到您的图片",
            BotRequestMessageEvent_Enter => "欢迎使用智能机器人！我能帮您做什么？",
            _ => "已收到您的消息"
        };
    }

    private string HandleTextMessage(BotRequestMessageText message)
    {
        var content = message.Content;
        
        // 添加您的业务逻辑
        if (content.Contains("天气"))
        {
            return "今天天气晴朗，温度 25°C";
        }
        else if (content.Contains("帮助"))
        {
            return @"我可以为您提供以下服务：
1. 查询天气
2. 日程提醒
3. 信息查询
4. 常见问题解答";
        }
        
        return $"您说：{content}\n我已收到您的消息。";
    }
}
```

### 2. 配置文件 (appsettings.json)

```json
{
  "WeChat": {
    "Robot": {
      "Token": "your_token_here",
      "EncodingAESKey": "your_encoding_aes_key_here",
      "CorpId": "your_corp_id_here"
    }
  }
}
```

### 3. 在企业微信后台配置 Webhook URL

1. 进入企业微信管理后台
2. 应用管理 -> 智能机器人
3. 配置接收消息服务器：`https://yourdomain.com/api/bot/webhook`
4. 配置 Token 和 EncodingAESKey

## 高级用法

### 处理不同的会话类型

```csharp
private string ProcessMessage(WorkBotRequestMessageBase message)
{
    if (message is BotRequestMessageText textMsg)
    {
        // 根据会话类型（单聊/群聊）返回不同内容
        if (textMsg.ChatType == ChatType.Group)
        {
            return $"[群聊消息] {ProcessGroupMessage(textMsg)}";
        }
        else
        {
            return $"[单聊消息] {ProcessPrivateMessage(textMsg)}";
        }
    }
    
    return "已收到";
}
```

### 处理混合消息（文本+图片）

```csharp
if (requestMessage is BotRequestMessageMixed mixedMsg)
{
    var textContent = mixedMsg.Text?.Content ?? "";
    var imageCount = mixedMsg.ImageList?.Count ?? 0;
    
    return $"已收到您的消息：{textContent}\n包含 {imageCount} 张图片";
}
```

### 手动控制加密过程

```csharp
// 解析消息
var requestMessage = BotMessageHelper.ParseBotMessage(Request.Body, postModel);

// 处理消息
var responseContent = "响应内容";

// 分步创建和加密
var responseXml = BotMessageHelper.CreateTextResponse(requestMessage, responseContent);
var encryptedXml = BotMessageHelper.EncryptResponse(responseXml, postModel);

return Content(encryptedXml, "text/xml");
```

## API 方法说明

### ParseBotMessage
解析接收到的 Webhook 消息（支持自动解密）

```csharp
WorkBotRequestMessageBase ParseBotMessage(Stream inputStream, PostModel postModel)
```

**参数：**
- `inputStream` - HTTP 请求的输入流
- `postModel` - 包含 Token、EncodingAESKey、CorpId 等配置

**返回：**
- 强类型的消息对象（BotRequestMessageText、BotRequestMessageImage等）

### CreateTextResponse
创建文本响应消息的 XML

```csharp
string CreateTextResponse(WorkBotRequestMessageBase requestMessage, string content)
```

**参数：**
- `requestMessage` - 接收到的请求消息
- `content` - 要响应的文本内容

**返回：**
- 响应消息的 XML 字符串

### EncryptResponse
加密响应消息

```csharp
string EncryptResponse(string responseXml, PostModel postModel)
```

**参数：**
- `responseXml` - 响应消息的 XML 字符串
- `postModel` - 包含加密配置的 PostModel

**返回：**
- 加密后的 XML 字符串

### CreateAndEncryptTextResponse
一步完成创建和加密

```csharp
string CreateAndEncryptTextResponse(
    WorkBotRequestMessageBase requestMessage, 
    string content, 
    PostModel postModel)
```

## 最佳实践

### 1. 使用依赖注入管理配置

```csharp
public class BotWebhookService
{
    private readonly PostModel _postModel;

    public BotWebhookService(IConfiguration configuration)
    {
        _postModel = new PostModel
        {
            Token = configuration["WeChat:Robot:Token"],
            EncodingAESKey = configuration["WeChat:Robot:EncodingAESKey"],
            CorpId = configuration["WeChat:Robot:CorpId"]
        };
    }

    public string ProcessWebhook(Stream inputStream, string msgSignature, string timestamp, string nonce)
    {
        _postModel.Msg_Signature = msgSignature;
        _postModel.Timestamp = timestamp;
        _postModel.Nonce = nonce;

        var requestMessage = BotMessageHelper.ParseBotMessage(inputStream, _postModel);
        var response = HandleMessage(requestMessage);
        return BotMessageHelper.CreateAndEncryptTextResponse(requestMessage, response, _postModel);
    }
}
```

### 2. 添加日志记录

```csharp
_logger.LogInformation("收到机器人消息，类型：{MsgType}，来自：{FromUser}", 
    requestMessage.MsgType, 
    requestMessage.FromUserName);
```

### 3. 异常处理

```csharp
try
{
    var requestMessage = BotMessageHelper.ParseBotMessage(Request.Body, postModel);
    // ... 处理逻辑
}
catch (Exception ex)
{
    _logger.LogError(ex, "解析机器人消息失败");
    return StatusCode(500, "消息处理失败");
}
```

## 与 IntelligentRobotApi 配合使用

BotMessageHelper（接收消息）和 IntelligentRobotApi（主动发送消息）可以配合使用：

```csharp
// 接收用户消息
var requestMessage = BotMessageHelper.ParseBotMessage(Request.Body, postModel);

if (requestMessage is BotRequestMessageText textMsg)
{
    // 通过 Webhook 立即响应
    var quickResponse = "我已收到您的请求，正在处理...";
    var encryptedResponse = BotMessageHelper.CreateAndEncryptTextResponse(
        requestMessage, 
        quickResponse, 
        postModel
    );
    
    // 异步处理复杂逻辑
    _ = Task.Run(async () =>
    {
        var detailedResponse = await ProcessComplexRequest(textMsg.Content);
        
        // 通过 API 主动发送详细响应
        var sendRequest = new SendRobotMessageRequest
        {
            robot_id = requestMessage.ToUserName,
            userid = requestMessage.FromUserName,
            msgtype = "text",
            text = new TextMessage { content = detailedResponse }
        };
        
        await IntelligentRobotApi.SendRobotMessageAsync(accessToken, sendRequest);
    });
    
    return Content(encryptedResponse, "text/xml");
}
```

## 注意事项

1. **安全性**：Token 和 EncodingAESKey 必须妥善保管，不要提交到代码仓库
2. **超时**：Webhook 响应需要在 5 秒内返回，复杂逻辑应异步处理
3. **消息去重**：使用 MsgId 字段进行消息去重，避免重复处理
4. **错误处理**：做好异常捕获，避免返回 500 错误导致企业微信重试

## 测试

运行单元测试：
```bash
dotnet test --filter "FullyQualifiedName~BotMessageHelperTest"
```

## 相关链接

- [IntelligentRobotApi 文档](../AdvancedAPIs/IntelligentRobot/README.md) - 主动发送消息API
- [企业微信官方文档](https://developer.work.weixin.qq.com/document/path/101039)
