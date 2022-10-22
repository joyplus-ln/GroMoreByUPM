using System;
using System.Collections.Generic;
using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class FullscreenAdExample
    {
        // 全屏视频广告对象
        public ABUFullScreenVideoAd fullScreenVideoAd;
        // 广告加载成功标志，需要在加载成功后展示
        public bool fullVideoAdLoadSuccess = false;
        
        /// <summary>
        /// Loads the full screen video ad Vertical.
        /// </summary>
        public void LoadExpressFullscreenV()
        {
            string unitID = ABUAdPositionId.FULL_SCREEN_VIDEO_EXPRESS_V_CODE;
            this.LoadFullscreenAd(unitID, true);
        }

        /// <summary>
        /// Loads the full screen video ad.
        /// </summary>
        /// <param name="orientation">Orientation.</param>
        /// <param name="codeID">Code identifier.</param>
        public void LoadExpressFullscreenH()
        {
            string unitID = ABUAdPositionId.FULL_SCREEN_VIDEO_EXPRESS_H_CODE;
            this.LoadFullscreenAd(unitID, true);
        }

        /// <summary>
        /// Loads the full screen video ad.
        /// </summary>
        /// <param name="orientation">Orientation.</param>
        /// <param name="codeID">Code identifier.</param>
        public void LoadNormalFullscreenV()
        {
            string unitID = ABUAdPositionId.FULL_SCREEN_VIDEO_NORMAL_V_CODE;
            this.LoadFullscreenAd(unitID, false);
        }

        public void LoadFullscreenAd(string unitID, bool useExpress2IfCanForGDT)
        {
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(unitID)
            .SetUseExpress2IfCanForGDT(useExpress2IfCanForGDT)//iOS:waterfall内gdt是否为模板2.0，客户端配置是waterfall级别的；但是SDK会优先以服务端rit级别的配置为准
            .SetImageAcceptedSize(1080, 1920)
            .SetTTVideoOption(ABUAdUnit.getFullVideoOrFeedTTVideoOption(useExpress2IfCanForGDT, false))//Android waterfall内gdt是否为模板2.0，客户端配置是waterfall级别的；但是SDK会优先以服务端rit级别的配置为准
            .Build();
            ABUFullScreenVideoAd.LoadFullScreenVideoAd(adUnit, new FullScreenVideoAdCallback(this));
        }

        /// <summary>
        /// Show the reward Ad.
        /// 展示全屏视频
        /// </summary>
        public void ShowFullscreenAd()
        {
            //// 为保障播放流畅，建议在视频加载完成后展示
            if (!fullVideoAdLoadSuccess || !ABUFullScreenVideoAd.isReady())
            {
                var msg = "请先加载广告或等待广告加载完成";
                Debug.Log("<Unity Log>..." + msg);
                ToastManager.Instance.ShowToast(msg);
                return;
            }
            // ritScene信息
            // 设置已定义的场景
            Dictionary<string, object> ritSceneMap = new Dictionary<string, object>();
            // ABUShowExtroInfoKeySceneType设置为非BURitSceneType_custom即可
            ritSceneMap.Add(ABUConstantHelper.ABUShowExtroInfoKeySceneType, ABURitSceneType.ABURitSceneType_game_finish_rewards);
            // 设置自定义的场景
            Dictionary<string, object> ritSceneMap_custom = new Dictionary<string, object>();
            // ABUShowExtroInfoKeySceneType设置为BURitSceneType_custom即可
            ritSceneMap_custom.Add(ABUConstantHelper.ABUShowExtroInfoKeySceneType, ABURitSceneType.BURitSceneType_custom);
            // 同时请务必设置
            ritSceneMap_custom.Add(ABUConstantHelper.ABUShowExtroInfoKeySceneDescription, "custom info");
            // 普通展示接口
            //ABUFullScreenVideoAd.ShowFullVideoAd(new FullScreenVideoAdInteractionCallback(this));
            // 带ritScene接口
            ABUFullScreenVideoAd.ShowFullVideoAdWithRitScene(new FullScreenVideoAdInteractionCallback(this), ritSceneMap_custom);
        }
    }

    public sealed class FullScreenVideoAdInteractionCallback : ABUFullScreenVideoAdInteractionCallback
    {
        private FullscreenAdExample example;

        public FullScreenVideoAdInteractionCallback(FullscreenAdExample example)
        {
            this.example = example;
        }

        public void OnViewRenderFail(int code, string message)
        {
            var s = "code : " + code + "--message = " + message;
            Log.D("<Unity Log>..." + s);
            ToastManager.Instance.ShowToast(s);
        }

        public void OnAdShow()
        {
            Debug.Log("<Unity Log>..." + "FullScreenVideoAd show");
            ToastManager.Instance.ShowToast("FullScreenVideoAd show");
            this.example.fullVideoAdLoadSuccess = false;
            string ecpm = ABUFullScreenVideoAd.GetPreEcpm();
            string ritID = ABUFullScreenVideoAd.GetAdNetworkRitId();
            string adnName = ABUFullScreenVideoAd.GetAdRitInfoAdnName();
            Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("<Unity Log>..." + "FullScreenVideoAd bar click");
            ToastManager.Instance.ShowToast("FullScreenVideoAd bar click");
        }

        public void OnAdClose()
        {
            Debug.Log("<Unity Log>..." + "FullScreenVideoAd close");
            ToastManager.Instance.ShowToast("FullScreenVideoAd close");
            this.example.fullVideoAdLoadSuccess = false;
        }

        public void OnVideoComplete()
        {
            Debug.Log("<Unity Log>..." + "FullScreenVideoAd complete");
            ToastManager.Instance.ShowToast("FullScreenVideoAd complete");
        }

        public void OnVideoError(int errCode, string errMsg)
        {
            string logs = " < Unity Log > ..." + "play error code:" + errCode + ",errMsg:" + errMsg;
            Debug.LogError(logs);
            ToastManager.Instance.ShowToast(logs);
        }

        public void OnSkippedVideo()
        {
            var message = "FullScreenVideoAd OnSkippedVideo for Android";
            Debug.Log(" <Unity Log> ..." + message);
            ToastManager.Instance.ShowToast(message);
        }

        /// <summary>
        /// Ons the other rit  in waterfall occur filll error.Call back after show.
        /// fillFailMessageInfo:Json string whose outer layer is an array,and the array elements are dictionaries.
        /// The keys of Internal dictionary are the following:
        /// 1."mediation_rit": 广告代码位
        /// 2.@"adn_name": 属于哪家广告adn
        /// 3."error_message": 错误信息
        /// 4."error_code": 错误码
        /// </summary>
        public void OnWaterfallRitFillFail(string fillFailMessageInfo)
        {
            Debug.Log("<Unity Log>...fillFailMessageInfo:" + fillFailMessageInfo);
        }

        /// <summary>
        /// Fail to show ad.Now only for iOS.
        /// errcode 错误码
        /// errorMsg 错误描述
        /// </summary>
        public void OnAdShowFailed(int errcode, string errorMsg)
        {
            Debug.Log("<Unity Log>...OnAdShowFailed Errcode:" + errcode + ", errMsg:" + errorMsg);
        }

        public void OnRewardVerify(bool rewardVerify, ABUAdapterRewardAdInfo rewardInfo)
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnRewardVerify"
                + ", rewardName : " + rewardInfo.rewardName// 发放奖励的名称
                + ", rewardAmount : " + rewardInfo.rewardAmount// 发放奖励的金额
                + ", tradeId : " + rewardInfo.tradeId// 交易的唯一标识
                + ", verify : " + rewardInfo.verify// 是否验证通过
                + ", verifyByGroMoreS2S : " + rewardInfo.verifyByGroMoreS2S// 是否是通过GroMore的S2S的验证
                + ", adnName : " + rewardInfo.adnName// 验证奖励发放的媒体名称，官方支持的ADN名称详见`ABUAdnType`注释部分，自定义ADN名称同平台配置
                + ", reason : " + rewardInfo.reason// 验证失败的原因
                + ", errorCode : " + rewardInfo.errorCode// 无法完成验证的错误码
                + ", errorMsg : " + rewardInfo.errorMsg// 无法完成验证的错误原因，包括网络错误、服务端无响应、服务端无法验证等
                + ", rewardType : " + rewardInfo.rewardType// 奖励类型，0:基础奖励 1:进阶奖励-互动 2:进阶奖励-超过30s的视频播放完成  目前支持返回该字段的adn：csj
                + ", rewardPropose : " + rewardInfo.rewardPropose);// 建议奖励百分比， 基础奖励为1，进阶奖励为0.0 ~ 1.0，开发者自行换算  目前支持返回该字段的adn：csj
        }
    }

    public sealed class FullScreenVideoAdCallback : ABUFullScreenVideoAdCallback
    {
        private FullscreenAdExample example;

        public FullScreenVideoAdCallback(FullscreenAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "OnFullScreenError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnFullScreenVideoAdLoad(object ad)
        {
            ToastManager.Instance.ShowToast("OnFullScreenVideoAdLoad");
            Debug.Log("<Unity Log>..." + "OnFullScreenVideoAdLoad");
        }

        public void OnFullScreenVideoAdCached()
        {
            Debug.Log("<Unity Log>..." + "OnFullScreenVideoAdCached");
            ToastManager.Instance.ShowToast("OnFullScreenVideoAdCached");
            this.example.fullVideoAdLoadSuccess = true;
        }
    }
}
