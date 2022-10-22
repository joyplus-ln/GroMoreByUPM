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
    /// Set the InterstitialFull Ad.
    /// </summary>
    public sealed class ABUInterstitialFullAd
    {

        /// <summary>
        /// Loads the InterstitialFull video ad.
        /// </summary>
        /// <param name="adUnit">ad dto.</param>
        /// <param name="callback">callback.</param>
        internal static void LoadInterstitialFullAd(ABUAdUnit adUnit, ABUInterstitialFullAdCallback callback)
        {
        }

        /// <summary>
        /// Show this InterstitialFull Ad.
        /// </summary>
        internal static void ShowInteractionAd(ABUInterstitialFullAdInteractionCallback callback)
        {
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