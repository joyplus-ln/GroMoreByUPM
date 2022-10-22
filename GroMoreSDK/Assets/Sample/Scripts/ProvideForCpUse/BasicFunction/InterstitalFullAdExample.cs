using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class InterstitalFullAdExample
    {
        // 广告加载成功标志，需要在加载成功后展示
        public bool interstitialFullAdLoadSuccess = false;

        /// <summary>
        ///  加载插全屏广告
        /// </summary>
        public void LoadInterstitalFullAd(string unitID)
        {
            var adUnit = new ABUAdUnit.Builder()
                .SetCodeId(unitID)
                .SetImageAcceptedSize(414, 300) // 针对插屏(非全屏)期望的广告尺寸
                .SetMutedIfCan(false)
                .Build();
            ABUInterstitialFullAd.LoadInterstitialFullAd(adUnit, new InterstitialFullAdCallback(this));
        }

        /// <summary>
        ///  展示插全屏广告
        /// </summary>
        public void ShowInterstitalFullAd()
        {
            if (!interstitialFullAdLoadSuccess || !ABUInterstitialFullAd.isReady())
            {
                var msg = "请先加载广告或等待广告加载完成";
                Debug.Log("<Unity Log>..." + msg);
                ToastManager.Instance.ShowToast(msg);
                return;
            }
            ABUInterstitialFullAd.ShowInteractionAd(new InterstitialFullAdInteractionCallback(this));
        }
    }

    public sealed class InterstitialFullAdCallback : ABUInterstitialFullAdCallback
    {
        private InterstitalFullAdExample example;

        public InterstitialFullAdCallback(InterstitalFullAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnError"
                + ", code" + code
                + ", message" + message
                );
            ToastManager.Instance.ShowToast("InterstitialFullAd OnError"
                + ", code" + code
                + ", message" + message);
        }

        public void OnInterstitialFullAdCached()
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnInterstitialFullAdCached");
        }

        public void OnInterstitialFullAdLoad(object ad)
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnInterstitialFullAdLoad");
            ToastManager.Instance.ShowToast("InterstitialFullAd OnInterstitialFullAdLoad");
            this.example.interstitialFullAdLoadSuccess = true;
        }
    }

    public sealed class InterstitialFullAdInteractionCallback : ABUInterstitialFullAdInteractionCallback
    {
        private InterstitalFullAdExample example;

        public InterstitialFullAdInteractionCallback(InterstitalFullAdExample example)
        {
            this.example = example;
        }

        public void OnAdClicked()
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnAdClicked");
            ToastManager.Instance.ShowToast("InterstitialFullAd OnAdClicked");
        }

        public void OnAdClose()
        {
            this.example.interstitialFullAdLoadSuccess = false;

            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnAdClose");
        }

        public void OnAdShow()
        {
            this.example.interstitialFullAdLoadSuccess = false;

            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnAdShow");
            ToastManager.Instance.ShowToast("InterstitialFullAd OnAdShow");

            string ecpm = ABUInterstitialFullAd.GetPreEcpm();
            string ritID = ABUInterstitialFullAd.GetAdNetworkRitId();
            string adnName = ABUInterstitialFullAd.GetAdRitInfoAdnName();
            Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
        }

        public void OnAdShowFailed(int errcode, string errorMsg)
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnAdShowFailed"
                + ", errcode" + errcode
                + ", errorMsg" + errorMsg
                );
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

        public void OnSkippedVideo()
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnSkippedVideo");
        }

        public void OnVideoComplete()
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnVideoComplete");
        }

        public void OnViewRenderFail(int code, string message)
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnViewRenderFail"
                + ", code" + code
                + ", message" + message);
        }

        public void OnWaterfallRitFillFail(string fillFailMessageInfo)
        {
            Debug.Log("<Unity Log>..." + "InterstitialFullAd OnWaterfallRitFillFail"
                + ", fillFailMessageInfo" + fillFailMessageInfo);
        }
    }
}
