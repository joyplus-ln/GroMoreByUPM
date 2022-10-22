
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_ANDROID
    /// <summary>
    /// The slot of a advertisement.
    /// </summary>
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public sealed class ABUAdUnit
    {
        public string unitID;

        public string userID;
        public float width;
        public float height;
        public bool isSupportDeepLink;
        public int adCount;
        public string RewardName;
        public int RewardAmount;
        public string ExtraInfo;
        public bool getExpress; // 是否模板广告
        public bool useExpress2IfCanForGDT; // GDT是否模板2.0；当服务端有相关配置时，以服务端优先！！！
        public bool muted;  // 静音选项
        public ABUSplashButtonType splashButtonType;
        public int AdStyleType;

        [Obsolete("接口废弃 - [轮播功能(autoRefreshTime)端上已不支持控制，需要在GroMore平台“瀑布流属性配置”模块配置]", false)]
        public int autoRefreshTime;

        /// <summary>
        /// The builder used to build an Ad slot.
        /// </summary>
        public sealed class Builder
        {
            private AndroidJavaObject builder;
            private string codeId;
            private int refreshTime;
            private string unitID;
            
            public Builder()
            {
                this.builder = new AndroidJavaObject("com.bytedance.msdk.api.AdSlot$Builder");
            }

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
                refreshTime = autoRefreshTime;
                return this;
            }

            /// <summary>
            /// Sets the user ID.
            /// </summary>
            public Builder SetUserID(string userID)
            {
                this.builder.Call<AndroidJavaObject>("setUserID", userID);
                return this;
            }

            /// <summary>
            /// Sets the image accepted size.
            /// </summary>
            public Builder SetImageAcceptedSize(int width, int height)
            {
                this.builder.Call<AndroidJavaObject>(
                    "setBannerSize", 6);
                this.builder.Call<AndroidJavaObject>(
                    "setImageAdSize", width, height);
                return this;
            }

            /// <summary>
            /// Sets a value : true for get a express Ad, or get normal native ad
            /// </summary>
            public Builder SetExpressIfCan(bool getExpress)
            {
                if (getExpress)
                {
                    this.builder.Call<AndroidJavaObject>(
                        "setAdStyleType", 1);
                }
                else
                {
                    this.builder.Call<AndroidJavaObject>(
                        "setAdStyleType", 2);
                }
                return this;
            }

            /// <summary>
            /// Sets a value : true for get a GDT express2.0 Ad, or get normal express1.0 ad
            /// </summary>
            public Builder SetUseExpress2IfCanForGDT(bool useExpress2IfCanForGDT)
            {
                return this;
            }

            /// <summary>
            /// Set the loading mode of ads
            /// </summary>
            /// <param name="adStyleType"></param>
            /// <returns></returns>
            public Builder SetAdStyleType(int adStyleType)
            {
                this.builder.Call<AndroidJavaObject>("setAdStyleType", adStyleType);
                return this;
            }

            /// <summary>
            /// Sets a value : true for get a express Ad, or get normal native ad
            /// </summary>
            public Builder SetMutedIfCan(bool muted)
            {
                // ABUAdUnit.CreateSharedAdSlot().muted = muted;
                return this;
            }

            /// <summary>
            /// Sets the support deep link.
            /// </summary>
            public Builder SetSupportDeepLink(bool support)
            {
                this.builder.Call<AndroidJavaObject>("setSupportDeepLink", support);
                return this;
            }

            /// <summary>
            /// Sets the Ad count.
            /// </summary>
            public Builder SetAdCount(int count)
            {
                this.builder.Call<AndroidJavaObject>("setAdCount", count);
                return this;
            }

            /// <summary>
            /// Sets the reward name.
            /// </summary>
            public Builder SetRewardName(string name)
            {
                this.builder.Call<AndroidJavaObject>("setRewardName", name);
                return this;
            }

            /// <summary>
            /// Sets the reward amount.
            /// </summary>
            public Builder SetRewardAmount(int amount)
            {
                this.builder.Call<AndroidJavaObject>("setRewardAmount", amount);
                return this;
            }

            /// <summary>
            /// set the express width and height 
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public Builder SetExpressViewAcceptedSize(int width, int height)
            {
                this.builder.Call<AndroidJavaObject>(
                    "setImageAdSize", width, height);
                return this;
            }

            /// <summary>
            /// Sets the extra media for Ad.
            /// </summary>
            public Builder SetMediaExtra(string extra)
            {
                this.builder.Call<AndroidJavaObject>("setMediaExtra", extra);
                return this;
            }

            /// <summary>
            /// Sets the splashButtonType.
            /// </summary>
            /// <param name="splashButtonType">splashButtonType.</param>
            public Builder setSplashButtonType(ABUSplashButtonType splashButtonType)
            {
                int type = 1;
                if (splashButtonType == ABUSplashButtonType.ABUSplashButtonTypeDownloadBar)
                {
                    type = 2;
                }
                else if (splashButtonType == ABUSplashButtonType.ABUSplashButtonTypeFullScreen)
                {
                    type = 1;
                }
                this.builder.Call<AndroidJavaObject>("setSplashButtonType", type);
                return this;
            }

            /// <summary>
            /// Special configuration items for Admob Native
            /// </summary>
            /// <param name="admobNativeAdOptions"></param>
            /// <returns></returns>
            public Builder SetAdmobNativeAdOptions(AndroidJavaObject admobNativeAdOptions)
            {
                this.builder.Call<AndroidJavaObject>("setAdmobNativeAdOptions", admobNativeAdOptions);
                return this;
            }

            /// <summary>
            /// Video and sound related configuration
            /// </summary>
            /// <param name="videoOption"></param>
            /// <returns></returns>
            public Builder SetTTVideoOption(AndroidJavaObject videoOption)
            {
                this.builder.Call<AndroidJavaObject>("setTTVideoOption", videoOption);
                return this;
            }

            /// <summary>
            /// Sets the custom data.
            /// </summary>
            /// <returns>The custom data.</returns>
            /// <param name="customData">Custom data.</param>
            public Builder setCustomData(Dictionary<string, string>  customData) 

            {
                if (customData == null) {
                    return this;
                }
                AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
                foreach(KeyValuePair<string, string> pair in customData){
                    map.Call<string>("put", pair.Key, pair.Value);
                }
                this.builder.Call<AndroidJavaObject>("setCustomData", map);
                return this;
            }

            /// <summary>
            /// Sets the type of the download.
            /// </summary>
            /// <returns>The download type.</returns>
            /// <param name="downloadType">Download type.</param>
            public Builder setDownloadType(ABUDownloadType downloadType)
            {
                int type = 0;
                if (downloadType == ABUDownloadType.DOWNLOAD_TYPE_NO_POPUP)
                {
                    type = 0;
                }
                else if (downloadType == ABUDownloadType.DOWNLOAD_TYPE_POPUP)
                {
                    type = 1;
                }
                this.builder.Call<AndroidJavaObject>("setDownloadType", type);
                return this;
            }

            /// <summary>
            /// Build the Ad slot.
            /// </summary>
            public ABUAdUnit Build()
            {
                var native = builder.Call<AndroidJavaObject>("build");
                ABUAdUnit aBUAdUnit = new ABUAdUnit(native);
                aBUAdUnit.unitID = unitID;
                aBUAdUnit.setAutoRefreshTime(refreshTime);
                return aBUAdUnit;
            }
        }

        // AndroidJavaObject返回类型的方式为Android专用
        public AndroidJavaObject getCurrentAndroidObject()
        {
            return new AndroidJavaObject("com.bytedance.msdk.api.AdSlot");
        }

        public static AndroidJavaObject getFullVideoOrFeedTTVideoOption(bool useExpress2IfCanForGDT, bool muted)
        {
            AndroidJavaObject ttVideoOption = ABUAdSDK.GetAdManager().Call<AndroidJavaObject>("getTTVideoOption", useExpress2IfCanForGDT, muted);
            return ttVideoOption;
        }

        public static AndroidJavaObject getRewardVideoTTVideoOption(bool muted)
        {
            AndroidJavaObject ttVideoOption = ABUAdSDK.GetAdManager().Call<AndroidJavaObject>("getRewardTTVideoOption", muted);
            return ttVideoOption;
        }

        public static AndroidJavaObject getAdmobNativeAdOptions()
        {
            AndroidJavaObject admobNativeAdOptions = ABUAdSDK.GetAdManager().Call<AndroidJavaObject>("getAdmobNativeAdOptions");
            return admobNativeAdOptions;
        }

        internal ABUAdUnit(AndroidJavaObject slot)
        {
            _adSlot = slot;
        }

        public void setAutoRefreshTime(int freshTime)
        {
            if (freshTime > 0)
            {
                autoRefreshTime = freshTime;
            }
        }

        public int getRefreshTime()
        {
            return autoRefreshTime;
        }

        internal AndroidJavaObject Handle
        {
            get { return this._adSlot; }
        }

        private AndroidJavaObject _adSlot = null;
    }
#endif
}
