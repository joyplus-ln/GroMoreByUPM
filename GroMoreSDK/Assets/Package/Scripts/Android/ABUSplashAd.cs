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
    /// The splash Ad.
    /// </summary>
    public class ABUSplashAd
    {
        private static ABUAdUnit adUnit;
        private static AndroidJavaObject splashAdObj;
        private static AndroidJavaObject adManager;
        private static AndroidJavaObject splashViewLayout;
        private static AndroidJavaObject ttNetworkRequestInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashAd"/> class.
        /// </summary>
        internal ABUSplashAd(AndroidJavaObject ad)
        {
        }

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return 0;
        }

        /// <summary>
        /// Get current SpalshAd
        /// </summary>
        public AndroidJavaObject getCurrentSplshAd()
        {
            return null;
        }
        
        public ABUAdUnit getAdUnit()
        {
            return adUnit;
        }
        
        /// <summary>
        /// 设置开屏兜底配置
        /// Loads the splash ad.
        /// </summary>
        // <param name="adnType">adnType.</param>
        /// <param name="appID">appID.</param>
        /// <param name="ritID">ritID.</param> 
        internal static void SetUserData(ABUAdnType adnType, string appID, string ritID)
        {
            if (adManager == null)
            {
                adManager = ABUAdSDK.GetAdManager();
            }
            Debug.Log("ABUSplash AdSetUserData adnType " + adnType);
            if (adnType == ABUAdnType.ABUAdnPangle)
            {
                //使用穿山甲兜底
                ttNetworkRequestInfo = adManager.Call<AndroidJavaObject>("getSplashPangolinNetworkRequestInfo", appID, ritID); 
            } else if (adnType == ABUAdnType.ABUAdnGDT)
            {
                //使用广点通兜底
                ttNetworkRequestInfo = adManager.Call<AndroidJavaObject>("getSplashGdtNetworkRequestInfo", appID, ritID); 
            }
        }
        
        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            if (splashAdObj == null)
            {
                return ABUAdnType.ABUAdnNone;
            }
            int platformId = splashAdObj.Call<int>("getAdNetworkPlatformId");
            return (ABUAdnType)platformId;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (splashAdObj == null)
            {
                return "";
            }
            string ritId = splashAdObj.Call<string>("getAdNetworkRitId");
            return ritId;
        }


        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            if (splashAdObj == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = splashAdObj.Call<AndroidJavaObject>("getShowEcpm");
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
            if (splashAdObj == null)
            {
                return "";
            }
            string ecpm = splashAdObj.Call<string>("getPreEcpm");
            return ecpm;
        }
        /// <summary>
        /// 加载开屏广告
        /// Loads the splash video ad.
        /// </summary>
        // <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="timeOut">timeOut.</param> 
        internal static void LoadSplashAd(ABUAdUnit adUnit, ABUSplashAdListener callback, int timeOut)
        {
            if (adUnit == null)
            {
                return;
            }
            initAndLoadSplashAd(adUnit, callback, timeOut);
        }
        
        private static void initAndLoadSplashAd(ABUAdUnit adUnit, ABUSplashAdListener listener, int timeOut)
        {
            if (adManager == null)
            {
                adManager = ABUAdSDK.GetAdManager(); 
            }
            string adUnitId = adUnit.unitID;
            Debug.Log("ABUSplashAd initAndLoadSplashAd adUnitId " + adUnitId + " timeOut " + timeOut);
            splashAdObj = adManager.Call<AndroidJavaObject>("getSplashAd",ABUAdSDK.GetActivity(), adUnitId);
            var activity = ABUAdSDK.GetActivity();
            var getSplashRunnable = new AndroidJavaRunnable(
                () => splashViewLayout = adManager.Call<AndroidJavaObject>("getFrameLayoutForSplash", ABUAdSDK.GetActivity()));
            activity.Call("runOnUiThread", getSplashRunnable);
            var androidListener = new SplashAdListener(listener);
            Debug.Log("ABUSplashAd initAndLoadSplashAd loadSplashAd " + adUnitId);
            splashAdObj.Call("loadAd", adUnit.Handle , ttNetworkRequestInfo, androidListener, timeOut * 1000);
        }

        /// <summary>
        /// 加载广告
        /// Loads the splash video ad.
        internal static void ShowSplashAd(ABUSplashAdInteractionListener callback)
        {
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => ShowSplashAdOnUi(callback));
            activity.Call("runOnUiThread", runnable);
          
        }
        
        private static void ShowSplashAdOnUi(ABUSplashAdInteractionListener callback)
        {
            if (adManager == null)
            {
                adManager = ABUAdSDK.GetAdManager(); 
            }
            if (splashAdObj == null)
            {
                return;
            }
            splashAdObj.Call("setTTAdSplashListener", new SplashAdInteractionListener(callback));
            splashAdObj.Call("showAd", splashViewLayout);
        }

        /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
            // var androidListener = new AppDownloadCallback(listener);
            // this.ad.Call("setDownloadListener", androidListener);
        }

        /// <summary>
        /// Set this Ad not allow sdk count down.
        /// </summary>
        public void SetNotAllowSdkCountdown()
        {
            // this.ad.Call("setNotAllowSdkCountdown");
        }
        
        private static void closeSplash()
        {
            if (adManager == null)
            {
                adManager = ABUAdSDK.GetAdManager(); 
            }
            Debug.Log("ABUSplashAd 关闭开屏");
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => adManager.Call("removeViewFromRootView", ABUAdSDK.GetActivity(), splashViewLayout));
            activity.Call("runOnUiThread", runnable);
        }
        
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class SplashAdListener : AndroidJavaProxy
        {
            private readonly ABUSplashAdListener listener;
            private AndroidJavaObject splashLayout;
            public SplashAdListener(ABUSplashAdListener listener)
                : base("com.bytedance.msdk.api.splash.TTSplashAdLoadCallback")
            {
                this.listener = listener;
            }

            public void onSplashAdLoadFail(AndroidJavaObject adError)
            {
                if (adError != null) {
                    Debug.Log("ABUSplashAd onSplashAdLoadFail code " + adError.Get<int>("code") + " message " + adError.Get<string>("message"));
                    UnityDispatcher.PostTask(
                        () => this.listener.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
                }
                closeSplash();
            }

            public void onSplashAdLoadSuccess()
            {
                Debug.Log("ABUSplashAd onSplashAdLoadSuccess" );
                UnityDispatcher.PostTask(
                    () => this.listener.OnSplashAdLoad(null));
            }

            public void onAdLoadTimeout()
            {
                Debug.Log("ABUSplashAd onAdLoadTimeout " );
                UnityDispatcher.PostTask(
                    () => this.listener.OnAdLoadTimeout());
                closeSplash();
            }
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class SplashAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABUSplashAdInteractionListener listener;
            private AndroidJavaObject splashLayout;

            public SplashAdInteractionListener(
                ABUSplashAdInteractionListener listener)
                : base("com.bytedance.msdk.api.splash.TTSplashAdListener")
            {
                this.listener = listener;
                this.splashLayout = splashLayout;
            }

            public void onAdClicked()
            {
                Debug.Log("ABUSplashAd onAdClicked " );
                UnityDispatcher.PostTask(() => this.listener.OnAdClicked());
            }

            public void onAdShow()
            {
                UnityDispatcher.PostTask(
                    () => this.listener.OnAdShow());
            }
            public void onAdShowFail(AndroidJavaObject error)
            {
                 var errCode = 0;
                 var errMsg = "";
                 Debug.Log("<Unity Log>...OnAdShowFailed error:" + error.Call<string>("toString"));

                 if(error != null) {
                    errCode = error.Get<int>("thirdSdkErrorCode");
                    errMsg = error.Get<string>("thirdSdkErrorMessage");
                    if(errCode == 0) {
                        errCode = error.Get<int>("code");
                        errMsg = error.Get<string>("message");
                    }
                 }
                 UnityDispatcher.PostTask(() => this.listener.OnAdShowFailed(errCode, errMsg));

            }
            public void onAdSkip()
            {
                UnityDispatcher.PostTask(() => this.listener.OnAdSkip());
                closeSplash();
            }

            public void onAdDismiss()
            {
                Debug.Log("ABUSplashAd onAdDismiss " );
                UnityDispatcher.PostTask(() => this.listener.OnAdClose());
                closeSplash();
            }
        }
        
#pragma warning restore SA1300
#pragma warning restore IDE1006
    }
#endif
}
