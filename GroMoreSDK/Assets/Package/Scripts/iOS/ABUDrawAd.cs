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
    /// The draw Ad.
    /// </summary>
    public class ABUDrawAd
    {
        private static int loadContextID = 0;
        private static Dictionary<int, ABUDrawAdCallback> loadListeners = new Dictionary<int, ABUDrawAdCallback>();
        private delegate void DrawAd_OnError(int code, string message, int context);
        private delegate void DrawAd_OnDrawAdLoad(IntPtr drawAd, int adCount, int context);
        private delegate void DrawAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);

        private static int interactionContextID = 0;
        private static Dictionary<int, ABUDrawAdInteractionCallback> interactionListeners = new Dictionary<int, ABUDrawAdInteractionCallback>();
        private static Dictionary<int, IntPtr> drawAds = new Dictionary<int, IntPtr>();
        private delegate void DrawAd_OnAdShow(int index, int context);
        private delegate void DrawAd_OnAdClicked(int index, int context);
        private delegate void DrawAd_OnAdClose(int index, int context);
        private delegate void DrawAd_OnAllAdClose(int context);
        private delegate void DrawAd_OnRenderFail(string msg, int code, int context);
        private delegate void DrawAd_OnRenderSuccess(float width, float height, int context);

        private IntPtr drawAd;
        private List<int> adInternalIndexs = new List<int>();
        private bool disposed;

        internal ABUDrawAd(IntPtr ad)
        {
            drawAd = ad;
        }

        ~ABUDrawAd()
        {
            this.Dispose(false);
        }

        public List<int> getAdIndexs()
        {
            return this.adInternalIndexs;
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
        /// iOS异步加载广告
        /// Loads the draw ad.
        /// </summary>
        /// <param name="adUnit">draw ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadDrawAd(ABUAdUnit adUnit, ABUDrawAdCallback listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            UnionPlatform_Ad_Load(
                DrawAd_OnErrorMethod,
                DrawAd_OnDrawAdLoadMethod,
                DrawAd_OnWaterfallRitFillFailMethod,
                context,
                ABUAdUnit.unitID,
                adUnit.adCount,
                adUnit.width,
                adUnit.height);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetAdInteractionListener(ABUDrawAdInteractionCallback listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);
            UnionPlatform_DrawAd_SetInteractionListener(
                drawAd,
                DrawAd_OnAdShowMethod,
                DrawAd_OnAdClickedMethod,
                DrawAd_OnAdCloseMethod,
                DrawAd_OnAllAdCloseMethod,
                DrawAd_OnRenderFailMethod,
                DrawAd_OnRenderSuccessMethod,
                context);
        }

        /// <summary>
        /// show the express Ad
        /// <param name="type">the type of ad</param>
        /// <param name="x">the origin x of th ad</param>
        /// <param name="y">the origin y of th ad</param>
        /// </summary>
        public void ShowDrawAd(int index, float x, float y)
        {
            UnionPlatform_DrawAd_ShowNativeAd(drawAd, index, x, y);
        }

        public void CloseDrawAd(int index)
        {
            UnionPlatform_DrawAd_CallClose(drawAd, index);
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public string GetAdRitInfoAdnName(int index)
        {
            return UnionPlatform_DrawAd_GetAdRitInfoAdnName(drawAd, index);
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public string GetAdNetworkRitId(int index)
        {
            return UnionPlatform_DrawAd_GetAdNetworkRitId(drawAd, index);
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public string GetPreEcpm(int index)
        {
            return UnionPlatform_DrawAd_GetPreEcpm(drawAd, index);
        }

        [DllImport("__Internal")]
        private static extern void UnionPlatform_Ad_Load(
            DrawAd_OnError onError,
            DrawAd_OnDrawAdLoad onDrawAdLoad,
            DrawAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            int context,
            string slotID,
            int adCount,
            float width,
            float height);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_DrawAd_ShowNativeAd(
            IntPtr drawAd,
            int index,
            float x,
            float y);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_DrawAd_CallClose(
            IntPtr drawAd,
            int index);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_DrawAd_SetInteractionListener(
            IntPtr drawAd,
            DrawAd_OnAdShow onAdShow,
            DrawAd_OnAdClicked onAdClicked,
            DrawAd_OnAdClose onAdClose,
            DrawAd_OnAllAdClose onAllAdClose,
            DrawAd_OnRenderFail onRenderFail,
            DrawAd_OnRenderSuccess onRenderSuccess,
            int context);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_DrawAd_GetAdRitInfoAdnName(
            IntPtr drawAd,
            int index);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_DrawAd_GetAdNetworkRitId(
            IntPtr drawAd,
            int index);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_DrawAd_GetPreEcpm(
            IntPtr drawAd,
            int index);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_DrawAd_Dispose(
            IntPtr drawAd);

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnDrawAdLoad))]
        private static void DrawAd_OnDrawAdLoadMethod(IntPtr drawAd, int adCount, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    var ad = new ABUDrawAd(drawAd);
                    for (int i = 0; i < adCount; i++)
                    {
                        ad.adInternalIndexs.Add(i);
                    }
                    drawAds.Add(context, drawAd);
                    listener.OnDrawAdLoad(ad);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnDrawAdLoadMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnError))]
        private static void DrawAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnErrorMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnWaterfallRitFillFail))]
        private static void DrawAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnWaterfallRitFillFail(fillFailMessageInfo);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnWaterfallRitFillFailMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnAdShow))]
        private static void DrawAd_OnAdShowMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow(index);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnAdShowMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnAdClicked))]
        private static void DrawAd_OnAdClickedMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked(index);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnAdClickedMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnAdClose))]
        private static void DrawAd_OnAdCloseMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdDismiss(index);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnAdCloseMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnAllAdClose))]
        private static void DrawAd_OnAllAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    interactionListeners.Remove(context);
                    IntPtr drawAd;
                    if (drawAds.TryGetValue(context, out drawAd))
                    {
                        UnionPlatform_DrawAd_Dispose(drawAd);
                    }
                }
                else
                {
                    Debug.LogError("The DrawAd_OnAllAdCloseMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnRenderFail))]
        private static void DrawAd_OnRenderFailMethod(string msg, int code, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnRenderFail(msg, code);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnRenderFailMethod can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(DrawAd_OnRenderSuccess))]
        private static void DrawAd_OnRenderSuccessMethod(float width, float height, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABUDrawAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnRenderSuccess(width, height);
                }
                else
                {
                    Debug.LogError("The DrawAd_OnRenderSuccessMethod can not find the context.");
                }
            });
        }

    }
#endif
}
