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
    using Newtonsoft.Json;
    using UnityEngine;

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
        /// 设置流量分组信息
        /// </summary>
        /// <param name="userInfoForSegment"></param>
        public static void SetUserInfoForSegment(ABUUserInfoForSegment userInfoForSegment)
        {
            // 传到桥接层字典需要使用json串
            string customInfoStr = JsonConvert.SerializeObject(userInfoForSegment.customDataDictionary);
            UnionPlatform_setUserInfoForSegment(
                userInfoForSegment.age,
                (int)(userInfoForSegment.gender),
                userInfoForSegment.userID,
                userInfoForSegment.channel,
                userInfoForSegment.subChannel,
                userInfoForSegment.userGroup,
                customInfoStr);
        }

        /// <summary>
        /// 启动可视化测试工具
        /// </summary>
        public static void LauchVisualDebugTool()
        {
            UnionPlatform_LauchVisualDebugTool();
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
            UnionPlatform_SetThemeStatusIfCan((int)themeStatus);
        }

        /// <summary>
        /// 设置相关隐私配置
        /// </summary>
        public static void SetPrivacyConfig(ABUPrivacyConfig privacyConfig)
        {
            // ABUPrivacyConfig有几个iOS相关的调用几次
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyForbiddenCAID, Convert.ToDouble(privacyConfig.limitCAID));
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyLimitPersonalAds, Convert.ToDouble(privacyConfig.limitPersonalAds));
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyLimitProgrammaticAds, Convert.ToDouble(privacyConfig.limitProgrammaticAds));
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyCanLocation, Convert.ToDouble(privacyConfig.canUseLocation));
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyLongitude, Convert.ToDouble(privacyConfig.longitude));
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyLatitude, Convert.ToDouble(privacyConfig.latitude));
            UnionPlatform_SetPrivacyConfigForKeyValue(ABUConstantHelper.ABUPrivacyNotAdult, Convert.ToDouble(privacyConfig.notAdult));
        }

        /// <summary>
        /// Sets the publisher did.
        /// </summary>
        /// <param name="did">Did.</param>
        public static void SetPublisherDid(string did)
        {
            if (did.Length <= 0)
            {
                return;
            }

            Dictionary<string, string> publisherDidMap = new Dictionary<string, string>();
            publisherDidMap.Add("device_id", did);

            string publisherDidStr = JsonConvert.SerializeObject(publisherDidMap);
            UnionPlatform_SetPublisherDid(publisherDidStr);
        }

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SetPublisherDid(string publisherDidStr);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SetPrivacyConfigForKeyValue(
            string privacyKey,
            double privacyIntValue
            );

        [DllImport("__Internal")]
        private static extern void UnionPlatform_setUserInfoForSegment(
            int age,
            int gender,
            string userID,
            string channel,
            string subCagehannel,
            string userGroup,
            string customInfos
            );

        [DllImport("__Internal")]
        private static extern void UnionPlatform_LauchVisualDebugTool();

        [DllImport("__Internal")]
        private static extern void UnionPlatform_SetThemeStatusIfCan(int themeStatus);

    }

#endif

}
