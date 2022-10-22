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

    /// <summary>
    /// The banner Ad.
    /// </summary>
    public sealed class ABUBannerAd : IDisposable
    {

        private float width;
        private float height;

        private static int loadContextID = 0;
        private static Dictionary<int, ABUBannerAdCallback> loadListeners =
            new Dictionary<int, ABUBannerAdCallback>();

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUBannerAdInteractionCallback> interactionListeners =
            new Dictionary<int, ABUBannerAdInteractionCallback>();

        private static ABUBannerAdInteractionCallback interactionListener;
        private delegate void BannerAd_OnError(int code, string message, int context);
        private delegate void BannerAd_OnAdLoad(IntPtr bannerAd, float width, float height, int context);

        private delegate void BannerAd_OnAdShow(int context);
        private delegate void BannerAd_OnAdClick(int context);
        private delegate void BannerAd_OnAdClose(int context);
        private delegate void BannerAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);

        private static IntPtr bannerAd;
        private bool disposed;

        public float getBannerViewWidth()
        {
            return this.width;
        }

        public float getBannerViewHeight()
        {
            return this.height;
        }

        ABUBannerAd(IntPtr banner)
        {
            bannerAd = banner;
        }

        ~ABUBannerAd()
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

            UnionPlatform_BannerAd_Dispose(bannerAd);
            this.disposed = true;
        }

        /// <summary>
        /// 异步加载广告
        /// Loads the reward video ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadBannerAd(
            ABUAdUnit adUnit, ABUBannerAdCallback listener, ABUBannerAdInteractionCallback InteractionListener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);
            interactionListener = InteractionListener;

            UnionPlatform_BannerAd_Load(
                ABUAdUnit.unitID,
                adUnit.width,
                adUnit.height,
                adUnit.autoRefreshTime,
                adUnit.muted,
                BannerAd_OnErrorMethod,
                BannerAd_OnAdLoadMethod,
                BannerAd_OnWaterfallRitFillFailMethod,
                context);
        }

        /// <summary>
        /// show the   Ad
        /// <param name="x">the origin x of th ad</param>
        /// <param name="y">the origin y of th ad</param>
        /// </summary>
        public void ShowBannerAd(float x, float y)
        {
            UnionPlatform_BannerAd_Show(
                bannerAd,
                x,
                y);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetBannerInteractionListener(
            ABUBannerAdInteractionCallback listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);

            UnionPlatform_BannerAd_SetInteractionListener(
                bannerAd,
                BannerAd_OnAdShowMethod,
                BannerAd_OnAdClickMethod,
                BannerAd_OnAdCloseMethod,
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
        /// Sets the show dislike icon.
        /// </summary>
        public void SetShowDislikeIcon(ABUDislikeInteractionListener listener)
        {
        }

        /// <summary>
        /// Gets the dislike dislog.
        /// </summary>
        public ABUAdDislike GetDislikeDialog(ABUDislikeInteractionListener listener)
        {
            var dislike = new ABUAdDislike();
            dislike.SetDislikeInteractionCallback(listener);
            return dislike;
        }

        /// <summary>
        /// Sets the slide interval time.
        /// </summary>
        public void SetSlideIntervalTime(int interval)
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
            return UnionPlatform_BannerAd_GetAdRitInfoAdnName(bannerAd);
        }

        // 获取最佳广告的代码位
        public static string GetAdNetworkRitId()
        {
            return UnionPlatform_BannerAd_GetAdNetworkRitId(bannerAd);
        }

        // 获取最佳广告的预设ecpm
        public static string GetPreEcpm()
        {
            return UnionPlatform_BannerAd_GetPreEcpm(bannerAd);
        }

        [DllImport("__Internal")]
        private static extern string UnionPlatform_BannerAd_GetAdRitInfoAdnName(
            IntPtr bannerAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_BannerAd_GetAdNetworkRitId(
            IntPtr bannerAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_BannerAd_GetPreEcpm(
            IntPtr bannerAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_BannerAd_Load(
            string adUnitID,
            float width,
            float height,
            int autoRefreshTime,
            bool muted,
            BannerAd_OnError onError,
            BannerAd_OnAdLoad onAdLoad,
            BannerAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_BannerAd_SetInteractionListener(
            IntPtr bannerAd,
            BannerAd_OnAdShow onAdShow,
            BannerAd_OnAdClick onAdClick,
            BannerAd_OnAdClose onAdClose,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_BannerAd_Dispose(
            IntPtr bannerAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_BannerAd_Show(
            IntPtr bannerAd,
            float x,
            float y);


        [AOT.MonoPInvokeCallback(typeof(BannerAd_OnError))]
        private static void BannerAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUBannerAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The BannerAd_OnErrorMethod can not find the context.");
                    //ToastManager.Instance.ShowToast("The BannerAd_OnErrorMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(BannerAd_OnAdLoad))]
        private static void BannerAd_OnAdLoadMethod(IntPtr bannerAd, float width, float height, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUBannerAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    ABUBannerAd banner = new ABUBannerAd(bannerAd);
                    banner.width = width;
                    banner.height = height;

                    listener.OnBannerAdLoad(banner);

                    var context1 = interactionContextID++;
                    interactionListeners.Add(context1, interactionListener);
                    UnionPlatform_BannerAd_SetInteractionListener(
                        bannerAd,
                        BannerAd_OnAdShowMethod,
                        BannerAd_OnAdClickMethod,
                        BannerAd_OnAdCloseMethod,
                        context1);
                }
                else
                {
                    Debug.LogError(
                        "The OnFullScreenVideoAdLoad can not find the context.");
                    //ToastManager.Instance.ShowToast("The OnFullScreenVideoAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(BannerAd_OnAdShow))]
        private static void BannerAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUBannerAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError(
                        "The BannerAd_OnAdShow can not find the context.");
                    //ToastManager.Instance.ShowToast("The BannerAd_OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(BannerAd_OnAdClick))]
        private static void BannerAd_OnAdClickMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUBannerAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked();
                }
                else
                {
                    Debug.LogError(
                        "The BannerAd_OnAdClick can not find the context.");
                    //ToastManager.Instance.ShowToast("The BannerAd_OnAdClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(BannerAd_OnAdClose))]
        private static void BannerAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUBannerAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    UnionPlatform_BannerAd_Dispose(bannerAd);
                    listener.OnAdDismiss();
                    interactionListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The BannerAd_OnAdClose can not find the context.");
                    //ToastManager.Instance.ShowToast("The BannerAd_OnAdClose can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(BannerAd_OnWaterfallRitFillFail))]
        private static void BannerAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUBannerAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnWaterfallRitFillFail(fillFailMessageInfo);
                }
                else
                {
                    Debug.LogError(
                        "The BannerAd_OnAdClose can not find the context.");
                    //ToastManager.Instance.ShowToast("The BannerAd_OnAdClose can not find the context.");
                }
            });
        }

    }

#endif

}
