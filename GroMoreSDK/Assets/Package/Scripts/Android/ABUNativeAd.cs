//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using System.Collections.Generic;
namespace ByteDance.Union
{

    //#if UNITY_ANDROID
#if !UNITY_EDITOR && UNITY_ANDROID

    using System;
    using UnityEngine;

    /// <summary>
    /// The banner Ad.
    /// </summary>
    public sealed class ABUNativeAd : NativeAd
    {
        private static AndroidJavaObject adNativeAd;
        private static string AdUnitId;
        private static AndroidJavaObject mAdManager;
        private static AndroidJavaObject adNativeAdList;
        private static AndroidJavaObject currentNativeAd;
        private ABUNativeAdInteractionCallback _nativeAdInteractionCallback;
        internal static int SDK_NAME_MT = 4;
        internal static int SDK_NAME_BAIDU = 6;
        internal static int SDK_NAME_SIGMOB = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeAd"/> class.
        /// </summary>
        internal ABUNativeAd(AndroidJavaObject ad): base(ad)
        {
            currentNativeAd = ad;
        }

        public void SetUnitId(string adUnitId)
        {
            AdUnitId = adUnitId;
        }
        
        private static void initAndLoadNativeAd(ABUAdUnit adUnit, ABUFeedAdListener listener)
        {
            if (mAdManager == null)
            {
                mAdManager = ABUAdSDK.GetAdManager(); 
            }
            string adUnitId = adUnit.unitID;
            adNativeAd = mAdManager.Call<AndroidJavaObject>("getFeedAd",ABUAdSDK.GetActivity(), adUnitId);  
            var androidListener = new FeedAdListener(listener);
            adNativeAd.Call("loadAd", adUnit.Handle ,androidListener);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetAdInteractionListener(
            ABUNativeAdInteractionCallback listener)
        {
            _nativeAdInteractionCallback = listener;
        }
        
        internal static void LoadNativeAd(
            ABUAdUnit adUnit, ABUFeedAdListener listener)
        {
            if (adUnit == null)
            {
                return;
            }
            initAndLoadNativeAd(adUnit, listener);
        }

        /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }

        public void ShowNativeAd(int index, float x, float y) {
            if (currentNativeAd == null)
            {
                return;
            }
            AndroidJavaObject feedAdManager = ABUAdSDK.GetFeedAdManager();
            bool isExpressAd = currentNativeAd.Call<bool>("isExpressAd");
            //ToastManager.Instance.ShowToast("feed广告加载成功 是否是模版feed " + isExpressAd);
            if (isExpressAd)
            {
                var activity = ABUAdSDK.GetActivity();
                var runnable = new AndroidJavaRunnable(
                    () => feedAdManager.Call("showExpressFeedAd", ABUAdSDK.GetActivity(), currentNativeAd, new ExpressNativeAdInteractionListener(_nativeAdInteractionCallback)));
                activity.Call("runOnUiThread", runnable);
            }
            else
            {
                var activity = ABUAdSDK.GetActivity();
                var runnable = new AndroidJavaRunnable(
                    () => feedAdManager.Call("showNativeFeedAd", ABUAdSDK.GetActivity(), currentNativeAd, new NativeAdInteractionListener(_nativeAdInteractionCallback)));
                activity.Call("runOnUiThread", runnable);
            }
            string ecpm = GetPreEcpm();
            string ritID = GetAdNetworkRitId();
            ABUAdnType adnType = GetAdNetworkPlaformId();
            Debug.Log("<Unity Log>..." + "Best Ad adnType:" + adnType + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID);
        }
        
        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            if (currentNativeAd == null)
            {
                return ABUAdnType.ABUAdnNone;
            }
            
            int platformId = currentNativeAd.Call<int>("getAdNetworkPlatformId");
            return (ABUAdnType)platformId;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (currentNativeAd == null)
            {
                return "";
            }
            string ritId = currentNativeAd.Call<string>("getAdNetworkRitId");
            return ritId;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            if (currentNativeAd == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = currentNativeAd.Call<AndroidJavaObject>("getShowEcpm");
            if (gmAdEcpmInfo == null) 
            {
                return "";
            }
            string adnName = gmAdEcpmInfo.Call<string>("getAdNetworkPlatformName");
            return adnName;
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public static string GetPreEcpm()
        {
            if (currentNativeAd == null)
            {
                return "";
            }
            string ecpm = currentNativeAd.Call<string>("getPreEcpm");
            return ecpm;
        }

        private sealed class FeedAdListener : AndroidJavaProxy
        {
            private readonly ABUFeedAdListener listener;

            public FeedAdListener(ABUFeedAdListener listener)
                : base("com.bytedance.msdk.api.nativeAd.TTNativeAdLoadCallback")
            {
                this.listener = listener;
            }

            public void onAdLoadedFial(AndroidJavaObject AdError)
            {
                if (AdError != null) {
                Debug.Log(" onFeedAdLoadedFial code " + AdError.Get<int>("code") + " message " + AdError.Get<string>("message"));
                UnityDispatcher.PostTask(
                    () => this.listener.OnError(AdError.Get<int>("code"), AdError.Get<string>("message")));
                }
            }

            public void onAdLoaded(AndroidJavaObject list)
            {
                adNativeAdList = list;
                Debug.Log("onFeedAdLoad  success");
                var size = list.Call<int>("size");
                if (size > 0)
                { 
                   List<ABUNativeAd> nativeAds = new List<ABUNativeAd>();
                   for (int i = 0; i < size; ++i)
                   {
                       ABUNativeAd ad = new ABUNativeAd(
                           list.Call<AndroidJavaObject>("get", i));
                       nativeAds.Insert(i, ad);
                   }
                   UnityDispatcher.PostTask(
                       () => this.listener.OnFeedAdLoad(list, nativeAds));
                }
            }
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class NativeAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABUNativeAdInteractionCallback listener;

            public NativeAdInteractionListener(
                ABUNativeAdInteractionCallback listener)
                : base("com.bytedance.msdk.api.nativeAd.TTNativeAdListener")
            {
                this.listener = listener;
            }

            public void onAdClick()
            {
                Debug.Log("ABUNativeAd onAdClick " );
                UnityDispatcher.PostTask(() => this.listener.OnAdClicked(0));
            }

            public void onAdShow()
            {
                Debug.Log("ABUNativeAd onAdShow " );
                UnityDispatcher.PostTask(
                    () => this.listener.OnAdShow(0));
            }
        }
        
        private sealed class ExpressNativeAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABUNativeAdInteractionCallback listener;

            public ExpressNativeAdInteractionListener(
                ABUNativeAdInteractionCallback listener)
                : base("com.bytedance.msdk.api.nativeAd.TTNativeExpressAdListener")
            {
                this.listener = listener;
            }

            public void onAdClick()
            {
                Debug.Log("ABUNativeAd onAdClick " );
                UnityDispatcher.PostTask(() => this.listener.OnAdClicked(0));
            }

            public void onAdShow()
            {
                Debug.Log("ABUNativeAd onAdShow " );
                UnityDispatcher.PostTask(
                    () => this.listener.OnAdShow(0));
            }

            public void onRenderFail(AndroidJavaObject view, string msg, int code)
            {
                UnityDispatcher.PostTask(
                    () => this.listener.OnRenderFail(msg, code)); 
            }
            
            public void onRenderSuccess(AndroidJavaObject view, float width, float height)
            {
                UnityDispatcher.PostTask(
                    () => this.listener.OnRenderSuccess(width, height)); 
            }
            
        }
    }
#endif
}
