using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class BannerAdExample
    {
        public ABUBannerAd bannerAd;   // for iOS

        public void LoadBannerAd()
        {
            string unitID = ABUAdPositionId.BANNER_CODE;
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(unitID)
            .SetImageAcceptedSize(320, 150)  // banner广告比例
            .Build();
            ABUBannerAd.LoadBannerAd(adUnit, new BannerAdCallback(this), new BannerAdInteractionCallback(this));
        }
    }

    public sealed class BannerAdCallback : ABUBannerAdCallback
    {
        private BannerAdExample example;

        public BannerAdCallback(BannerAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "OnError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnBannerAdLoad(ABUBannerAd ad)
        {
            Debug.Log("<Unity Log>..." + "OnBannerAdLoad");
            ToastManager.Instance.ShowToast("OnBannerAdLoad");
#if UNITY_IOS
            this.example.bannerAd = ad;
            this.example.bannerAd.ShowBannerAd(0, ABUiOSBridgeTools.getScreenHeight() - ad.getBannerViewHeight());
#endif
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
    }

    public sealed class BannerAdInteractionCallback : ABUBannerAdInteractionCallback
    {
        private BannerAdExample example;

        public BannerAdInteractionCallback(BannerAdExample example)
        {
            this.example = example;
        }

        public void OnAdClicked()
        {
            Debug.Log("<Unity Log>..." + "BannerAd OnAdClicked");
            ToastManager.Instance.ShowToast("BannerAd OnAdClicked");
        }

        public void OnAdShow()
        {
            Debug.Log("<Unity Log>..." + "BannerAd OnAdShow");
            ToastManager.Instance.ShowToast("BannerAd OnAdShow"); 
            string ecpm = ABUBannerAd.GetPreEcpm();
            string ritID = ABUBannerAd.GetAdNetworkRitId();
            string adnName = ABUBannerAd.GetAdRitInfoAdnName();
            Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
        }

        public void OnAdDismiss()
        {
            Debug.Log("<Unity Log>..." + "BannerAd OnAdDismiss");
            ToastManager.Instance.ShowToast("BannerAd OnAdDismiss");
        }
    }
}
