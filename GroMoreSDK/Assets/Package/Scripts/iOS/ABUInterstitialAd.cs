//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_IOS
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    /// <summary>
    /// Set the interaction Ad.
    /// </summary>
    public sealed class ABUInterstitialAd : IDisposable
    {
        private bool disposed;

        ABUInterstitialAd(IntPtr interstitialAdPtr)
        {
            interstitialAd = interstitialAdPtr;
        }

        ~ABUInterstitialAd()
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

        private static int loadContextID = 0;
        private static Dictionary<int, ABUInterstitialAdCallback> loadListeners = new Dictionary<int, ABUInterstitialAdCallback>();

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUInterstitialAdInteractionCallback> interactionListeners = new Dictionary<int, ABUInterstitialAdInteractionCallback>();

        private delegate void InterstitialAd_OnError(int code, string message, int context);
        private delegate void InterstitialAd_OnAdLoad(IntPtr interstitialAd, int context);

        private delegate void InterstitialAd_OnAdShow(int context);
        private delegate void InterstitialAd_OnAdClick(int context);
        private delegate void InterstitialAd_OnAdClose(int context);
        private delegate void InterstitialAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);
        private delegate void InterstitialAd_OnAdShowFailed(int code, string message, int context);

        private static IntPtr interstitialAd;

        /// <summary>
        /// 异步加载广告
        /// Loads the Interstitial ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadInterstitialAd(ABUAdUnit adUnit, ABUInterstitialAdCallback listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            UnionPlatform_Interstitial_Load(
                ABUAdUnit.unitID,
                adUnit.useExpress2IfCanForGDT,
                adUnit.width,
                adUnit.height,
                InterstitialAd_OnErrorMethod,
                InterstitialAd_OnAdLoadMethod,
                context);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetAdInteractionListener(ABUInterstitialAdInteractionCallback listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);

            UnionPlatform_Interstitial_SetInteractionListener(
                interstitialAd,
                InterstitialAd_OnAdShowMethod,
                InterstitialAd_OnAdClickMethod,
                InterstitialAd_OnAdCloseMethod,
                InterstitialAd_OnWaterfallRitFillFailMethod,
                InterstitialAd_OnAdShowFailedMethod,
                context);
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

        /// <summary>
        /// Show this interaction Ad.
        /// </summary>
        internal static void ShowInteractionAd(ABUInterstitialAdInteractionCallback callback)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, callback);

            UnionPlatform_Interstitial_Show(
                interstitialAd,
                InterstitialAd_OnAdShowMethod,
                InterstitialAd_OnAdClickMethod,
                InterstitialAd_OnAdCloseMethod,
                InterstitialAd_OnWaterfallRitFillFailMethod,
                InterstitialAd_OnAdShowFailedMethod,
                context);
        }

        // 获取最佳广告的adn类型
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            return ABUAdnType.ABUAdnNoPermission;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            return UnionPlatform_Interstitial_GetAdRitInfoAdnName(interstitialAd);
        }

        // 获取最佳广告的代码位
        public static string GetAdNetworkRitId()
        {
            return UnionPlatform_Interstitial_GetAdNetworkRitId(interstitialAd);
        }

        // 获取最佳广告的预设ecpm
        public static string GetPreEcpm()
        {
            return UnionPlatform_Interstitial_GetPreEcpm(interstitialAd);
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return UnionPlatform_Interstitial_isReady(interstitialAd);
        }

        [DllImport("__Internal")]
        private static extern bool UnionPlatform_Interstitial_isReady(
            IntPtr fullScreenVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_Interstitial_GetAdRitInfoAdnName(
            IntPtr interstitialAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_Interstitial_GetAdNetworkRitId(
            IntPtr interstitialAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_Interstitial_GetPreEcpm(
            IntPtr interstitialAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_Interstitial_Load(
            string adUnitID,
            bool useExpress2IfCanForGDT,
            float width,
            float height,
            InterstitialAd_OnError onError,
            InterstitialAd_OnAdLoad onAdLoad,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_Interstitial_SetInteractionListener(
            IntPtr interstitialAd,
            InterstitialAd_OnAdShow onAdShow,
            InterstitialAd_OnAdClick onAdClick,
            InterstitialAd_OnAdClose onAdClose,
            InterstitialAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            InterstitialAd_OnAdShowFailed onAdShowFailed,
            int context);


        [DllImport("__Internal")]
        private static extern void UnionPlatform_Interstitial_Dispose(
            IntPtr interstitialAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_Interstitial_Show(
            IntPtr interstitialAd,
            InterstitialAd_OnAdShow onAdShow,
            InterstitialAd_OnAdClick onAdClick,
            InterstitialAd_OnAdClose onAdClose,
            InterstitialAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            InterstitialAd_OnAdShowFailed onAdShowFailed,
            int context);

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnError))]
        private static void InterstitialAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The InterstitialAd_OnError can not find the context.");
                    //ToastManager.Instance.ShowToast("The InterstitialAd_OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnAdLoad))]
        private static void InterstitialAd_OnAdLoadMethod(IntPtr interstitialAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {

                    ABUInterstitialAd interstitial = new ABUInterstitialAd(interstitialAd);

                    listener.OnInterstitialAdLoad(interstitial);
                }
                else
                {
                    Debug.LogError(
                        "The InterstitialAd_OnAdLoad can not find the context.");
                    //ToastManager.Instance.ShowToast("The InterstitialAd_OnAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnAdShow))]
        private static void InterstitialAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError(
                        "The InterstitialAd_OnAdShow can not find the context.");
                    //ToastManager.Instance.ShowToast("The InterstitialAd_OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnAdClick))]
        private static void InterstitialAd_OnAdClickMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked();
                }
                else
                {
                    Debug.LogError(
                        "The InterstitialAd_OnAdClick can not find the context.");
                    //ToastManager.Instance.ShowToast("The InterstitialAd_OnAdClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnAdClose))]
        private static void InterstitialAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdDismiss();
                    interactionListeners.Remove(context);
                    UnionPlatform_Interstitial_Dispose(interstitialAd);
                }
                else
                {
                    Debug.LogError(
                        "The InterstitialAd_OnAdClose can not find the context.");
                    //ToastManager.Instance.ShowToast("The InterstitialAd_OnAdClose can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnWaterfallRitFillFail))]
        private static void InterstitialAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdInteractionCallback listener;
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

        [AOT.MonoPInvokeCallback(typeof(InterstitialAd_OnAdShowFailed))]
        private static void InterstitialAd_OnAdShowFailedMethod(int errCode , string errMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUInterstitialAdInteractionCallback listener;
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
     
    }
#endif
}
