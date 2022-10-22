using System.Collections.Generic;
using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class NativeAdExample
    {
        public bool nativeAdExpressed;
        public AndroidJavaObject mNativeAd;

        /// <summary>
        /// Loads the native ad
        /// 加载原生广告
        /// </summary>
        public void LoadNormalNativeAd()
        {
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(ABUAdPositionId.NATIVE_NORMAL_CODE)
            .SetExpressIfCan(false) // LoadNormalNativeAd需要设置为NO 加载模板时需要设置为Yes
            .SetUseExpress2IfCanForGDT(false)//waterfall内gdt是否为模板2.0，客户端配置是waterfall级别的；但是SDK会优先以服务端rit级别的配置为准
            .SetMutedIfCan(false)   // 设置静音，支持静音的三方adn有效，仅gdt有效
            .SetAdmobNativeAdOptions(ABUAdUnit.getAdmobNativeAdOptions())
            .SetTTVideoOption(ABUAdUnit.getFullVideoOrFeedTTVideoOption(false, false))//waterfall内gdt是否为模板2.0，以及视频是否静音
            .SetImageAcceptedSize(300 * 3, 400 * 3) // iOS端尺寸设置需考虑到不同iPhone的scale
            .SetAdCount(1)
            .Build();
#if UNITY_IOS
            ABUNativeAd.LoadNativeAd(adUnit, new NativeAdListener(this));
#else
            ABUNativeAd.LoadNativeAd(adUnit, new FeedAdListener(this));
#endif
            this.nativeAdExpressed = false;
        }

        public void LoadExpressNativeAd()
        {
            var adUnit = new ABUAdUnit.Builder()
            .SetCodeId(ABUAdPositionId.NATIVE_EXPRESS_CODE)
            .SetExpressIfCan(true) // 加载模板需要设置为true 加载模板时需要设置为Yes
            .SetUseExpress2IfCanForGDT(false)//waterfall内gdt是否为模板2.0，客户端配置是waterfall级别的；但是SDK会优先以服务端rit级别的配置为准
            .SetMutedIfCan(true)   // 设置静音，支持静音的三方adn有效
            .SetAdmobNativeAdOptions(ABUAdUnit.getAdmobNativeAdOptions())
            .SetTTVideoOption(ABUAdUnit.getFullVideoOrFeedTTVideoOption(false, true))///waterfall内gdt是否为模板2.0，视频是否静音
            .SetExpressViewAcceptedSize(300 * 3, 400 * 3) // 
            .SetAdCount(1)
            .Build();
#if UNITY_IOS
            ABUNativeAd.LoadNativeAd(adUnit, new NativeAdListener(this));
#else
            ABUNativeAd.LoadNativeAd(adUnit, new FeedAdListener(this));
#endif
            this.nativeAdExpressed = true;
        }
    }

#if UNITY_IOS
    public sealed class NativeAdListener : ABUNativeAdCallback
    {
        private NativeAdExample example;

        public NativeAdListener(NativeAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "NativeAd OnError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnNativeAdLoad(ABUNativeAd nativeAd)
        {
            Debug.LogError("<Unity Log>..." + "OnNativeAdLoad");
            ToastManager.Instance.ShowToast("<Unity Log>..." + "OnNativeAdLoad");
            nativeAd.SetAdInteractionListener(new NativeAdInteractionCallback(this.example));
            List<int> lists = nativeAd.getAdIndexs();
            float y = 100;
            for (int i = 0; i < lists.Count; i++)
            {
                Debug.Log("chaors" + i + "nativeAd ShowNativeAd");

                // 展示广告, 位置自己进行赋值
                y = y + 400 * i;// 100 500 900
                nativeAd.ShowNativeAd(i, 100, (int)ABUiOSBridgeTools.getWindowSafeAreaInsetsBottom() + y);

                string ecpm = nativeAd.GetPreEcpm(i);
                string ritID = nativeAd.GetAdNetworkRitId(i);
                string adnName = nativeAd.GetAdRitInfoAdnName(i);
                Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
            }
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
            // 开发者根据上述规则解开字符串获取对应adn广告加载失败信息
            Debug.Log("<Unity Log>...fillFailMessageInfo:" + fillFailMessageInfo);
        }

    }

#else

    public sealed class FeedAdListener : ABUFeedAdListener
    {
        private NativeAdExample example;

        public FeedAdListener(NativeAdExample example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            var errMsg = "NativeAd OnError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnFeedAdLoad(AndroidJavaObject list, List<ABUNativeAd> nativeAds)
        {
            var msg = "NativeAd onSuccess : ";
            Debug.LogError("<Unity Log>..." + msg);
            ToastManager.Instance.ShowToast(msg);
            var size = list.Call<int>("size");
            Debug.Log("OnFeedAdLoad size " + size);
            if (size > 0)
            {
                this.example.mNativeAd = list.Call<AndroidJavaObject>("get", 0);
                ABUNativeAd currentNativeAd = nativeAds[0];
                currentNativeAd.SetAdInteractionListener(new NativeAdInteractionCallback(this.example));
                currentNativeAd.ShowNativeAd(0, 0, 0);
            }
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
#endif

    public sealed class NativeAdInteractionCallback : ABUNativeAdInteractionCallback
    {
        private NativeAdExample example;

        public NativeAdInteractionCallback(NativeAdExample example)
        {
            this.example = example;
        }

        public void OnAdClicked(int index)
        {
            var errMsg = "OnAdClicked";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnAdShow(int index)
        {
            var errMsg = "OnAdShow";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnAdDismiss(int index)
        {
            var errMsg = "OnAdDismiss";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnRenderFail(string msg, int code)
        {
            var errMsg = "OnRenderFail";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnRenderSuccess(float width, float height)
        {
            var successMsg = "OnRenderSuccess";
            Debug.LogError("<Unity Log>..." + successMsg);
            ToastManager.Instance.ShowToast(successMsg);
        }
    }

}
