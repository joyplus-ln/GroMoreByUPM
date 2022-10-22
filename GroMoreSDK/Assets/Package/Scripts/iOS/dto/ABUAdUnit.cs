//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_IOS
    /// <summary>
    /// The slot of a advertisement.
    /// </summary>
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public sealed class ABUAdUnit
    {
        public static string unitID;

        public string userID;
        public float width;
        public float height;
        public bool isSupportDeepLink;
        public int adCount;
        public string RewardName;
        public int RewardAmount;
        public string ExtraInfo;
        public bool getExpress; 
        public bool useExpress2IfCanForGDT; 
        public bool muted;
        public ABUSplashButtonType splashButtonType;
        public int AdStyleType;

        [Obsolete("接口废弃 - [轮播功能(autoRefreshTime)端上已不支持控制，需要在GroMore平台“瀑布流属性配置”模块配置]", false)]
        public int autoRefreshTime;

        /// <summary>
        /// The builder used to build an Ad slot.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Sets the code ID.
            /// </summary>
            public Builder SetCodeId(string codeId)
            {
                unitID = codeId;
                return this;
            }

            /// <summary>
            /// Sets the time the ad refresh auto. If 0 , the ad not auto refresh.
            /// </summary>
            [Obsolete("接口废弃 - [轮播功能(autoRefreshTime)端上已不支持控制，需要在GroMore平台“瀑布流属性配置”模块配置]", false)]
            public Builder SetAutoRefreshTime(int autoRefreshTime)
            {
                ABUAdUnit.CreateSharedAdSlot().autoRefreshTime = autoRefreshTime;
                return this;
            }

            /// <summary>
            /// Sets the user ID.
            /// </summary>
            public Builder SetUserID(string userID)
            {
                ABUAdUnit.CreateSharedAdSlot().userID = userID;
                return this;
            }

            /// <summary>
            /// Sets the image accepted size.
            /// </summary>
            public Builder SetImageAcceptedSize(int width, int height)
            {
                ABUAdUnit.CreateSharedAdSlot().width = width;
                ABUAdUnit.CreateSharedAdSlot().height = height;
                return this;
            }

            /// <summary>
            /// Sets a value : true for get a express Ad, or get normal native ad
            /// </summary>
            public Builder SetExpressIfCan(bool getExpress)
            {
                ABUAdUnit.CreateSharedAdSlot().getExpress = getExpress;
                return this;
            }

            /// <summary>
            /// Sets a value : true for get a GDT express2.0 Ad, or get normal express1.0 ad
            /// </summary>
            public Builder SetUseExpress2IfCanForGDT(bool useExpress2IfCanForGDT)
            {
                ABUAdUnit.CreateSharedAdSlot().useExpress2IfCanForGDT = useExpress2IfCanForGDT;
                return this;
            }

            /// <summary>
            /// Set the loading mode of ads
            /// </summary>
            /// <param name="adStyleType"></param>
            /// <returns></returns>
            public Builder SetAdStyleType(int adStyleType)
            {
                return this;
            }

            /// <summary>
            /// Sets a value : true for get a express Ad, or get normal native ad
            /// </summary>
            public Builder SetMutedIfCan(bool muted)
            {
                ABUAdUnit.CreateSharedAdSlot().muted = muted;
                return this;
            }


            /// <summary>
            /// Sets the support deep link.
            /// </summary>
            public Builder SetSupportDeepLink(bool support)
            {
                ABUAdUnit.CreateSharedAdSlot().isSupportDeepLink = support;
                return this;
            }

            /// <summary>
            /// Sets the Ad count.
            /// </summary>
            public Builder SetAdCount(int count)
            {
                ABUAdUnit.CreateSharedAdSlot().adCount = count;
                return this;
            }

            /// <summary>
            /// Sets the reward name.
            /// </summary>
            public Builder SetRewardName(string name)
            {
                ABUAdUnit.CreateSharedAdSlot().RewardName = name;
                return this;
            }

            /// <summary>
            /// Sets the reward amount.
            /// </summary>
            public Builder SetRewardAmount(int amount)
            {
                ABUAdUnit.CreateSharedAdSlot().RewardAmount = amount;
                return this;
            }

            /// <summary>
            /// set the express width and height 
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public Builder SetExpressViewAcceptedSize(float width, float height)
            {
                ABUAdUnit.CreateSharedAdSlot().width = width;
                ABUAdUnit.CreateSharedAdSlot().height = height;
                return this;
            }

            /// <summary>
            /// Sets the extra media for Ad.
            /// </summary>
            public Builder SetMediaExtra(string extra)
            {
                ABUAdUnit.CreateSharedAdSlot().ExtraInfo = extra;
                return this;
            }

            /// <summary>
            /// Sets the splashButtonType.
            /// </summary>
            /// <param name="splashButtonType">splashButtonType.</param>
            public Builder setSplashButtonType(ABUSplashButtonType splashButtonType)
            {
                ABUAdUnit.CreateSharedAdSlot().splashButtonType = splashButtonType;
                return this;
            }

            /// <summary>
            /// Special configuration items for Admob Native
            /// </summary>
            /// <param name="admobNativeAdOptions"></param>
            /// <returns></returns>
            public Builder SetAdmobNativeAdOptions(AndroidJavaObject admobNativeAdOptions)
            {
                return this;
            }

            /// <summary>
            /// Video and sound related configuration
            /// </summary>
            /// <param name="videoOption"></param>
            /// <returns></returns>
            public Builder SetTTVideoOption(AndroidJavaObject videoOption)
            {
                return this;
            }

            /// <summary>
            /// Sets the custom data.
            /// </summary>
            /// <returns>The custom data.</returns>
            /// <param name="customData">Custom data.</param>
            public Builder setCustomData(Dictionary<string, string> customData)
            {
                return this;
            }

            /// <summary>
            /// Sets the type of the download.
            /// </summary>
            /// <returns>The download type.</returns>
            /// <param name="downloadType">Download type.</param>
            public Builder setDownloadType(ABUDownloadType downloadType)
            {
                return this;
            }

            /// <summary>
            /// Build the Ad slot.
            /// </summary>
            public ABUAdUnit Build()
            {
                return ABUAdUnit.CreateSharedAdSlot();
            }
        }

        // AndroidJavaObject返回类型的方式为Android专用
        public AndroidJavaObject getCurrentAndroidObject()
        {
            return null;
        }

        public static AndroidJavaObject getFullVideoOrFeedTTVideoOption(bool useExpress2IfCanForGDT, bool muted)
        {
            return null;
        }

        public static AndroidJavaObject getRewardVideoTTVideoOption(bool muted)
        {
            return null;
        }

        public static AndroidJavaObject getAdmobNativeAdOptions()
        {
            return null;
        }

        private static ABUAdUnit _adSlot = null;

        private static ABUAdUnit CreateSharedAdSlot()
        {
            if (_adSlot == null)
            {
                _adSlot = new ABUAdUnit();
            }
            return _adSlot;
        }
    }
#endif
}
