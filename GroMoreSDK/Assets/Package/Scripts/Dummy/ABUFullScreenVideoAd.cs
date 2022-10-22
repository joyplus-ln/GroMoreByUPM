//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// The full screen video Ad.
    /// </summary>
    public sealed class ABUFullScreenVideoAd
    {

        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDance.Union.LGRewardVideoAd"/> class.
        /// </summary>
        /// <param name="rewardVideoAdObj">Reward video ad object.</param>
        internal ABUFullScreenVideoAd(AndroidJavaObject fullScreenVideoAdObj)
        {
        }

        /// <summary>
        /// 异步视频广告
        /// Loads the reward video ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadFullScreenVideoAd(ABUAdUnit adUnit, ABUFullScreenVideoAdCallback listener)
        {
        }

        /// <summary>
        /// Show the full screen video.
        /// </summary>
        internal static void ShowFullVideoAd(ABUFullScreenVideoAdInteractionCallback callback)
        {
        }

        /// <summary>
        /// Show the full screen video.
        /// extro 额外信息，主要用于ritscene
        /// </summary>
        internal static void ShowFullVideoAdWithRitScene(ABUFullScreenVideoAdInteractionCallback callback, Dictionary<string, object> extro)
        {
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

        // 获取最佳广告的adn类型 （该接口需要在show回调之后会返回对应的adn），当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3
        [Obsolete("接口废弃 - 请使用 GetAdRitInfoAdnName", true)]
        public static ABUAdnType GetAdNetworkPlaformId()
        {
            return ABUAdnType.ABUAdnNoPermission;
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public static string GetAdRitInfoAdnName()
        {
            return null;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public static string GetAdNetworkRitId()
        {
            return null;
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public static string GetPreEcpm()
        {
            return null;
        }

        // 广告是否准备好；建议在show之前调用判断
        public static bool isReady()
        {
            return true;
        }

    }

#endif
}
