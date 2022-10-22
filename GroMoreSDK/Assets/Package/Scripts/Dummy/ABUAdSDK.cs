
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    using System;
    using UnityEngine;
    using Newtonsoft.Json;

#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    //#if !UNITY_EDITOR && UNITY_ANDROID
    /// <summary>
    /// The android bridge of the union SDK.
    /// </summary>
    public sealed class ABUAdSDK
    {
        private static AndroidJavaObject activity;
        private static AndroidJavaObject mAdManager;
        private static AndroidJavaObject mNativeAd;
        /// <summary>
        /// Create the advertisement native object.
        /// </summary>
        public static AndroidJavaObject GetAdManager()
        {
            return null;
        }

        /// <summary>
        /// Gets the unity main activity.
        /// </summary>
        internal static AndroidJavaObject GetActivity()
        {
            return null;
        }

        public static AndroidJavaObject GetFeedAdManager()
        {
            return null;
        }


        /// <summary>
        /// 设置流量分组信息
        /// </summary>
        /// <param name="userInfoForSegment"></param>
        public static void SetUserInfoForSegment(ABUUserInfoForSegment userInfoForSegment)
        {
        }

        /// <summary>
        /// Sets the publisher did.
        /// </summary>
        /// <param name="did">Did.</param>
        public static void SetPublisherDid(string did)
        {

        }

        /// <summary>
        /// 启动可视化测试工具
        /// </summary>
        public static void LauchVisualDebugTool()
        {

        }

        /// <summary>
        /// 获取Android imei用于在平台注册测试设备，测试设备可启动可视化测试工具
        /// </summary>
        public static string GetImeiForAndroid()
        {
            return null;
        }

        /// <summary>
        /// 获取Android oaid用于在平台注册测试设备，测试设备可启动可视化测试工具;oaid和imei只需要一个即可
        /// </summary>
        public static string GetOaidForAndroid()
        {
            return null;
        }

        /// <summary>
        /// 设置主题模式(如果可以)，目前仅支持穿山甲
        /// </summary>
        public static void SetThemeStatusIfCan(ABUAdSDKThemeStatus themeStatus)
        {

        }

        /// <summary>
        /// 设置相关隐私配置
        /// </summary>
        public static void SetPrivacyConfig(ABUPrivacyConfig privacyConfig)
        {

        }
    }
#endif
}
