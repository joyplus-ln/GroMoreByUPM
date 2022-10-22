//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using UnityEngine;

namespace ByteDance.Union
{
#if (!UNITY_EDITOR) && UNITY_ANDROID
    using System;

    /// <summary>
    /// Set the InterstitialFull Ad.
    /// </summary>
    public sealed class ABUInterstitialFullAd
    {
        private static AndroidJavaObject AdHandle { get; set; }

        /// <summary>
        /// Loads the InterstitialFull video ad.
        /// </summary>
        /// <param name="adUnit">ad dto.</param>
        /// <param name="callback">callback.</param>
        internal static void LoadInterstitialFullAd(ABUAdUnit adUnit, ABUInterstitialFullAdCallback callback)
        {
            if (adUnit == null)
            {
                return;
            }

            AdHandle = null;
            var adUnitId = adUnit.unitID;
            var interstitialFullAd = new ABUInterstitialFullAd();
            var androidListener = new GMInterstitialFullAdLoadCallback(callback);
            AdHandle = ABUAdSDK.GetAdManager()?.CallStatic<AndroidJavaObject>(
                "loadInterstitialFullAd", ABUAdSDK.GetActivity(), adUnitId,
                adUnit.Handle,
                androidListener);
        }

        /// <summary>
        /// Sets the listener for the Ad download.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }

        /// <summary>
        /// Show this InterstitialFull Ad.
        /// </summary>
        internal static void ShowInteractionAd(ABUInterstitialFullAdInteractionCallback callback)
        {
            if (AdHandle == null)
            {
                return;
            }

            ABUAdSDK.GetAdManager()?.CallStatic(
                "showInterstitialFullAd", ABUAdSDK.GetActivity(), AdHandle, new GMInterstitialFullAdListener(callback));
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            
            if (AdHandle == null)
            {
                return "";
            }
            AndroidJavaObject gmAdEcpmInfo = AdHandle.Call<AndroidJavaObject>("getShowEcpm");
            if (gmAdEcpmInfo == null) 
            {
                return "";
            }
            string adnName = gmAdEcpmInfo.Call<string>("getAdNetworkPlatformName");
            return adnName;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            if (AdHandle == null)
            {
                return "";
            }
            return AdHandle.Call<string>("getAdNetworkRitId");
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public static string GetPreEcpm()
        {
            if (AdHandle == null)
            {
                return "";
            }
            return AdHandle.Call<string>("getPreEcpm");
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return AdHandle!=null && AdHandle.Call<bool>("isReady");
        }


        private class GMInterstitialFullAdLoadCallback : AndroidJavaProxy
        {
            private readonly ABUInterstitialFullAdCallback _callback;

            public GMInterstitialFullAdLoadCallback(ABUInterstitialFullAdCallback callback) : base(
                "com.bytedance.msdk.api.v2.ad.interstitialFull.GMInterstitialFullAdLoadCallback")
            {
                this._callback = callback;
            }


            /**
     * 广告加载失败
     */
            public void onInterstitialFullLoadFail(AndroidJavaObject adError)
            {
                UnityDispatcher.PostTask(
                    () => this._callback?.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
            }

            /**
     * 广告加载成功
     */
            public void onInterstitialFullAdLoad()
            {
                UnityDispatcher.PostTask(
                    () => this._callback?.OnInterstitialFullAdLoad(ABUInterstitialFullAd.AdHandle));
            }

            public void onInterstitialFullCached()
            {
                UnityDispatcher.PostTask(
                    () => this._callback?.OnInterstitialFullAdCached());
            }
        }

        private class GMInterstitialFullAdListener : AndroidJavaProxy
        {
            private ABUInterstitialFullAdInteractionCallback _callback;

            public GMInterstitialFullAdListener(
                ABUInterstitialFullAdInteractionCallback interstitialFullAdInteractionCallback) : base(
                "com.bytedance.msdk.api.v2.ad.interstitialFull.GMInterstitialFullAdListener")
            {
                _callback = interstitialFullAdInteractionCallback;
            }

            /**
     * 广告展示
     */
            public void onInterstitialFullShow()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnAdShow());
            }


            /**
     * show失败回调。如果show时发现无可用广告（比如广告过期或者isReady=false），会触发该回调。
     * 开发者应该在该回调里进行重新请求。
     *
     * @param adError showFail的具体原因
     */
            public void onInterstitialFullShowFail(AndroidJavaObject adError)
            {
                UnityDispatcher.PostTask(
                    () => this._callback?.OnAdShowFailed(adError.Get<int>("code"), adError.Get<string>("message")));
            }

            /**
     * 广告被点击
     */
            public void onInterstitialFullClick()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnAdClicked());
            }

            /**
     * 关闭广告
     */
            public void onInterstitialFullClosed()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnAdClose());
            }

            /**
     * 视频播放完毕的回调
     */
            public void onVideoComplete()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnVideoComplete());
            }

            /**
     * 1、视频播放失败的回调
     */
            public void onVideoError()
            {
            }

            /**
     * 跳过视频播放
     */
            public void onSkippedVideo()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnSkippedVideo());
            }

            /**
     * 当广告打开浮层时调用，如打开内置浏览器、内容展示浮层，一般发生在点击之后
     * 常常在onAdLeftApplication之前调用
     */
            public void onAdOpened()
            {
            }


            /**
     * 此方法会在用户点击打开其他应用（例如 Google Play）时
     * 于 onAdOpened() 之后调用，从而在后台运行当前应用。
     */
            public void onAdLeftApplication()
            {
            }

            /**
     * 全屏视频播放完毕，验证是否有效发放奖励的回调
     */
            public void onRewardVerify(AndroidJavaObject rewardItem)
            {
                Debug.Log("ABUFullScreenVideoAd OnSkippedVideo ");
                if (_callback == null)
                {
                    return;
                }

                if (rewardItem == null)
                {
                    this._callback.OnRewardVerify(false, null);
                    return;
                }

                bool verify = rewardItem.Call<bool>("rewardVerify");
                var result = ABUAdSDK.GetAdManager().CallStatic<string>("rewardItemToUnityJsonString", rewardItem);
                if (string.IsNullOrEmpty(result))
                {
                    this._callback.OnRewardVerify(verify, null);
                }

                this._callback.OnRewardVerify(verify, JsonUtility.FromJson<ABUAdapterRewardAdInfo>(result));
            }
        }
    }
#endif
}