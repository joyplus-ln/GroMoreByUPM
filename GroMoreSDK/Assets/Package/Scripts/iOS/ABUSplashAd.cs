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
    /// The splash Ad.
    /// </summary>
    public class ABUSplashAd : IDisposable
    {

        private static int loadContextID = 0;
        private static Dictionary<int, ABUSplashAdListener> loadListeners =
            new Dictionary<int, ABUSplashAdListener>();

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUSplashAdInteractionListener> interactionListeners =
            new Dictionary<int, ABUSplashAdInteractionListener>();

        private delegate void SplashAd_OnError(int code, string message, int context);
        private delegate void SplashAd_OnLoad(IntPtr splashAd, int context);

        private delegate void SplashAd_OnAdShow(int context);
        private delegate void SplashAd_OnAdClick(int context);
        private delegate void SplashAd_OnAdClose(int context);
        private delegate void SplashAd_OnAdSkip(int context);
        private delegate void SplashAd_OnAdTimeOver(int context);
        private delegate void SplashAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);
        private delegate void SplashAd_OnAdShowFailed(int code, string message, int context);

        private static IntPtr splashAd;
        private bool disposed;
        private static ABUAdUnit internalUnitAd;

        public ABUSplashAd ()
        {
        }

        private ABUSplashAd(IntPtr splash)
        {
            splashAd = splash;
        }

        ~ABUSplashAd()
        {
            this.Dispose(false);
        }

        public ABUAdUnit getAdUnit()
        {
            return internalUnitAd;
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
        /// 设置开屏兜底配置
        /// Loads the splash ad.
        /// </summary>
        // <param name="adnType">adnType.</param>
        /// <param name="appID">appID.</param>
        /// <param name="ritID">ritID.</param> 
        internal static void SetUserData(ABUAdnType adnType, string appID, string ritID)
        {
            UnionPlatform_SplashAd_SetUserData(
                (int)adnType,
                appID,
                ritID
                );
        }

        /// <summary>
        /// Load the  splash Ad.
        /// </summary>
        internal static void LoadSplashAd(ABUAdUnit adUnit, ABUSplashAdListener listener, int timeOut)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);
            internalUnitAd = adUnit;

            UnionPlatform_SplashAd_Load(
                ABUAdUnit.unitID,
                timeOut,
                adUnit.splashButtonType,
                SplashAd_OnErrorMethod,
                SplashAd_OnLoadMethod,
                context);
        }

  

        /// <summary>
        /// Sets the listener for the Ad download.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }

        /// <summary>
        /// Show the full screen video.
        /// </summary>
        internal static void ShowSplashAd(ABUSplashAdInteractionListener splashAdInteractionListener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, splashAdInteractionListener);

            UnionPlatform_SplashAd_Show(
                splashAd,
                SplashAd_OnAdShowMethod,
                SplashAd_OnAdClickMethod,
                SplashAd_OnAdCloseMethod,
                SplashAd_OnAdSkipMethod,
                SplashAd_OnAdTimeOverMethod,
                SplashAd_OnWaterfallRitFillFailMethod,
                SplashAd_OnAdShowFailedMethod,
                context);
        }

        // 获取最佳广告的adn类型
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            return ABUAdnType.ABUAdnNone;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            return UnionPlatform_SplashAd_GetAdRitInfoAdnName(splashAd);
        }

        // 获取最佳广告的代码位
        public static string GetAdNetworkRitId()
        {
            return UnionPlatform_SplashAd_GetAdNetworkRitId(splashAd);
        }

        // 获取最佳广告的预设ecpm
        public static string GetPreEcpm()
        {
            return UnionPlatform_SplashAd_GetPreEcpm(splashAd);
        }

        [DllImport("__Internal")]
        private static extern string UnionPlatform_SplashAd_GetAdRitInfoAdnName(
            IntPtr splashAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_SplashAd_GetAdNetworkRitId(
            IntPtr splashAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_SplashAd_GetPreEcpm(
            IntPtr splashAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SplashAd_SetUserData(
            int adnType,
            string appID,
            string ritID);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SplashAd_Load(
            string slotID,
            int timeOut,
            ABUSplashButtonType splashButtonType,
            SplashAd_OnError onError,
            SplashAd_OnLoad onAdLoad,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SplashAd_SetInteractionListener(
            IntPtr splashAd,
            SplashAd_OnAdShow onShow,
            SplashAd_OnAdClick onAdClick,
            SplashAd_OnAdClose onClose,
            SplashAd_OnAdSkip onAdSkip,
            SplashAd_OnAdTimeOver onAdTimeOver,
            SplashAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            SplashAd_OnAdShowFailed onAdShowFailed,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SplashAd_Show(
            IntPtr splashAd,
            SplashAd_OnAdShow onShow,
            SplashAd_OnAdClick onAdClick,
            SplashAd_OnAdClose onClose,
            SplashAd_OnAdSkip onAdSkip,
            SplashAd_OnAdTimeOver onAdTimeOver,
            SplashAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            SplashAd_OnAdShowFailed onAdShowFailed,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SplashAd_Dispose(
            IntPtr splashAd);



        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnError))]
        private static void SplashAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdListener listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnError can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnLoad))]
        private static void SplashAd_OnLoadMethod(IntPtr splashAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdListener listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnSplashAdLoad(new ABUSplashAd(splashAd));
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnLoad can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnAdShow))]
        private static void SplashAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnAdShow can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnAdClick))]
        private static void SplashAd_OnAdClickMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    
                    listener.OnAdClicked();
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnAdClick can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnAdClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnAdClose))]
        private static void SplashAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClose();
                    interactionListeners.Remove(context);
                    UnionPlatform_SplashAd_Dispose(splashAd);
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnAdClose can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnAdClose can not find the context.");
                }
            });
        }
        
        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnAdSkip))]
        private static void SplashAd_OnAdSkipMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdSkip();
                    interactionListeners.Remove(context);
                    UnionPlatform_SplashAd_Dispose(splashAd);
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnAdSkip can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnAdSkip can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnAdTimeOver))]
        private static void SplashAd_OnAdTimeOverMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdTimeOver();
                }
                else
                {
                    Debug.LogError(
                        "The SplashAd_OnAdTimeOver can not find the context.");
                    //ToastManager.Instance.ShowToast("The SplashAd_OnAdTimeOver can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnWaterfallRitFillFail))]
        private static void SplashAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
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

        [AOT.MonoPInvokeCallback(typeof(SplashAd_OnAdShowFailed))]
        private static void SplashAd_OnAdShowFailedMethod(int errCode , string errMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUSplashAdInteractionListener listener;
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
