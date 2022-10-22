using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class InterstitalAdExample
    {
        // 广告加载成功标志，需要在加载成功后展示
        public bool interstitialAdLoadSuccess = false;

        /// <summary>
        /// Load the intertital Ad.
        /// </summary>
        public void LoadInterstitalAd(bool useExpress2IfCanForGDT)
        {
            string unitID = ABUAdPositionId.INTERSTITAL_CODE;
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(unitID)
            .SetImageAcceptedSize(414, 300)
            .SetExpressIfCan(true)// 设置出模板广告
            .SetTTVideoOption(ABUAdUnit.getFullVideoOrFeedTTVideoOption(useExpress2IfCanForGDT, false))//Android:waterfall内gdt是否为模板2.0，客户端配置是waterfall级别的；但是SDK会优先以服务端rit级别的配置为准
            .SetUseExpress2IfCanForGDT(useExpress2IfCanForGDT)//iOS:waterfall内gdt是否为模板2.0，客户端配置是waterfall级别的；但是SDK会优先以服务端rit级别的配置为准
            .Build();
            ABUInterstitialAd.LoadInterstitialAd(adUnit, new InterstitialAdCallback(this));
        }

        /// <summary>
        /// Show the intertital Ad.
        /// 加载插屏广告
        /// </summary>
        public void ShowInterstitalAd()
        {
            if (!interstitialAdLoadSuccess || !ABUInterstitialAd.isReady())
            {
                var msg = "请先加载广告或等待广告加载完成";
                Debug.Log("<Unity Log>..." + msg);
                ToastManager.Instance.ShowToast(msg);
                return;
            }
            ABUInterstitialAd.ShowInteractionAd(new InterstitialAdInteractionCallback(this));
        }
    }

    public sealed class InterstitialAdCallback : ABUInterstitialAdCallback
    {
        private InterstitalAdExample example;

        public InterstitialAdCallback(InterstitalAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "OnError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnInterstitialAdLoad(object ad)
        {
            Debug.Log("<Unity Log>..." + "OnInterstitialAdLoad");
            ToastManager.Instance.ShowToast("OnInterstitialAdLoad");
            this.example.interstitialAdLoadSuccess = true;
        }

    }

    public sealed class InterstitialAdInteractionCallback : ABUInterstitialAdInteractionCallback
    {
        private InterstitalAdExample example;

        public InterstitialAdInteractionCallback(InterstitalAdExample example)
        {
            this.example = example;
        }

        public void OnAdClicked()
        {
            Debug.Log("<Unity Log>..." + "InterstitialAd click");
            ToastManager.Instance.ShowToast("InterstitialAd click");
        }

        public void OnAdShow()
        {
            Debug.Log("<Unity Log>..." + "InterstitialAd OnAdShow");
            ToastManager.Instance.ShowToast("InterstitialAd OnAdShow");
            this.example.interstitialAdLoadSuccess = false;

            string ecpm = ABUInterstitialAd.GetPreEcpm();
            string ritID = ABUInterstitialAd.GetAdNetworkRitId();
            string adnName = ABUInterstitialAd.GetAdRitInfoAdnName();
            Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
        }

        public void OnAdDismiss()
        {
            Debug.Log("<Unity Log>..." + "InterstitialAd OnAdDismiss");
            ToastManager.Instance.ShowToast("InterstitialAd OnAdDismiss");
            this.example.interstitialAdLoadSuccess = false;
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
    }
}
