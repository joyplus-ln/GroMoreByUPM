//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    //#if UNITY_IOS
#if !UNITY_EDITOR && UNITY_IOS
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Newtonsoft.Json;
    using UnityEngine;

    /// <summary>
    /// Set the interaction Ad.
    /// </summary>
    public sealed class ABUInterstitialFullAd : IDisposable
    {

        private static IntPtr interstitialFullAd;
        private bool disposed;

        private static int loadContextID = 0;
        private static Dictionary<int, ABUInterstitialFullAdCallback> loadListeners = new Dictionary<int, ABUInterstitialFullAdCallback>();
        private delegate void InterstitialFullAd_OnError(int code, string message, int context);
        private delegate void InterstitialFullAd_OnAdLoad(IntPtr interstitialFullAd, int context);
        private delegate void InterstitialFullAd_OnCached(int context);

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUInterstitialFullAdInteractionCallback> interactionListeners = new Dictionary<int, ABUInterstitialFullAdInteractionCallback>();
        private delegate void InterstitialFullAd_OnViewRenderFail(int code, string message, int context);
        private delegate void InterstitialFullAd_OnAdClicked(int context);
        private delegate void InterstitialFullAd_OnAdShow(int context);
        private delegate void InterstitialFullAd_OnAdShowFailed(int errcode, string errorMsg, int context);
        private delegate void InterstitialFullAd_OnSkippedVideo(int context);
        private delegate void InterstitialFullAd_OnAdClose(int context);
        private delegate void InterstitialFullAd_OnVideoComplete(int context);
        private delegate void InterstitialFullAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);
        private delegate void InterstitialFullAd_OnRewardVerify(bool rewardVerify, string rewardInfoJson, int context);

        ABUInterstitialFullAd(IntPtr interstitialFullAdPtr)
        {
            interstitialFullAd = interstitialFullAdPtr;
        }

        ~ABUInterstitialFullAd()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            this.disposed = true;
        }

        /// <summary>
        /// 异步加载广告
        /// Loads the InterstitialFull video ad.
        /// </summary>
        /// <param name="adUnit">ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadInterstitialFullAd(ABUAdUnit adUnit, ABUInterstitialFullAdCallback callback)
        {
            var context = loadContextID++;
            loadListeners.Add(context, callback);

            UnionPlatform_InterstitialFullAd_Load(
                ABUAdUnit.unitID,
                adUnit.width,
                adUnit.height,
                adUnit.muted,
                adUnit.userID,
                adUnit.RewardName,
                adUnit.RewardAmount,
                adUnit.ExtraInfo,
                InterstitialFullAd_OnErrorMethod,
                InterstitialFullAd_OnAdLoadMethod,
                InterstitialFullAd_OnCachedMethod,
                context);
        }

        /// <summary>
        /// Show this InterstitialFull Ad.
        /// </summary>
        internal static void ShowInteractionAd(ABUInterstitialFullAdInteractionCallback callback)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, callback);

            UnionPlatform_InterstitialFullAd_Show(
                interstitialFullAd,
                InterstitialFullAd_OnViewRenderFailMethod,
                InterstitialFullAd_OnAdClickedMethod,
                InterstitialFullAd_OnAdShowMethod,
                InterstitialFullAd_OnAdShowFailedMethod,
                InterstitialFullAd_OnSkippedVideoMethod,
                InterstitialFullAd_OnAdCloseMethod,
                InterstitialFullAd_OnVideoCompleteMethod,
                InterstitialFullAd_OnWaterfallRitFillFailMethod,
                InterstitialFullAd_OnRewardVerifyMethod,
                context);
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            return UnionPlatform_InterstitialFullAd_GetAdRitInfoAdnName(interstitialFullAd);
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            return UnionPlatform_InterstitialFullAd_GetAdNetworkRitId(interstitialFullAd);
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public static string GetPreEcpm()
        {
            return UnionPlatform_InterstitialFullAd_GetPreEcpm(interstitialFullAd);
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return UnionPlatform_InterstitialFullAd_isReady(interstitialFullAd);
        }

        [DllImport("__Internal")]
        private static extern void UnionPlatform_InterstitialFullAd_Dispose(IntPtr interstitialFullAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_InterstitialFullAd_Load(
            string adUnitID,
            float width,
            float height,
            bool mutedIfCan,
            string UserId,
            string RewardName,
            int RewardAmount,
            string ExtraInfo,
            InterstitialFullAd_OnError onError,
            InterstitialFullAd_OnAdLoad onAdLoad,
            InterstitialFullAd_OnCached onCached,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_InterstitialFullAd_Show(
            IntPtr interstitialFullAd,
            InterstitialFullAd_OnViewRenderFail onViewRenderFail,
            InterstitialFullAd_OnAdClicked onAdClicked,
            InterstitialFullAd_OnAdShow onAdShow,
            InterstitialFullAd_OnAdShowFailed onAdShowFailed,
            InterstitialFullAd_OnSkippedVideo onSkippedVideo,
            InterstitialFullAd_OnAdClose onAdClose,
            InterstitialFullAd_OnVideoComplete onVideoComplete,
            InterstitialFullAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            InterstitialFullAd_OnRewardVerify onRewardVerify,
            int context);

        [DllImport("__Internal")]
        private static extern bool UnionPlatform_InterstitialFullAd_isReady(IntPtr interstitialFullAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_InterstitialFullAd_GetAdRitInfoAdnName(IntPtr interstitialFullAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_InterstitialFullAd_GetAdNetworkRitId(IntPtr interstitialFullAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_InterstitialFullAd_GetPreEcpm(IntPtr interstitialFullAd);

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnError))]
        private static void InterstitialFullAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnAdLoad))]
        private static void InterstitialFullAd_OnAdLoadMethod(IntPtr interstitialFullAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    ABUInterstitialFullAd ad = new ABUInterstitialFullAd(interstitialFullAd);
                    listener.OnInterstitialFullAdLoad(ad);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnCached))]
        private static void InterstitialFullAd_OnCachedMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnInterstitialFullAdCached();
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnCached can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnViewRenderFail))]
        private static void InterstitialFullAd_OnViewRenderFailMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnViewRenderFail(code, message);
                    interactionListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnViewRenderFail can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnAdClicked))]
        private static void InterstitialFullAd_OnAdClickedMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked();
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnAdClicked can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnAdShow))]
        private static void InterstitialFullAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnAdShowFailed))]
        private static void InterstitialFullAd_OnAdShowFailedMethod(int errcode, string errorMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShowFailed(errcode, errorMsg);
                    interactionListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnAdShowFailed can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnSkippedVideo))]
        private static void InterstitialFullAd_OnSkippedVideoMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnSkippedVideo();
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnSkippedVideo can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnAdClose))]
        private static void InterstitialFullAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClose();
                    interactionListeners.Remove(context);
                    UnionPlatform_InterstitialFullAd_Dispose(interstitialFullAd);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnSkippedVideo can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnVideoComplete))]
        private static void InterstitialFullAd_OnVideoCompleteMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnVideoComplete();
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnVideoComplete can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnWaterfallRitFillFail))]
        private static void InterstitialFullAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnWaterfallRitFillFail(fillFailMessageInfo);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnWaterfallRitFillFail can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialFullAd_OnRewardVerify))]
        private static void InterstitialFullAd_OnRewardVerifyMethod(bool rewardVerify, string rewardInfoJson, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialFullAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    // json反序列化
                    ABUAdapterRewardAdInfo rewardAdInfo = JsonConvert.DeserializeObject<ABUAdapterRewardAdInfo>(rewardInfoJson);
                    listener.OnRewardVerify(rewardVerify, rewardAdInfo);
                }
                else
                {
                    Debug.LogError("The InterstitialFullAd_OnRewardVerify can not find the context.");
                }
            });
        }
    }
#endif
}
