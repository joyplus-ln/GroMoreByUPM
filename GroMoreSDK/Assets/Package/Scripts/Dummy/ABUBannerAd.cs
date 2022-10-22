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
    /// <summary>
    /// The banner Ad.
    /// </summary>
    public sealed class ABUBannerAd
    {
        public float getBannerViewWidth()
        {
            return 0;
        }

        public float getBannerViewHeight()
        {
            return 0;
        }

        /// <summary>
        /// 异步加载广告
        /// Loads the reward video ad.
        /// </summary>
        /// <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadBannerAd(
            ABUAdUnit adUnit, ABUBannerAdCallback listener, ABUBannerAdInteractionCallback callback)
        {

        }

        /// <summary>
        /// show the   Ad
        /// <param name="x">the origin x of th ad</param>
        /// <param name="y">the origin y of th ad</param>
        /// </summary>
        public void ShowBannerAd(float x, float y)
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

    }
#endif
}
