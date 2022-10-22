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
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// The banner Ad.
    /// </summary>
    public sealed class ABUFullScreenVideoAd : IDisposable
    {
        private static AndroidJavaObject fullVideoAdObj;
        private static AndroidJavaObject mAdManager;
        // 是否缓存成功
        public bool IsDownloaded;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDance.Union.LGFullScreenVideoAd"/> class.
        /// </summary>
        /// <param name="javaObject">Java object.</param>
        internal ABUFullScreenVideoAd(AndroidJavaObject javaObject)
        {
           
        }

        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadFullScreenVideoAd(ABUAdUnit adUnit, ABUFullScreenVideoAdCallback callback)
        {
            if (adUnit == null)
            {
                return;
            }
            initAndLoadFullVideoAd(adUnit, callback);
        }
        
        private static void initAndLoadFullVideoAd(ABUAdUnit adUnit, ABUFullScreenVideoAdCallback listener)
        {
            if (mAdManager == null)
            {
                mAdManager = ABUAdSDK.GetAdManager(); 
            }
            string adUnitId = adUnit.unitID;
            Debug.Log("ABUFullScreenVideoAd initAndLoadFullVideoAd adUnitId " + adUnitId);
            fullVideoAdObj = mAdManager.Call<AndroidJavaObject>("getFullVideoAd",ABUAdSDK.GetActivity(), adUnitId);
            var androidListener = new FullVideoAdListener(listener);
            Debug.Log("ABUFullScreenVideoAd initAndLoadNormalRewardAd loadFullAd " + adUnitId);
            fullVideoAdObj.Call("loadFullAd", adUnit.Handle ,androidListener);
        }
        
        /// <summary>
        /// Shows the reward video ad.
        /// </summary>
        internal static void ShowFullVideoAd(ABUFullScreenVideoAdInteractionCallback callback)
        {
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => showFullVideoAdOnUi(callback, null));
            activity.Call("runOnUiThread", runnable);
        }

        /// <summary>
        /// Show the full screen video.
        /// extro 额外信息，主要用于ritscene
        /// </summary>
        internal static void ShowFullVideoAdWithRitScene(ABUFullScreenVideoAdInteractionCallback callback, Dictionary<string, object> extro)
        {
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => showFullVideoAdOnUi(callback, extro));
            activity.Call("runOnUiThread", runnable);

        }

        private static void showFullVideoAdOnUi(ABUFullScreenVideoAdInteractionCallback callback, Dictionary<string, object> extro)
        {
            if (fullVideoAdObj == null)
            {
                return;
            }
            AndroidJavaObject map = null;
            Debug.Log("<Unity Log>...showFullVideoAdOnUi");

            if (extro != null)
            {
                map = new AndroidJavaObject("java.util.HashMap");
                Debug.Log("<Unity Log>...extro.Count:" + extro.Count);


            List<string> test = new List<string>(extro.Keys);

            for (int i = 0; i < extro.Count; i++)

            {
                Debug.Log("<Unity Log>..info ="+ test[i] + "  ----  "+ extro[test[i]]);

                map.Call<string>("put", test[i], extro[test[i]]+"");

            }
            }

            //bool isReady = fullVideoAdObj.Call<bool>("isReady");
            //Debug.Log("ShowFullVideoAd isReady " + isReady);
            var fullAdInteractionListener = new FullAdInteractionListener(callback);
            //if (isReady)
            //{
                //Debug.Log("ShowFullVideoAd isReady ShowFullAd " + isReady);
                //fullVideoAdObj.Call("showFullAd", ABUAdSDK.GetActivity(), fullAdInteractionListener);
                ABUAdSDK.GetAdManager().Call("showFullAd", fullVideoAdObj, ABUAdSDK.GetActivity(), map, fullAdInteractionListener);
            //}
        }

        /// <summary>
        /// Sets the listener for the Ad download.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }

        /// <summary>
        /// Shows the full screen video ad.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public void ShowFullScreenVideoAd()
        {

        }

        /// <summary>
        /// show the full screen video ad with scene.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scene"></param>
        public void ShowFullScreenVideoAdWithScene(ABURitSceneType type, string scene)
        {
            
        }

        /// <summary>
        /// Sets the interaction listener.
        /// </summary>
        /// <param name="interactionCallback">Interaction listener.</param>
        public void SetInteractionCallback(ABUFullScreenVideoAdInteractionCallback interactionCallback) 
        {
          
        }

        /// <summary>
        /// Sets the download callback.
        /// </summary>
        /// <param name="downloadCallback">Download callback.</param>
        public void SetDownloadCallback(ABUAppDownloadCallback downloadCallback) 
        {
           
        }

        /// <summary>
        /// Gets the type of the interaction.
        /// </summary>
        /// <returns>The interaction type.</returns>
        public ABUAdInteractionType GetInteractionType()
        {
           

            return ABUAdInteractionType.UNKNOWN;
        }
        
        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            if (fullVideoAdObj == null)
            {
                return ABUAdnType.ABUAdnNone;
            }
            int platformId = fullVideoAdObj.Call<int>("getAdNetworkPlatformId");
           
            return (ABUAdnType)platformId;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (fullVideoAdObj == null)
            {
                return "";
            }
            string ritId = fullVideoAdObj.Call<string>("getAdNetworkRitId");
            return ritId;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            if (fullVideoAdObj == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = fullVideoAdObj.Call<AndroidJavaObject>("getShowEcpm");
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
            if (fullVideoAdObj == null)
            {
                return "";
            }
            string ecpm = fullVideoAdObj.Call<string>("getPreEcpm");
            return ecpm;
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return true;
        }

        private ABUAdInteractionType Obj2InteractionType(int index)
        {
            switch (index)
            {
                case (int)ABUAdInteractionType.BROWSER:
                    return ABUAdInteractionType.BROWSER;
                case (int)ABUAdInteractionType.LANDING_PAGE:
                    return ABUAdInteractionType.LANDING_PAGE;
                case (int)ABUAdInteractionType.DOWNLOAD:
                    return ABUAdInteractionType.DOWNLOAD;
                case (int)ABUAdInteractionType.DIAL:
                    return ABUAdInteractionType.DIAL;
                default:
                    return ABUAdInteractionType.UNKNOWN;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class FullVideoAdListener : AndroidJavaProxy
        {
            private readonly ABUFullScreenVideoAdCallback listener;

            public FullVideoAdListener(ABUFullScreenVideoAdCallback listener)
                : base("com.bytedance.msdk.api.fullVideo.TTFullVideoAdLoadCallback")
            {
                this.listener = listener;
            }

            public void onFullVideoLoadFail(AndroidJavaObject adError)
            {
                if (adError != null) {
                    Debug.Log("ABUFullScreenVideoAd onFullVideoLoadFail code " + adError.Get<int>("code") + " message " + adError.Get<string>("message"));
                    UnityDispatcher.PostTask(
                        () => this.listener.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
                } 
            }

            public void onFullVideoAdLoad()
            {
                Debug.Log("ABUFullScreenVideoAd onFullVideoAdLoad" );
                UnityDispatcher.PostTask(
                    () => this.listener.OnFullScreenVideoAdLoad(null));
            }

            public void onFullVideoCached()
            {
                Debug.Log("ABUFullScreenVideoAd onFullVideoCached " );
                UnityDispatcher.PostTask(
                    () => this.listener.OnFullScreenVideoAdCached());
            }
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class FullAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABUFullScreenVideoAdInteractionCallback listener;

            public FullAdInteractionListener(
                ABUFullScreenVideoAdInteractionCallback listener)
                : base("com.bytedance.msdk.api.fullVideo.TTFullVideoAdListener")
            {
                this.listener = listener;
            }

            public void onFullVideoAdShow()
            {
                Debug.Log("ABUFullScreenVideoAd onRewardedAdShow " );
                if (listener == null)
                {
                    return;
                }
                this.listener.OnAdShow();
                //UnityDispatcher.PostTask(() => this.listener.OnAdShow());
            }

            public void onFullVideoAdShowFail(AndroidJavaObject error) 
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
                 this.listener.OnAdShowFailed(errCode, errMsg);
                 //UnityDispatcher.PostTask(() => this.listener.OnAdShowFailed(errCode, errMsg));
            }

            public void onFullVideoAdClick()
            {
                if (listener == null)
                {
                    return;
                }
                this.listener.OnAdVideoBarClick();
                //UnityDispatcher.PostTask(() => this.listener.OnAdVideoBarClick());
            }

            public void onFullVideoAdClosed()
            {
                if (listener == null)
                {
                    return;
                }
                this.listener.OnAdClose();
                //UnityDispatcher.PostTask(() => this.listener.OnAdClose());
            }

            public void onVideoComplete()
            {
                Debug.Log("ABUFullScreenVideoAd onVideoComplete " );
                if (listener == null)
                {
                    return;
                }
                this.listener.OnVideoComplete();
                //UnityDispatcher.PostTask(() => this.listener.OnVideoComplete());
            }

            public void onVideoError()
            {
                Debug.Log("ABUFullScreenVideoAd onVideoError " );
                if (listener == null)
                {
                    return;
                }
                this.listener.OnVideoError(0, "");
                //UnityDispatcher.PostTask(() => this.listener.OnVideoError(0, ""));
            }
            
            public void onSkippedVideo()
            {
                Debug.Log("ABUFullScreenVideoAd OnSkippedVideo " );
                if (listener == null)
                {
                    return;
                }
                this.listener.OnSkippedVideo();
                //UnityDispatcher.PostTask(() => this.listener.OnSkippedVideo());
            }

            public void onRewardVerify(AndroidJavaObject rewardItem)
            {
                Debug.Log("ABUFullScreenVideoAd OnSkippedVideo ");
                if (listener == null)
                {
                    return;
                }

                if (rewardItem == null)
                {
                    this.listener.OnRewardVerify(false, null);
                    return;
                }

                bool verify = rewardItem.Call<bool>("rewardVerify");
                var result = ABUAdSDK.GetAdManager().CallStatic<string>("rewardItemToUnityJsonString", rewardItem);
                if (string.IsNullOrEmpty(result))
                {
                    this.listener.OnRewardVerify(verify, null);
                }

                this.listener.OnRewardVerify(verify, JsonUtility.FromJson<ABUAdapterRewardAdInfo>(result));
            }
        }
    }
#endif
}
