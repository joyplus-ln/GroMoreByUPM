
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
    /// The reward video Ad.
    /// </summary>
    public sealed class ABURewardVideoAd : IDisposable
    {
        private static IntPtr rewardVideoAd;
        private bool disposed;

        private static int loadContextID = 0;
        private static Dictionary<int, ABURewardVideoAdCallback> loadListeners = new Dictionary<int, ABURewardVideoAdCallback>();
        private delegate void RewardVideoAd_OnError(int code, string message, int context);
        private delegate void RewardVideoAd_OnRewardVideoAdLoad(IntPtr rewardVideoAd, int context);
        private delegate void RewardVideoAd_OnRewardVideoCached(int context);

        private static int interactionContextID = 0;
        private static Dictionary<int, ABURewardAdInteractionCallback> interactionListeners = new Dictionary<int, ABURewardAdInteractionCallback>();
        private delegate void RewardVideoAd_OnAdShow(int context);
        private delegate void RewardVideoAd_OnAdVideoBarClick(int context);
        private delegate void RewardVideoAd_OnAdClose(int context);
        private delegate void RewardVideoAd_OnVideoComplete(int context);
        private delegate void RewardVideoAd_OnVideoSkip(int context);
        private delegate void RewardVideoAd_OnVideoError(int code, string message, int context);
        private delegate void RewardVideoAd_OnRewardVerify(bool rewardVerify, string rewardInfoJson, int context);
        private delegate void RewardVideoAd_OnWaterfallRitFillFail(string fillFailMessageInfo, int context);
        private delegate void RewardVideoAd_OnAdShowFailed(int code, string message, int context);

        internal ABURewardVideoAd(IntPtr rwdAd)
        {
            rewardVideoAd = rwdAd;
        }

        ~ABURewardVideoAd()
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

        internal static void LoadRewardVideoAd(ABUAdUnit adUnit, ABURewardVideoAdCallback listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            UnionPlatform_RewardVideoAd_Load(
                ABUAdUnit.unitID,
                adUnit.getExpress,
                adUnit.userID,
                adUnit.RewardName,
                adUnit.RewardAmount,
                adUnit.ExtraInfo,
                RewardVideoAd_OnErrorMethod,
                RewardVideoAd_OnRewardVideoAdLoadMethod,
                RewardVideoAd_OnRewardVideoCachedMethod,
                context);
        }

        /// <summary>
        /// Shows the reward video ad.
        /// </summary>
        internal static void ShowRewardVideoAd(ABURewardAdInteractionCallback callback)
        {
            // 设置回调
            var context = interactionContextID++;
            interactionListeners.Add(context, callback);

            UnionPlatform_RewardVideoAd_ShowRewardVideoAd(
                rewardVideoAd,
                null,
                RewardVideoAd_OnAdShowMethod,
                RewardVideoAd_OnAdVideoBarClickMethod,
                RewardVideoAd_OnAdCloseMethod,
                RewardVideoAd_OnVideoCompleteMethod,
                RewardVideoAd_OnVideoSkipMethod,
                RewardVideoAd_OnVideoErrorMethod,
                RewardVideoAd_OnRewardVerifyMethod,
                RewardVideoAd_OnWaterfallRitFillFailMethod,
                RewardVideoAd_OnAdShowFailedMethod,
                context);
        }

        /// <summary>
        /// Show the full reward video.
        /// extro 额外信息，主要用于ritscene
        /// </summary>
        ///                    
        internal static void ShowRewardVideoAdWithRitScene(ABURewardAdInteractionCallback callback, Dictionary<string, object> extro)
        {
            // 设置回调
            var context = interactionContextID++;
            interactionListeners.Add(context, callback);

            // 传到桥接层字典使用json串
            string extroStr = JsonConvert.SerializeObject(extro);
            UnionPlatform_RewardVideoAd_ShowRewardVideoAd(
                rewardVideoAd,
                extroStr,
                RewardVideoAd_OnAdShowMethod,
                RewardVideoAd_OnAdVideoBarClickMethod,
                RewardVideoAd_OnAdCloseMethod,
                RewardVideoAd_OnVideoCompleteMethod,
                RewardVideoAd_OnVideoSkipMethod,
                RewardVideoAd_OnVideoErrorMethod,
                RewardVideoAd_OnRewardVerifyMethod,
                RewardVideoAd_OnWaterfallRitFillFailMethod,
                RewardVideoAd_OnAdShowFailedMethod,
                context);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetInteractionCallback(ABURewardAdInteractionCallback listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);

            UnionPlatform_RewardVideoAd_SetInteractionListener(
                rewardVideoAd,
                RewardVideoAd_OnAdShowMethod,
                RewardVideoAd_OnAdVideoBarClickMethod,
                RewardVideoAd_OnAdCloseMethod,
                RewardVideoAd_OnVideoCompleteMethod,
                RewardVideoAd_OnVideoSkipMethod,
                RewardVideoAd_OnVideoErrorMethod,
                RewardVideoAd_OnRewardVerifyMethod,
                RewardVideoAd_OnWaterfallRitFillFailMethod,
                RewardVideoAd_OnAdShowFailedMethod,
                context);
        }

        /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadCallback(ABUAppDownloadCallback listener)
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
            return UnionPlatform_RewardVideoAd_GetAdRitInfoAdnName(rewardVideoAd);
        }

        // 获取最佳广告的代码位
        public static string GetAdNetworkRitId()
        {
            return UnionPlatform_RewardVideoAd_GetAdNetworkRitId(rewardVideoAd);
        }

        // 获取最佳广告的预设ecpm
        public static string GetPreEcpm()
        {
            return UnionPlatform_RewardVideoAd_GetPreEcpm(rewardVideoAd);
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return UnionPlatform_RewardVideoAd_isReady(rewardVideoAd);
        }

        [DllImport("__Internal")]
        private static extern bool UnionPlatform_RewardVideoAd_isReady(IntPtr fullScreenVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_RewardVideoAd_GetAdRitInfoAdnName(IntPtr rewardVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_RewardVideoAd_GetAdNetworkRitId(IntPtr rewardVideoAd);

        [DllImport("__Internal")]
        private static extern string UnionPlatform_RewardVideoAd_GetPreEcpm(IntPtr rewardVideoAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_RewardVideoAd_Load(
            string CodeId,
            bool getExpress,
            string UserId,
            string RewardName,
            int RewardAmount,
            string ExtraInfo,
            RewardVideoAd_OnError onError,
            RewardVideoAd_OnRewardVideoAdLoad onRewardVideoAdLoad,
            RewardVideoAd_OnRewardVideoCached onRewardVideoCached,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_RewardVideoAd_SetInteractionListener(
            IntPtr rewardVideoAd,
            RewardVideoAd_OnAdShow onAdShow,
            RewardVideoAd_OnAdVideoBarClick onAdVideoBarClick,
            RewardVideoAd_OnAdClose onAdClose,
            RewardVideoAd_OnVideoComplete onVideoComplete,
            RewardVideoAd_OnVideoSkip onVideoSkip,
            RewardVideoAd_OnVideoError onVideoError,
            RewardVideoAd_OnRewardVerify onRewardVerify,
            RewardVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            RewardVideoAd_OnAdShowFailed onAdShowFailed,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_RewardVideoAd_ShowRewardVideoAd(
            IntPtr rewardVideoAd,
            string extro,
            RewardVideoAd_OnAdShow onAdShow,
            RewardVideoAd_OnAdVideoBarClick onAdVideoBarClick,
            RewardVideoAd_OnAdClose onAdClose,
            RewardVideoAd_OnVideoComplete onVideoComplete,
            RewardVideoAd_OnVideoSkip onVideoSkip,
            RewardVideoAd_OnVideoError onVideoError,
            RewardVideoAd_OnRewardVerify onRewardVerify,
            RewardVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
            RewardVideoAd_OnAdShowFailed onAdShowFailed,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_RewardVideoAd_ShowRewardVideoAdWithScene(IntPtr rewardVideoAd,
            ABURitSceneType type,
            string scene);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_RewardVideoAd_Dispose(IntPtr rewardVideoAd);

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnError))]
        private static void RewardVideoAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardVideoAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    loadListeners.Remove(context);
                    listener.OnError(code, message);
                }
                else
                {
                    Debug.LogError("The OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnRewardVideoAdLoad))]
        private static void RewardVideoAd_OnRewardVideoAdLoadMethod(IntPtr rewardVideoAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardVideoAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnRewardVideoAdLoad(new ABURewardVideoAd(rewardVideoAd));
                }
                else
                {
                    Debug.LogError("The OnRewardVideoAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnRewardVideoCached))]
        private static void RewardVideoAd_OnRewardVideoCachedMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardVideoAdCallback listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnRewardVideoAdCached();
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError("The OnRewardVideoCached can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnAdShow))]
        private static void RewardVideoAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError("The OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnAdVideoBarClick))]
        private static void RewardVideoAd_OnAdVideoBarClickMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdVideoBarClick();
                }
                else
                {
                    Debug.LogError("The OnAdVideoBarClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnAdClose))]
        private static void RewardVideoAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClose();
                    interactionListeners.Remove(context);
                    UnionPlatform_RewardVideoAd_Dispose(rewardVideoAd);
                }
                else
                {
                    Debug.LogError("The OnAdClose can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnVideoComplete))]
        private static void RewardVideoAd_OnVideoCompleteMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnVideoComplete();
                }
                else
                {
                    Debug.LogError("The OnVideoComplete can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnVideoSkip))]
        private static void RewardVideoAd_OnVideoSkipMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnSkippedVideo();
                }
                else
                {
                    Debug.LogError("The onVideoSkip can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnVideoError))]
        private static void RewardVideoAd_OnVideoErrorMethod(int errCode, string errMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnVideoError(errCode, errMsg);
                }
                else
                {
                    Debug.LogError("The OnVideoError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnRewardVerify))]
        private static void RewardVideoAd_OnRewardVerifyMethod(bool rewardVerify, string rewardInfoJson, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    // json反序列化
                    ABUAdapterRewardAdInfo rewardAdInfo = JsonConvert.DeserializeObject<ABUAdapterRewardAdInfo>(rewardInfoJson);
                    // 存量用户
                    listener.OnRewardVerify(rewardVerify);
                    // 3510新增
                    listener.OnRewardVerify(rewardVerify, rewardAdInfo);
                }
                else
                {
                    Debug.LogError("The OnRewardVerify can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnWaterfallRitFillFail))]
        private static void RewardVideoAd_OnWaterfallRitFillFailMethod(string fillFailMessageInfo, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnWaterfallRitFillFail(fillFailMessageInfo);
                }
                else
                {
                    Debug.LogError("The OnWaterfallRitFillFail can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(RewardVideoAd_OnAdShowFailed))]
        private static void RewardVideoAd_OnAdShowFailedMethod(int errCode, string errMsg, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ABURewardAdInteractionCallback listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShowFailed(errCode, errMsg);
                }
                else
                {
                    Debug.LogError("The OnAdShowFailed can not find the context.");
                }
            });
        }

    }
#endif
}
