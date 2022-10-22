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
    /// The native Ad.
    /// </summary>
    public sealed class ABUNativeAd : IDisposable
    {
        private static int loadContextID = 0;
        private static Dictionary<int, ABUNativeAdCallback> loadListeners = new Dictionary<int, ABUNativeAdCallback>();
        private delegate void NativeAd_OnError(int code, string message, int context);
        private delegate void NativeAd_OnNativeAdLoad(IntPtr nativeAd, int adCount, int context);
        private delegate void NativeAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUNativeAdInteractionCallback> interactionListeners = new Dictionary<int, ABUNativeAdInteractionCallback>();
        private static Dictionary<int, IntPtr> nativeAds = new Dictionary<int, IntPtr>();
        private delegate void NativeAd_OnAdShow(int index, int context);
        private delegate void NativeAd_OnAdDidClick(int index, int context);
        private delegate void NativeAd_OnAdClose(int index, int context);
        private delegate void NativeAd_OnAllAdClose(int context);

        private IntPtr nativeAd;
        private List<int> adInternalIndexs = new List<int>();
        private bool disposed;

        internal ABUNativeAd(IntPtr ad)
        {
            nativeAd = ad;
        }

        ~ABUNativeAd()
        {
            this.Dispose(false);
        }

        public void ShowNativeAd(int index, float x, float y)
        {
            UnionPlatform_NativeAd_ShowNativeAd(nativeAd, index, x, y);
        }

        public List<int> getAdIndexs()
        {
            return this.adInternalIndexs;
        }

        internal static void LoadNativeAd(ABUAdUnit adUnit, ABUNativeAdCallback listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            UnionPlatform_NativeAd_Load(
                NativeAd_OnErrorMethod,
                NativeAd_OnNativeAdLoadMethod,
                NativeAd_OnWaterfallRitFillFailMethod,
                context,
                ABUAdUnit.unitID,
                adUnit.adCount,
                adUnit.width,
                adUnit.height,
                adUnit.getExpress,
                adUnit.useExpress2IfCanForGDT,
                adUnit.muted);
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

        public void SetAdInteractionListener(ABUNativeAdInteractionCallback listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);
            UnionPlatform_NativeAd_SetInteractionListener(
                nativeAd,
                NativeAd_OnAdShowMethod,
                NativeAd_OnAdDidClickMethed,
                NativeAd_OnAdCloseMethod,
                NativeAd_OnAllAdCloseMethod,
                context);
        }

        // 获取最佳广告的adn类型
        public ABUAdnType GetAdNetworkPlaformId(int index)
        {
            return ABUAdnType.ABUAdnNoPermission;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public string GetAdRitInfoAdnName(int index)
        {
            return UnionPlatform_NativeAd_GetAdRitInfoAdnName(nativeAd, index);
        }

        // 获取最佳广告的代码位
        public string GetAdNetworkRitId(int index)
        {
            return UnionPlatform_NativeAd_GetAdNetworkRitId(nativeAd, index);
        }

        // 获取最佳广告的预设ecpm
        public string GetPreEcpm(int index)
        {
            return UnionPlatform_NativeAd_GetPreEcpm(nativeAd, index);
        }

        [DllImport("__Internal")]
        private static extern string UnionPlatform_NativeAd_GetAdRitInfoAdnName(
            IntPtr nativeAd,
            int index);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_NativeAd_GetAdNetworkRitId(
            IntPtr nativeAd,
            int index);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_NativeAd_GetPreEcpm(
            IntPtr nativeAd,
            int index);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_Dispose(
            IntPtr nativeAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_Load(
            NativeAd_OnError onError,
            NativeAd_OnNativeAdLoad onNativeAdLoad,
            NativeAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            int context,
            string slotID,
            int adCount,
            float width,
            float height,
            bool getExpress,
            bool useExpress2IfCanForGDT,
            bool muted);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_ShowNativeAd(
            IntPtr nativeAd,
            int index,
            float x,
            float y);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_SetInteractionListener(
            IntPtr nativeAd,
            NativeAd_OnAdShow onAdShow,
            NativeAd_OnAdDidClick onAdNativeClick,
            NativeAd_OnAdClose onAdClose,
            NativeAd_OnAllAdClose onAllAdCloseMethod,
            int context);

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnError))]
        private static void NativeAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnNativeAdLoad))]
        private static void NativeAd_OnNativeAdLoadMethod(IntPtr nativeAd, int adCount, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    var ad = new ABUNativeAd(nativeAd);
                    for (int i = 0; i < adCount; i++)
                    {
                        ad.adInternalIndexs.Add(i);
                    }
                    nativeAds.Add(context, nativeAd);
                    listener.OnNativeAdLoad(ad);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The NativeAd_OnNativeAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnWaterfallRitFillFail))]
        private static void NativeAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnWaterfallRitFillFail(fillFailMessageInfo);
                }
                else
                {
                    Debug.LogError("The OnWaterfallRitFillFail can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAdShow))]
        private static void NativeAd_OnAdShowMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow(index);
                }
                else
                {
                    Debug.LogError("The OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAdDidClick))]
        private static void NativeAd_OnAdDidClickMethed(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked(index);
                }
                else
                {
                    Debug.LogError("The OnAdVideoBarClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAdClose))]
        private static void NativeAd_OnAdCloseMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdDismiss(index);
                }
                else
                {
                    Debug.LogError("The OnAdClose can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAllAdClose))]
        private static void NativeAd_OnAllAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUNativeAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    interactionListeners.Remove(context);
                    IntPtr nativeAd;
                    if (nativeAds.TryGetValue(context, out nativeAd))
                    {
                        UnionPlatform_NativeAd_Dispose(nativeAd);
                    }
                }
                else
                {
                    Debug.LogError("The OnAllAdClose can not find the context.");
                }
            });
        }

    }
#endif
}
