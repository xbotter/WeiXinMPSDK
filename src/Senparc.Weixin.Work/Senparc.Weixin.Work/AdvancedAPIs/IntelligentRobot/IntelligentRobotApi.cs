/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：IntelligentRobotApi.cs
    文件功能描述：企业微信智能机器人接口
    
    
    创建标识：Senparc - 20260224
    创建描述：实现智能机器人相关接口，包括创建、修改、查询、删除、发送消息、获取聊天记录等功能
    
    官方文档参考：
    https://developer.work.weixin.qq.com/document/path/100719 - 获取企业内机器人列表
    https://developer.work.weixin.qq.com/document/path/101027 - 创建机器人
    https://developer.work.weixin.qq.com/document/path/101031 - 修改机器人
    https://developer.work.weixin.qq.com/document/path/101032 - 查询机器人
    https://developer.work.weixin.qq.com/document/path/101033 - 删除机器人
    https://developer.work.weixin.qq.com/document/path/100989 - 发送机器人消息
    https://developer.work.weixin.qq.com/document/path/101028 - 接收机器人消息
    https://developer.work.weixin.qq.com/document/path/101029 - 接收机器人事件
    https://developer.work.weixin.qq.com/document/path/101138 - 获取机器人聊天记录
    https://developer.work.weixin.qq.com/document/path/101039 - 智能机器人消息推送

----------------------------------------------------------------*/

using Senparc.CO2NET.Extensions;
using Senparc.NeuChar;
using Senparc.Weixin.CommonAPIs;
using Senparc.Weixin.Entities;
using System.Threading.Tasks;

namespace Senparc.Weixin.Work.AdvancedAPIs.IntelligentRobot
{
    /// <summary>
    /// 企业微信智能机器人接口
    /// </summary>
    [NcApiBind(NeuChar.PlatformType.WeChat_Work, true)]
    public static class IntelligentRobotApi
    {
        #region 异步方法

        /// <summary>
        /// 【异步方法】获取企业内机器人列表
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<GetRobotListResult> GetRobotListAsync(string accessTokenOrAppKey, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/list?access_token={0}", accessToken.AsUrlData());
                return await CommonJsonSend.SendAsync<GetRobotListResult>(null, url, null, CommonJsonSendType.GET, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        /// <summary>
        /// 【异步方法】创建机器人
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="name">机器人名称</param>
        /// <param name="description">机器人描述</param>
        /// <param name="avatar_mediaid">机器人头像MediaId</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<CreateRobotResult> CreateRobotAsync(string accessTokenOrAppKey, string name, string description = null, string avatar_mediaid = null, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/create?access_token={0}", accessToken.AsUrlData());
                var data = new
                {
                    name,
                    description,
                    avatar_mediaid
                };
                return await CommonJsonSend.SendAsync<CreateRobotResult>(accessToken, url, data, CommonJsonSendType.POST, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        /// <summary>
        /// 【异步方法】修改机器人
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="robot_id">机器人ID</param>
        /// <param name="name">机器人名称</param>
        /// <param name="description">机器人描述</param>
        /// <param name="avatar_mediaid">机器人头像MediaId</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<WorkJsonResult> UpdateRobotAsync(string accessTokenOrAppKey, string robot_id, string name = null, string description = null, string avatar_mediaid = null, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/update?access_token={0}", accessToken.AsUrlData());
                var data = new
                {
                    robot_id,
                    name,
                    description,
                    avatar_mediaid
                };
                return await CommonJsonSend.SendAsync<WorkJsonResult>(accessToken, url, data, CommonJsonSendType.POST, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        /// <summary>
        /// 【异步方法】查询机器人详情
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="robot_id">机器人ID</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<GetRobotDetailResult> GetRobotDetailAsync(string accessTokenOrAppKey, string robot_id, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/get?access_token={0}&robot_id={1}", accessToken.AsUrlData(), robot_id.AsUrlData());
                return await CommonJsonSend.SendAsync<GetRobotDetailResult>(null, url, null, CommonJsonSendType.GET, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        /// <summary>
        /// 【异步方法】删除机器人
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="robot_id">机器人ID</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<WorkJsonResult> DeleteRobotAsync(string accessTokenOrAppKey, string robot_id, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/delete?access_token={0}", accessToken.AsUrlData());
                var data = new
                {
                    robot_id
                };
                return await CommonJsonSend.SendAsync<WorkJsonResult>(accessToken, url, data, CommonJsonSendType.POST, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        /// <summary>
        /// 【异步方法】发送机器人消息
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="data">发送消息数据</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<SendRobotMessageResult> SendRobotMessageAsync(string accessTokenOrAppKey, SendRobotMessageRequest data, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/send?access_token={0}", accessToken.AsUrlData());
                return await CommonJsonSend.SendAsync<SendRobotMessageResult>(accessToken, url, data, CommonJsonSendType.POST, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        /// <summary>
        /// 【异步方法】获取机器人聊天记录
        /// </summary>
        /// <param name="accessTokenOrAppKey">调用接口凭证（AccessToken）或AppKey（根据AccessTokenContainer.BuildingKey(corpId, corpSecret)方法获得）</param>
        /// <param name="robot_id">机器人ID</param>
        /// <param name="cursor">分页游标，第一次请求不填，后续请求填写上次返回的next_cursor</param>
        /// <param name="limit">返回的最大记录数，整型，最大值100，默认值100</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<GetRobotChatRecordResult> GetRobotChatRecordAsync(string accessTokenOrAppKey, string robot_id, string cursor = null, int limit = 100, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var url = string.Format(Config.ApiWorkHost + "/cgi-bin/robot/chat/record?access_token={0}", accessToken.AsUrlData());
                var data = new
                {
                    robot_id,
                    cursor,
                    limit
                };
                return await CommonJsonSend.SendAsync<GetRobotChatRecordResult>(accessToken, url, data, CommonJsonSendType.POST, timeOut).ConfigureAwait(false);
            }, accessTokenOrAppKey).ConfigureAwait(false);
        }

        #endregion
    }
}
