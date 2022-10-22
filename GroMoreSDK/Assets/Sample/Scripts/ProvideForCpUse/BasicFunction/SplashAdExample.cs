using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class SplashAdExample
    {
        // 广告加载成功标志，需要在加载成功后展示
        public bool splashAdLoadSuccess = false;

        /// <summary>
        /// 加载开屏
        /// </summary>
        public void LoadNormalSplashAd()
        {
            string unitID = ABUAdPositionId.SPLASH_NORMA_CODE;
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(unitID)
            .setSplashButtonType(ABUSplashButtonType.ABUSplashButtonTypeFullScreen)
            .setDownloadType(ABUDownloadType.DOWNLOAD_TYPE_NO_POPUP)
            .SetImageAcceptedSize(1080, 1920)//仅Android在使用
            .SetExpressIfCan(false)  // 非模板开屏设置false
            .setSplashButtonType(ABUSplashButtonType.ABUSplashButtonTypeFullScreen) // 开屏点击合规：点击全屏跳转还是按钮跳转
            .Build();
            this.SetSplashUserData();       // 加载开屏前需设置该兜底配置，用于提升填充率；当开屏调用时waterfall配置未获取到会使用开发者设置的兜底
            ABUSplashAd.LoadSplashAd(adUnit, new SplashAdListener(this), 3);
        }

        /// <summary>
        /// 加载开屏
        /// </summary>
        public void LoadExpressSplashAd()
        {
            string unitID = ABUAdPositionId.SPLASH_EXPRESS_CODE;
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(unitID)
            .setSplashButtonType(ABUSplashButtonType.ABUSplashButtonTypeDownloadBar)
            .setDownloadType(ABUDownloadType.DOWNLOAD_TYPE_POPUP)
            .SetExpressIfCan(true)  // 模板开屏设置true
            .SetExpressViewAcceptedSize(1080, 1920)//目前只有Android 使用到了
            .setSplashButtonType(ABUSplashButtonType.ABUSplashButtonTypeDownloadBar)    // 开屏点击合规：点击全屏跳转还是按钮跳转
            .Build();
            this.SetSplashUserData();   // 加载开屏前需设置该兜底配置，用于提升填充率；当开屏调用时waterfall配置未获取到会使用开发者设置的兜底
            ABUSplashAd.LoadSplashAd(adUnit, new SplashAdListener(this), 3);
        }

        /// <summary>
        /// 开屏兜底配置 
        /// </summary>
        /// 用于在广告位还在失败，采用传入的rit进行广告加载;该配置需要在load前设置
        public void SetSplashUserData()
        {
            string appID = ABUAdPositionId.SPLASH_BASELINE_APPID;
            string ritID = ABUAdPositionId.SPLASH_BASELINE_CODE;
            // 加载开屏前需设置该兜底配置，用于提升填充率；当开屏调用时waterfall配置未获取到会使用开发者设置的兜底
            ABUSplashAd.SetUserData(
                ABUAdnType.ABUAdnPangle,
                appID,  // 对应adn的AppID, 开发者可根据自身需要设置用于兜底的开屏兜底adn
                ritID);    // 对应adn的代码位
        }

        /// <summary>
        /// 展示开屏
        /// </summary>
        public void ShowSplashAd()
        {
            if (!splashAdLoadSuccess)
            {
                var msg = "请先加载广告或等待广告加载完成";
                Debug.Log("<Unity Log>..." + msg);
                ToastManager.Instance.ShowToast(msg);
                return;
            }
            ABUSplashAd.ShowSplashAd(new SplashAdInteractionListener(this));
        }

    }

    public sealed class SplashAdListener : ABUSplashAdListener
    {
        private SplashAdExample example;

        public SplashAdListener(SplashAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "OnError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnSplashAdLoad(ABUSplashAd ad)
        {
            Debug.Log("<Unity Log>..." + "OnSplashAdLoad");
            ToastManager.Instance.ShowToast("OnSplashAdLoad");

            this.example.splashAdLoadSuccess = true;
        }

        public void OnAdLoadTimeout()
        {
            //only Android
            Debug.Log("<Unity Log>..." + "OnAdLoadTimeout");
            ToastManager.Instance.ShowToast("OnAdLoadTimeout");
        }
    }

    public sealed class SplashAdInteractionListener : ABUSplashAdInteractionListener
    { 
        private SplashAdExample example;
        public SplashAdInteractionListener(SplashAdExample example)
        {
            this.example = example;
        }

        public void OnAdClicked()
        {
            Debug.Log("<Unity Log>..." + "SplashAd click");
            ToastManager.Instance.ShowToast("SplashAd click");
        }

        public void OnAdShow()
        {
            Debug.Log("<Unity Log>..." + "SplashAd show");
            ToastManager.Instance.ShowToast("SplashAd Show");
            this.example.splashAdLoadSuccess = false;

            // 展示广告的预设ecpm
            string ecpm = ABUSplashAd.GetPreEcpm();
            // 展示广告的代码位
            string ritID = ABUSplashAd.GetAdNetworkRitId();
            // 展示广告的name
            string adnName = ABUSplashAd.GetAdRitInfoAdnName();
            Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
        }

        public void OnAdSkip()
        {
            Debug.Log("<Unity Log>..." + "SplashAd skip");
            ToastManager.Instance.ShowToast("SplashAd skip");

            this.example.splashAdLoadSuccess = false;
        }

        public void OnAdTimeOver()
        {
            Debug.Log("<Unity Log>..." + "SplashAd OnAdTimeOver");
            ToastManager.Instance.ShowToast("SplashAd OnAdTimeOver");
        }

        public void OnAdClose()
        {
            Debug.Log("<Unity Log>..." + "SplashAd CLOSE");
            ToastManager.Instance.ShowToast("SplashAd CLOSE");
            this.example.splashAdLoadSuccess = false;
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
