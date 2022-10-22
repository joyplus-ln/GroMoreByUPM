using System;
using System.Collections.Generic;
using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class RewardVideoAdExample
    {
        // 激励视频
        public ABURewardVideoAd rewardAd;
        // 广告加载成功标志，需要在加载成功后展示
        public bool rewardVideoAdLoadSuccess = false; 

        /// <summary>
        /// Load the reward Ad Horizontal.
        /// 加载横评个性化激励视屏.
        /// </summary>
        public void LoadExpressRewardAdH()
        {
            Debug.Log("<Unity Log>..." + "Load ExpressReward Horizontal");
            string unitID = ABUAdPositionId.REWARD_VIDEO_EXPRESS_H_CODE;
            this.LoadRewardAd(unitID, true);
        }

        /// <summary>
        /// Loa the reward Ad Vertical.
        /// 加载个性化模板竖屏激励视频.
        /// </summary>
        public void LoadExpressRewardAdV()
        {
            Debug.Log("<Unity Log>..." + "Load ExpressReward Vertical");
            string unitID = ABUAdPositionId.REWARD_VIDEO_EXPRESS_V_CODE;
            this.LoadRewardAd(unitID, true);
        }

        /// <summary>
        /// Loa the reward Ad Vertical.
        /// 加载竖屏激励视频.
        /// </summary>
        public void LoadNormalRewardAdV()
        {
            Debug.Log("<Unity Log>..." + "Load NormalReward Vertical");
            string unitID = ABUAdPositionId.REWARD_VIDEO_NORMAL_CODE;
            this.LoadRewardAd(unitID, false);
        }

        public void LoadRewardAd(string unitID, bool getExpress)
        {
            var adSlot = new ABUAdUnit.Builder()
            .SetCodeId(unitID)
                .SetImageAcceptedSize(1080, 1920)
                .SetRewardName("金币") // 奖励的名称
                .SetRewardAmount(3) // 奖励的数量
                .SetUserID("user123") // 用户id,必传参数   只对穿山甲adn有效
                .SetMediaExtra("media_extra") // 附加参数，可选    只对穿山甲adn有效
                .SetExpressIfCan(getExpress) // 加载模板需要设置为true 加载模板时需要设置为Yes
                .SetTTVideoOption(ABUAdUnit.getRewardVideoTTVideoOption(true))
                .Build();
            ABURewardVideoAd.LoadRewardVideoAd(adSlot, new RewardVideoAdListener(this));
        }

        /// <summary>
        /// Show the reward Ad.
        /// </summary>
        public void ShowRewardAd()
        {
            // 为保障播放流畅，建议在视频加载完成后展示
            if (!rewardVideoAdLoadSuccess || !ABURewardVideoAd.isReady())
            {
                var msg = "请先加载广告或等广告加载完成";
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
            // 普通展示方式
            ABURewardVideoAd.ShowRewardVideoAd(new RewardAdInteractionListener(this));
            // 带scene的展示方式
            //ABURewardVideoAd.ShowRewardVideoAdWithRitScene(new RewardAdInteractionListener(this), ritSceneMap);
            rewardVideoAdLoadSuccess = false;
        }

    }

    public sealed class RewardVideoAdListener : ABURewardVideoAdCallback
    {
        private RewardVideoAdExample example;

        public RewardVideoAdListener(RewardVideoAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "OnRewardVideoAdLoadError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnRewardVideoAdLoad(object ad)
        {
            ToastManager.Instance.ShowToast("OnRewardVideoAdLoad");
            Debug.Log("<Unity Log>..." + "OnRewardVideoAdLoad");
        }

        public void OnRewardVideoAdCached()
        {
            Debug.Log("<Unity Log>..." + "OnRewardVideoCached");
            ToastManager.Instance.ShowToast("OnRewardVideoCached");
            this.example.rewardVideoAdLoadSuccess = true;
        }
    }

    public sealed class RewardAdInteractionListener : ABURewardAdInteractionCallback
    {
        private RewardVideoAdExample example;

        public RewardAdInteractionListener(RewardVideoAdExample example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("<Unity Log>..." + "expressRewardAd show");
            ToastManager.Instance.ShowToast("expressRewardAd show");
            this.example.rewardVideoAdLoadSuccess = false;
            string ecpm = ABURewardVideoAd.GetPreEcpm();
            string ritID = ABURewardVideoAd.GetAdNetworkRitId();
            string adnName = ABURewardVideoAd.GetAdRitInfoAdnName();
            Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);

        }

        public void OnViewRenderFail(int code, string message)
        {
            var s = "code : " + code + "--message = " + message;
            Log.D("<Unity Log>..." + s);
            ToastManager.Instance.ShowToast(s);
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("<Unity Log>..." + "expressRewardAd bar click");
            ToastManager.Instance.ShowToast("expressRewardAd bar click");
        }

        public void OnAdClose()
        {
            Debug.Log("<Unity Log>..." + "expressRewardAd close");
            ToastManager.Instance.ShowToast("expressRewardAd close");
            this.example.rewardVideoAdLoadSuccess = false;
        }

        public void OnVideoComplete()
        {
            Debug.Log("<Unity Log>..." + "expressRewardAd complete");
            ToastManager.Instance.ShowToast("expressRewardAd complete");
        }

        public void OnVideoError(int errCode, string errMsg)
        {
            string logs = " < Unity Log > ..." + "play error code:" + errCode + ",errMsg:" + errMsg;
            Debug.LogError(logs);
            ToastManager.Instance.ShowToast(logs);
        }

        public void OnRewardVerify(bool rewardVerify)
        {
            var message = "verify:" + rewardVerify;
            Debug.Log("<Unity Log>..." + message);
            ToastManager.Instance.ShowToast(message);
        }

        public void OnSkippedVideo()
        {
            var message = "expressrewardAd OnSkippedVideo for Android";
            Debug.Log("<Unity Log>..." + message);
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
}
