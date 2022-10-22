//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using UnityEngine;

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    using System;
    /// <summary>
    /// The splash Ad.
    /// </summary>
    public class ABUSplashAd
    {
        public AndroidJavaObject ad;
        public ABUAdUnit adUnit;

        internal ABUSplashAd(AndroidJavaObject ad)
        {

        }

        public AndroidJavaObject getCurrentSplshAd()
        {
            return null;
        }

        public ABUAdUnit getAdUnit()
        {
            return null;
        }

        public int GetInteractionType()
        {
            return 0;
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

        }

        /// <summary>
        /// 加载广告
        /// Loads the splash ad.
        /// </summary>
        // <param name="adUnit">Reward video ad dto.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="timeOut">timeOut.</param> 
        internal static void LoadSplashAd(ABUAdUnit adUnit, ABUSplashAdListener callback, int timeOut)
        {

        }

        /// <summary>
        /// 展示广告
        /// Show the splash video ad.
        internal static void ShowSplashAd(ABUSplashAdInteractionListener splashAdInteractionListener)
        {

        }

        /// <summary>
        /// Sets the listener for the Ad download.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
        {
        }

        public void Dispose()
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
