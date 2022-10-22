//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    //#if UNITY_ANDROID
#if !UNITY_EDITOR && UNITY_ANDROID
    using UnityEngine;

    /// <summary>
    /// The banner Ad.
    /// </summary>
    public sealed class ABUBannerAd
    {
        private static AndroidJavaObject bannerAdObj;
        private static AndroidJavaObject mAdManager;
        private static AndroidJavaObject bannerViewLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerAd"/> class.
        /// </summary>
        internal ABUBannerAd(AndroidJavaObject ad)
        {
        }
  
        public float getBannerViewWidth()
        {
            return 0;
        }

        public float getBannerViewHeight()
        {
            return 0;
        }
        
        
        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            if (bannerAdObj == null)
            {
                return ABUAdnType.ABUAdnNone;
            }
            int platformId = bannerAdObj.Call<int>("getAdNetworkPlatformId");
            return (ABUAdnType)platformId;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (bannerAdObj == null)
            {
                return "";
            }
            string ritId = bannerAdObj.Call<string>("getAdNetworkRitId");
            return ritId;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            if (bannerAdObj == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = bannerAdObj.Call<AndroidJavaObject>("getShowEcpm");
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
            if (bannerAdObj == null)
            {
                return "";
            }
            string ecpm = bannerAdObj.Call<string>("getPreEcpm");
            return ecpm;
        }

        /// <summary>
        /// 异步加载广告
        /// Loads the reward video ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadBannerAd(
            ABUAdUnit adUnit, ABUBannerAdCallback listener, ABUBannerAdInteractionCallback callback)
        {
            if (adUnit == null)
            {
                return;
            }
            initAndLoadBannerAd(adUnit, listener, callback);
        }

        private static void initAndLoadBannerAd(ABUAdUnit adUnit, ABUBannerAdCallback listener, ABUBannerAdInteractionCallback callback)
        {
            if (mAdManager == null)
            {
                mAdManager = ABUAdSDK.GetAdManager(); 
            }
            string adUnitId = adUnit.unitID;
            Debug.Log("ABUBannerAd initAndLoadBannerAd adUnitId " + adUnitId);
            bannerAdObj = mAdManager.Call<AndroidJavaObject>("getBannerAd",ABUAdSDK.GetActivity(), adUnitId);
            bannerAdObj.Call("setAdBannerListener", new BannerAdInteractionListener(callback));
            initBannerLayout();
            var androidListener = new BannerAdListener(listener);
            Debug.Log("ABUBannerAd initAndLoadBannerAd loadAd " + adUnitId + ",refreshTime:" + adUnit.getRefreshTime());
            mAdManager.Call("loadBannerAd",ABUAdSDK.GetActivity(), bannerAdObj,adUnit.Handle,adUnit.getRefreshTime(),androidListener);
            //bannerAdObj.Call("loadAd", adUnit.Handle ,androidListener);
        }

        private static void initBannerLayout()
        {
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => bannerViewLayout = mAdManager.Call<AndroidJavaObject>("getFrameLayout", ABUAdSDK.GetActivity()));
            activity.Call("runOnUiThread", runnable);
        }
        
        private static void destroyBannerAd()
        {
            if (bannerAdObj == null)
            {
                return;
            }
            bannerAdObj.Call("destroy");
        }
        
        private static void removeBannerLayout()
        {
            Debug.Log("ABUBannerAd removeBannerLayout" );
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => mAdManager.Call<AndroidJavaObject>("removeViewFromRootView", ABUAdSDK.GetActivity(), bannerViewLayout));
            activity.Call("runOnUiThread", runnable);
        }
        
       /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class BannerAdListener : AndroidJavaProxy
        {
            private readonly ABUBannerAdCallback listener;

            public BannerAdListener(ABUBannerAdCallback listener)
                : base("com.bytedance.msdk.api.v2.ad.banner.GMBannerAdLoadCallback")
            {
                this.listener = listener;
            }

            public void onAdFailedToLoad(AndroidJavaObject adError)
            {
                if (adError != null) {
                    Debug.Log("ABUBannerAd onAdFailedToLoad code " + adError.Get<int>("code") + " message " + adError.Get<string>("message"));
                    removeBannerLayout();
                    UnityDispatcher.PostTask(
                        () => this.listener.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
                } 
            }

            public void onAdLoaded()
            {
                Debug.Log("ABUBannerAd onAdLoaded" );
                removeBannerLayout();
                AndroidJavaObject bannerView = bannerAdObj.Call<AndroidJavaObject>("getBannerView");
                if (bannerView != null)
                {
                    Debug.Log("ABUBannerAd onAdLoaded addView" );
                    bannerViewLayout.Call("addView", bannerView);
                }
                UnityDispatcher.PostTask(
                    () => this.listener.OnBannerAdLoad(null));
            }
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class BannerAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABUBannerAdInteractionCallback listener;

            public BannerAdInteractionListener(
                ABUBannerAdInteractionCallback listener)
                : base("com.bytedance.msdk.api.v2.ad.banner.GMBannerAdListener")
            {
                this.listener = listener;
            }

            public void onAdOpened()
            {
                Debug.Log("ABUBannerAd onAdOpened " );
            }

            public void onAdLeftApplication()
            {
            }

            public void onAdClosed()
            {
                destroyBannerAd();
                if (listener != null)
                {
                    UnityDispatcher.PostTask(() => this.listener.OnAdDismiss());  
                }
                
               
            }

            public void onAdClicked()
            {
                Debug.Log("ABUBannerAd onAdClicked " );
                if (listener != null)
                {
                    UnityDispatcher.PostTask(() => this.listener.OnAdClicked()); 
                }
               
            }

            public void onAdShow()
            {
                Debug.Log("ABUBannerAd onAdShow " );
                if (listener != null)
                {
                    UnityDispatcher.PostTask(() => this.listener.OnAdShow());
                }
            }

            public void onAdShowFail(AndroidJavaObject adError) {

            }
        }
        
        public void ShowBannerAd(float x, float y)
        {
        }

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return 0;
        }
        
        public void SetSlideIntervalTime(int interval)
        {
        }
    }
#endif
}
