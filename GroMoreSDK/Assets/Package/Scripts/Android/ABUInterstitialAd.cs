//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace ByteDance.Union
{
    //#if UNITY_ANDROID
#if !UNITY_EDITOR && UNITY_ANDROID
    /// <summary>
    /// Set the interaction Ad.
    /// </summary>
    public sealed class ABUInterstitialAd
    {
      
        private static AndroidJavaObject mInterstitialAdObj;
        private static AndroidJavaObject mAdManager;
        
        /// <summary>
        /// 异步加载广告
        /// Loads the Interstitial video ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadInterstitialAd(ABUAdUnit adUnit, ABUInterstitialAdCallback callback)
        {
            if (adUnit == null)
            {
                return;
            }
            initAndLoadInterstitialAd(adUnit, callback);
        }
        
        private static void initAndLoadInterstitialAd(ABUAdUnit adUnit, ABUInterstitialAdCallback listener)
        {
            if (mAdManager == null)
            {
                mAdManager = ABUAdSDK.GetAdManager(); 
            }
            string adUnitId = adUnit.unitID;
            Debug.Log("ABUInterstitialAd initAndLoadInterstitialAd adUnitId " + adUnitId);
            mInterstitialAdObj = mAdManager.Call<AndroidJavaObject>("getInterstitialAd",ABUAdSDK.GetActivity(), adUnitId);
            var androidListener = new InterstitialAdListener(listener);
            Debug.Log("ABUInterstitialAd initAndLoadInterstitialAd loadFullAd " + adUnitId);
            mInterstitialAdObj.Call("loadAd", adUnit.Handle ,androidListener);
        }

        /// <summary>
        /// Sets the listener for the Ad download.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return 0;
        }
        
        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            if (mInterstitialAdObj == null)
            {
                return ABUAdnType.ABUAdnNone;
            }
            int platformId = mInterstitialAdObj.Call<int>("getAdNetworkPlatformId");
            return (ABUAdnType)platformId;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (mInterstitialAdObj == null)
            {
                return "";
            }
            string ritId = mInterstitialAdObj.Call<string>("getAdNetworkRitId");
            return ritId;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            if (mInterstitialAdObj == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = mInterstitialAdObj.Call<AndroidJavaObject>("getShowEcpm");
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
            if (mInterstitialAdObj == null)
            {
                return "";
            }
            string ecpm = mInterstitialAdObj.Call<string>("getPreEcpm");
            return ecpm;
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return true;
        }

        /// <summary>
        /// Show this interaction Ad.
        /// </summary>
        internal static void ShowInteractionAd(ABUInterstitialAdInteractionCallback callback)
        {
            Debug.Log("ShowInteractionAd start");
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => ShowInteractionAdOnUi(callback));
            activity.Call("runOnUiThread", runnable);
        }

        private static void ShowInteractionAdOnUi(ABUInterstitialAdInteractionCallback callback)
        {
            Debug.Log("showInteractionAd end");
            if (mInterstitialAdObj == null)
            {
                return;
            }
            //bool isReady = mInterstitialAdObj.Call<bool>("isReady");
            //Debug.Log("ShowInteractionAd isReady " + isReady);
            //if (isReady)
            //{
                //Debug.Log("ShowInteractionAd isReady showAd " + isReady);
                var callbackListener = new InterstitialAdInteractionListener(callback);
                mInterstitialAdObj.Call("setTTAdInterstitialListener", callbackListener);
                mInterstitialAdObj.Call("showAd", ABUAdSDK.GetActivity());
            //}
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class InterstitialAdListener : AndroidJavaProxy
        {
            private readonly ABUInterstitialAdCallback listener;

            public InterstitialAdListener(ABUInterstitialAdCallback listener)
                : base("com.bytedance.msdk.api.interstitial.TTInterstitialAdLoadCallback")
            {
                this.listener = listener;
            }

            public void onInterstitialLoadFail(AndroidJavaObject adError)
            {
                if (adError != null) {
                    Debug.Log("ABUInterstitialAd onInterstitialLoadFail code " + adError.Get<int>("code") + " message " + adError.Get<string>("message"));
                    UnityDispatcher.PostTask(
                        () => this.listener.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
                } 
            }

            public void onInterstitialLoad()
            {
                Debug.Log("ABUInterstitialAd onInterstitialLoad" );
                UnityDispatcher.PostTask(
                    () => this.listener.OnInterstitialAdLoad(null));
            }
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class InterstitialAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABUInterstitialAdInteractionCallback listener;

            public InterstitialAdInteractionListener(
                ABUInterstitialAdInteractionCallback listener)
                : base("com.bytedance.msdk.api.interstitial.TTInterstitialAdListener")
            {
                this.listener = listener;
            }

            public void onInterstitialShow()
            {
                Debug.Log("ABUInterstitialAd onInterstitialShow " );
                UnityDispatcher.PostTask(() => this.listener.OnAdShow());
            }
        
            public void onInterstitialShowFail(AndroidJavaObject error) 
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

            public void onInterstitialAdClick()
            {
                UnityDispatcher.PostTask(
                    () => this.listener.OnAdClicked());
            }

            public void onInterstitialClosed()
            {
                UnityDispatcher.PostTask(() => this.listener.OnAdDismiss());
            }

            public void onAdOpened()
            {
                Debug.Log("ABUInterstitialAd onAdOpened " );
               
            }

            public void onAdLeftApplication()
            {
                Debug.Log("ABUInterstitialAd onAdLeftApplication " );
            }
        }  
        
    }
#endif
}
