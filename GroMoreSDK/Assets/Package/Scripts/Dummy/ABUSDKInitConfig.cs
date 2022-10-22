
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    /// <summary>
    /// The config of SDK init.
    /// </summary>
    using UnityEngine;
    public sealed class ABUSDKInitConfig
    {
        public string appID;
        public bool openDebugLog;

        public string appName;//only for android
        public bool openAdnTest;//only for android
        public bool isPanglePaid;//only for android
        public bool usePangleTextureView;//only for android
        public int pangleTitleBarTheme;//only for android
        public bool allowPangleShowNotify;//only for android
        public bool allowPangleShowPageWhenScreenLock;//only for android
        public int pangleDirectDownloadNetworkType;//only for android
        public bool needPangleClearTaskReset;//only for android

        public AndroidJavaObject getCurrentAndroidObject()
        {
            return null;
        }

        /// <summary>
        /// The builder used to build an Ad slot.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Sets the appID.
            /// </summary>
            public Builder SetAppID(string appID)
            {
                return this;
            }

            /// <summary>
            /// Sets the appName.
            /// </summary>
            public Builder SetAppName(string appName)
            {
                return this;
            }

            /// <summary>
            /// Sets the openAdnTest.
            /// </summary>
            public Builder SetOpenAdnTest(bool openAdnTest)
            {
                return this;
            }

            /// <summary>
            /// Sets the isPanglePaid
            /// </summary>
            public Builder SetIsPanglePaid(bool isPanglePaid)
            {
                return this;
            }

            /// <summary>
            /// Sets a value : openDebugLog
            /// </summary>
            public Builder SetOpenDebugLog(bool openDebugLog)
            {
                return this;
            }

            /// <summary>
            /// Sets a value : pangleTitleBarTheme
            /// </summary>
            public Builder SetPangleTitleBarTheme(int pangleTitleBarTheme)
            {
                return this;
            }

            /// <summary>
            /// Set the loading mode of ads
            /// </summary>
            /// <param name="adStyleType"></param>
            /// <returns></returns>
            public Builder SetAllowPangleShowNotify(bool allowPangleShowNotify)
            {
                return this;
            }

            /// <summary>
            /// Sets a value : allowPangleShowPageWhenScreenLock
            /// </summary>
            public Builder SetAllowPangleShowPageWhenScreenLock(bool allowPangleShowPageWhenScreenLock)
            {
                return this;
            }

            /// <summary>
            /// Sets the pangleDirectDownloadNetworkType.
            /// </summary>
            public Builder SetPangleDirectDownloadNetworkType(int pangleDirectDownloadNetworkType)
            {
                return this;
            }

            /// <summary>
            /// Sets the needPangleClearTaskReset
            /// </summary>
            public Builder SetNeedPangleClearTaskReset(bool needPangleClearTaskReset)
            {
                return this;
            }

            /// <summary>
            /// Sets the usePangleTextureView
            /// </summary>
            public Builder SetUsePangleTextureView(bool usePangleTextureView)
            {
                return this;
            }

            /// <summary>
            /// Build the ABUSDKInitConfig.
            /// </summary>
            public ABUSDKInitConfig Build()
            {
                return new ABUSDKInitConfig();
            }
        }
    }
#endif
}
