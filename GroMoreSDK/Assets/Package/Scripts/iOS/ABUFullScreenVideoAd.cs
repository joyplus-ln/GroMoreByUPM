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
    using UnityEngine;
    using Newtonsoft.Json;

    /// <summary>
    /// The fullScreen video Ad.
    /// </summary>
    public sealed class ABUFullScreenVideoAd : IDisposable
    {
        private static int loadContextID = 0;
        private static Dictionary<int, ABUFullScreenVideoAdCallback> loadListeners =
            new Dictionary<int, ABUFullScreenVideoAdCallback>();

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUFullScreenVideoAdInteractionCallback> interactionListeners =
            new Dictionary<int, ABUFullScreenVideoAdInteractionCallback>();

        private delegate void FullScreenVideoAd_OnError(int code, string message, int context);
        private delegate void FullScreenVideoAd_OnFullScreenVideoAdLoad(IntPtr fullScreenVideoAd, int context);
        private delegate void FullScreenVideoAd_OnFullScreenVideoCached(int context);

        private delegate void FullScreenVideoAd_OnAdShow(int context);
        private delegate void FullScreenVideoAd_OnAdVideoBarClick(int context);
        private delegate void FullScreenVideoAd_OnAdVideoSkip(int context);
        private delegate void FullScreenVideoAd_OnAdClose(int context);
        private delegate void FullScreenVideoAd_OnVideoComplete(int context);
        private delegate void FullScreenVideoAd_OnVideoError(int code, string message, int context);
        private delegate void FullScreenVideoAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);
        private delegate void FullScreenVideoAd_OnAdShowFailed(int code, string message, int context);
        private delegate void FullScreenVideoAd_OnRewardVerify(bool rewardVerify, string rewardInfoJson, int context);

        private static IntPtr fullScreenVideoAd;
        private bool disposed;

        internal ABUFullScreenVideoAd(IntPtr fullScreenVideoAdPtr)
        {
            fullScreenVideoAd = fullScreenVideoAdPtr;
        }

        ~ABUFullScreenVideoAd()
        {
            this.Dispose(false);
        }

        internal static void LoadFullScreenVideoAd(
            ABUAdUnit adUnit, ABUFullScreenVideoAdCallback listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            UnionPlatform_FullScreenVideoAd_Load(
                ABUAdUnit.unitID,
                adUnit.useExpress2IfCanForGDT,
                FullScreenVideoAd_OnErrorMethod,
                FullScreenVideoAd_OnFullScreenVideoAdLoadMethod,
                FullScreenVideoAd_OnFullScreenVideoCachedMethod,
                context);
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

            //UnionPlatform_FullScreenVideoAd_Dispose(fullScreenVideoAd);
            this.disposed = true;
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetFullScreenVideoAdInteractionListener(ABUFullScreenVideoAdInteractionCallback listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);

            UnionPlatform_FullScreenVideoAd_SetInteractionListener(
                fullScreenVideoAd,
                FullScreenVideoAd_OnAdShowMethod,
                FullScreenVideoAd_OnAdVideoBarClickMethod,
                FullScreenVideoAd_OnAdCloseMethod,
                FullScreenVideoAd_OnVideoCompleteMethod,
                FullScreenVideoAd_OnVideoErrorMethod,
                FullScreenVideoAd_OnWaterfallRitFillFailMethod,
                FullScreenVideoAd_OnAdShowFailedMethod,
                FullScreenVideoAd_OnAdVideoSkipMethod,
                FullScreenVideoAd_OnRewardVerifyMethod,
                context);
        }

        /// <summary>
        /// Sets the download listener.
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

        /// <summary>
        /// Show the fullScreen video Ad.
        /// </summary>
        internal static void ShowFullVideoAd(ABUFullScreenVideoAdInteractionCallback callback)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, callback);

            UnionPlatform_FullScreenVideoAd_ShowFullScreenVideoAd(
                fullScreenVideoAd,
                null,
                FullScreenVideoAd_OnAdShowMethod,
                FullScreenVideoAd_OnAdVideoBarClickMethod,
                FullScreenVideoAd_OnAdCloseMethod,
                FullScreenVideoAd_OnVideoCompleteMethod,
                FullScreenVideoAd_OnVideoErrorMethod,
                FullScreenVideoAd_OnWaterfallRitFillFailMethod,
                FullScreenVideoAd_OnAdShowFailedMethod,
                FullScreenVideoAd_OnAdVideoSkipMethod,
                FullScreenVideoAd_OnRewardVerifyMethod,
                context);
        }

        /// <summary>
        /// Show the full screen video.
        /// extro 额外信息，主要用于ritscene
        /// </summary>
        internal static void ShowFullVideoAdWithRitScene(ABUFullScreenVideoAdInteractionCallback callback, Dictionary<string, object> extro)
        {
            // 设置回调
            var context = interactionContextID++;
            interactionListeners.Add(context, callback);

            string extroStr = JsonConvert.SerializeObject(extro);
            UnionPlatform_FullScreenVideoAd_ShowFullScreenVideoAd(
                fullScreenVideoAd,
                extroStr,
                FullScreenVideoAd_OnAdShowMethod,
                FullScreenVideoAd_OnAdVideoBarClickMethod,
                FullScreenVideoAd_OnAdCloseMethod,
                FullScreenVideoAd_OnVideoCompleteMethod,
                FullScreenVideoAd_OnVideoErrorMethod,
                FullScreenVideoAd_OnWaterfallRitFillFailMethod,
                FullScreenVideoAd_OnAdShowFailedMethod,
                FullScreenVideoAd_OnAdVideoSkipMethod,
                FullScreenVideoAd_OnRewardVerifyMethod,
                context);
        }

        /// <summary>
        /// Sets whether to show the download bar.
        /// </summary>
        public void SetShowDownLoadBar(bool show)
        {
        }

        // 获取最佳广告的adn类型
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            return ABUAdnType.ABUAdnNoPermission;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            return UnionPlatform_FullScreenVideoAd_GetAdRitInfoAdnName(fullScreenVideoAd);
        }

        // 获取最佳广告的代码位
        public static string GetAdNetworkRitId()
        {
            return UnionPlatform_FullScreenVideoAd_GetAdNetworkRitId(fullScreenVideoAd);
        }

        // 获取最佳广告的预设ecpm
        public static string GetPreEcpm()
        {
            return UnionPlatform_FullScreenVideoAd_GetPreEcpm(fullScreenVideoAd);
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return UnionPlatform_FullScreenVideoAd_isReady(fullScreenVideoAd);
        }

        [DllImport("__Internal")]
        private static extern bool UnionPlatform_FullScreenVideoAd_isReady(
            IntPtr fullScreenVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_FullScreenVideoAd_GetAdRitInfoAdnName(
            IntPtr fullScreenVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_FullScreenVideoAd_GetAdNetworkRitId(
            IntPtr fullScreenVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_FullScreenVideoAd_GetPreEcpm(
            IntPtr fullScreenVideoAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_FullScreenVideoAd_Load(
            string adUnitID,
            bool useExpress2IfCanForGDT,
            FullScreenVideoAd_OnError onError,
            FullScreenVideoAd_OnFullScreenVideoAdLoad onFullScreenVideoAdLoad,
            FullScreenVideoAd_OnFullScreenVideoCached onFullScreenVideoCached,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_FullScreenVideoAd_SetInteractionListener(
            IntPtr fullScreenVideoAd,
            FullScreenVideoAd_OnAdShow onAdShow,
            FullScreenVideoAd_OnAdVideoBarClick onAdVideoBarClick,
            FullScreenVideoAd_OnAdClose onAdClose,
            FullScreenVideoAd_OnVideoComplete onVideoComplete,
            FullScreenVideoAd_OnVideoError onVideoError,
            FullScreenVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            FullScreenVideoAd_OnAdShowFailed onAdShowFailed,
            FullScreenVideoAd_OnAdVideoSkip onAdVideoSkip,
            FullScreenVideoAd_OnRewardVerify onRewardVerify,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_FullScreenVideoAd_ShowFullScreenVideoAd(
            IntPtr fullScreenVideoAd,
            string extro,
            FullScreenVideoAd_OnAdShow onAdShow,
            FullScreenVideoAd_OnAdVideoBarClick onAdVideoBarClick,
            FullScreenVideoAd_OnAdClose onAdClose,
            FullScreenVideoAd_OnVideoComplete onVideoComplete,
            FullScreenVideoAd_OnVideoError onVideoError,
            FullScreenVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            FullScreenVideoAd_OnAdShowFailed onAdShowFailed,
            FullScreenVideoAd_OnAdVideoSkip onAdVideoSkip,
            FullScreenVideoAd_OnRewardVerify onRewardVerify,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_FullScreenVideoAd_Dispose(
            IntPtr fullScreenVideoAd);

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnError))]
        private static void FullScreenVideoAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The OnError can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnFullScreenVideoAdLoad))]
        private static void FullScreenVideoAd_OnFullScreenVideoAdLoadMethod(IntPtr fullScreenVideoAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnFullScreenVideoAdLoad(new ABUFullScreenVideoAd(fullScreenVideoAd));
                }
                else
                {
                    Debug.LogError(
                        "The OnFullScreenVideoAdLoad can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnFullScreenVideoAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnFullScreenVideoCached))]
        private static void FullScreenVideoAd_OnFullScreenVideoCachedMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnFullScreenVideoAdCached();
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The OnFullScreenVideoAdCached can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnFullScreenVideoCached can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnAdShow))]
        private static void FullScreenVideoAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError(
                        "The OnAdShow can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnAdVideoBarClick))]
        private static void FullScreenVideoAd_OnAdVideoBarClickMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdVideoBarClick();
                }
                else
                {
                    Debug.LogError(
                        "The OnAdVideoBarClick can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnAdVideoBarClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnAdClose))]
        private static void FullScreenVideoAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClose();
                    interactionListeners.Remove(context);

                    UnionPlatform_FullScreenVideoAd_Dispose(fullScreenVideoAd);
                }
                else
                {
                    Debug.LogError(
                        "The OnAdClose can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnAdClose can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnVideoComplete))]
        private static void FullScreenVideoAd_OnVideoCompleteMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnVideoComplete();
                }
                else
                {
                    Debug.LogError(
                        "The OnVideoComplete can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnVideoComplete can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnVideoError))]
        private static void FullScreenVideoAd_OnVideoErrorMethod(int errCode, string errMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnVideoError(errCode, errMsg);
                }
                else
                {
                    Debug.LogError(
                        "The OnVideoError can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnVideoError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnWaterfallRitFillFail))]
        private static void FullScreenVideoAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnWaterfallRitFillFail(fillFailMessageInfo);
                }
                else
                {
                    Debug.LogError(
                        "The OnWaterfallRitFillFail can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnWaterfallRitFillFail can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnAdShowFailed))]
        private static void FullScreenVideoAd_OnAdShowFailedMethod(int errCode , string errMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShowFailed(errCode, errMsg);
                }
                else
                {
                    Debug.LogError(
                        "The OnAdShowFailed can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnAdShowFailed can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnAdVideoSkip))]
        private static void FullScreenVideoAd_OnAdVideoSkipMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnSkippedVideo();
                }
                else
                {
                    Debug.LogError(
                        "The OnAdShowFailed can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnAdShowFailed can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(FullScreenVideoAd_OnRewardVerify))]
        private static void FullScreenVideoAd_OnRewardVerifyMethod(bool rewardVerify, string rewardInfoJson, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUFullScreenVideoAdInteractionCallback listener;
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
