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
    /// The Reward Ad.
    /// </summary>
    public sealed class ABURewardVideoAd : IDisposable
    {
        private static AndroidJavaObject rewardVideoAdObj;
        private static AndroidJavaObject mAdManager;
        // 是否缓存成功
        public bool IsDownloaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDance.Union.LGRewardVideoAd"/> class.
        /// </summary>
        /// <param name="rewardVideoAdObj">Reward video ad object.</param>
        internal ABURewardVideoAd(AndroidJavaObject rewardVideoAdObj)
        {
        }
        
        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            if (rewardVideoAdObj == null)
            {
                return ABUAdnType.ABUAdnNone;
            }
            int platformId = rewardVideoAdObj.Call<int>("getAdNetworkPlatformId");
            return (ABUAdnType)platformId;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (rewardVideoAdObj == null)
            {
                return "";
            }
            string ritId = rewardVideoAdObj.Call<string>("getAdNetworkRitId");
            return ritId;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            if (rewardVideoAdObj == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = rewardVideoAdObj.Call<AndroidJavaObject>("getShowEcpm");
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
            if (rewardVideoAdObj == null)
            {
                return "";
            }
            string ecpm = rewardVideoAdObj.Call<string>("getPreEcpm");
            return ecpm;
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return true;
        }

        /// <summary>
        /// Shows the reward video ad.
        /// </summary>
        internal static void ShowRewardVideoAd(ABURewardAdInteractionCallback callback)
        {
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => ShowRewardVideoAdOnUi(callback, null));
            activity.Call("runOnUiThread", runnable);
        }

        /// <summary>
        /// Show the full reward video.
        /// extro 额外信息，主要用于ritscene
        /// </summary>
        internal static void ShowRewardVideoAdWithRitScene(ABURewardAdInteractionCallback callback, Dictionary<string, object> extro)
        {
            Debug.Log("<Unity Log>...ShowRewardVideoAdWithRitScene");
            var activity = ABUAdSDK.GetActivity();
            var runnable = new AndroidJavaRunnable(
                () => ShowRewardVideoAdOnUi(callback, extro));
            activity.Call("runOnUiThread", runnable);
        }

        private static void ShowRewardVideoAdOnUi(ABURewardAdInteractionCallback callback, Dictionary<string, object> extro)
        {
            if (rewardVideoAdObj == null)
            {
                return;
            }
            AndroidJavaObject map = null;
            Debug.Log("<Unity Log>...ShowRewardVideoAdOnUi");

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

                //foreach (KeyValuePair<string, object> pair in extro)
                //{
                //    Debug.Log("<Unity Log>...extro.key="+pair.Key);
    
                //    var isEnum = pair.Value.GetType() == typeof(ABURitSceneType);
                //    if(isEnum){
                //        Debug.Log("<Unity Log>...extro.Value="+ pair.Value.ToString());
                //        map.Call<string>("put", pair.Key, (string)pair.Value);
                //    }else{
                //            Debug.Log("<Unity Log>...extro.Value="+ (string)pair.Value);
                //            map.Call<string>("put", pair.Key, (string)pair.Value);
                //    }
                    
                //}
            }
           

            //bool isReady = rewardVideoAdObj.Call<bool>("isReady");
            //Debug.Log("ShowRewardVideoAd isReady " + isReady);
            var rewardAdInteractionListener = new RewardAdInteractionListener(callback);
            //if (isReady)
            //{
                //Debug.Log("ShowRewardVideoAd isReady showRewardAd " + isReady);
                //rewardVideoAdObj.Call("showRewardAd", ABUAdSDK.GetActivity(), rewardAdInteractionListener);
                Debug.Log("<Unity Log>...showRewardAd map");
                ABUAdSDK.GetAdManager().Call("showRewardAd", rewardVideoAdObj, ABUAdSDK.GetActivity(), map, rewardAdInteractionListener);
            //}
        }
        
        /// <summary>
        /// 异步加载激励视频广告，结果会通过RewardVideoAdListener回调
        /// Loads the reward video ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadRewardVideoAd(ABUAdUnit adUnit, ABURewardVideoAdCallback callback)
        {
            if (adUnit == null)
            {
                return;
            }
            initAndLoadNormalRewardAd(adUnit, callback);
        }
        
        private static void initAndLoadNormalRewardAd(ABUAdUnit adUnit, ABURewardVideoAdCallback listener)
        {
            if (mAdManager == null)
            {
                mAdManager = ABUAdSDK.GetAdManager(); 
            }
            string adUnitId = adUnit.unitID;
            Debug.Log("ABURewardVideoAd initAndLoadNormalRewardAd adUnitId " + adUnitId);
            rewardVideoAdObj = mAdManager.Call<AndroidJavaObject>("getNormalRewardAd",ABUAdSDK.GetActivity(), adUnitId);  
            var androidListener = new RewardVideoAdListener(listener);
            Debug.Log("ABURewardVideoAd initAndLoadNormalRewardAd loadRewardAd " + adUnitId);
            rewardVideoAdObj.Call("loadRewardAd", adUnit.Handle ,androidListener);
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class RewardVideoAdListener : AndroidJavaProxy
        {
            private readonly ABURewardVideoAdCallback listener;

            public RewardVideoAdListener(ABURewardVideoAdCallback listener)
                : base("com.bytedance.msdk.api.reward.TTRewardedAdLoadCallback")
            {
                this.listener = listener;
            }

            public void onRewardVideoLoadFail(AndroidJavaObject adError)
            {
                if (adError != null) {
                    Debug.Log(" onRewardVideoLoadFail code " + adError.Get<int>("code") + " message " + adError.Get<string>("message"));
                    UnityDispatcher.PostTask(
                        () => this.listener.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
                } 
            }

            public void onRewardVideoAdLoad()
            {
                Debug.Log("ABURewardVideoAd onRewardVideoAdLoad" );
                UnityDispatcher.PostTask(
                    () => this.listener.OnRewardVideoAdLoad(null));
            }

            public void onRewardVideoCached()
            {
                Debug.Log("ABURewardVideoAd onRewardVideoCached " );
                UnityDispatcher.PostTask(
                    () => this.listener.OnRewardVideoAdCached());
            }
        }
        
#pragma warning disable SA1300
#pragma warning disable IDE1006
        private sealed class RewardAdInteractionListener : AndroidJavaProxy
        {
            private readonly ABURewardAdInteractionCallback listener;

            public RewardAdInteractionListener(
                ABURewardAdInteractionCallback listener)
                : base("com.bytedance.msdk.api.reward.TTRewardedAdListener")
            {
                this.listener = listener;
            }

            public void onRewardedAdShow()
            {
                Debug.Log("ABURewardVideoAd onRewardedAdShow " );
                this.listener.OnAdShow();
                //UnityDispatcher.PostTask(() => this.listener.OnAdShow());
            }

            public void onRewardedAdShowFail(AndroidJavaObject error){
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

            public void onRewardClick()
            {
                this.listener.OnAdVideoBarClick();
                //UnityDispatcher.PostTask(
                    //() => this.listener.OnAdVideoBarClick());
            }

            public void onRewardedAdClosed()
            {
                this.listener.OnAdClose();
                //UnityDispatcher.PostTask(() => this.listener.OnAdClose());
            }

            public void onVideoComplete()
            {
                Debug.Log("ABURewardVideoAd onVideoComplete " );
                this.listener.OnVideoComplete();
                //UnityDispatcher.PostTask(() => this.listener.OnVideoComplete());
            }

            public void onVideoError()
            {
                Debug.Log("ABURewardVideoAd onVideoError " );
                this.listener.OnVideoError(0, "");
                //UnityDispatcher.PostTask(() => this.listener.OnVideoError(0, ""));
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
            
            public void onSkippedVideo()
            {
                Debug.Log("ABURewardVideoAd OnSkippedVideo " );
                this.listener.OnSkippedVideo();
                //UnityDispatcher.PostTask(() => this.listener.OnSkippedVideo());
            }
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

        /// <summary>
        /// Sets the show down load bar.
        /// </summary>
        /// <param name="show">If set to <c>true</c> show.</param>
        public void SetShowDownLoadBar(bool show)
        {
            // this.rewardVideoAdObj.Call("setShowDownLoadBar", show);
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
    }
#endif
}
