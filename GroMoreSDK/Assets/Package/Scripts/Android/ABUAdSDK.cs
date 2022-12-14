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

    //#if  UNITY_ANDROID
#if !UNITY_EDITOR && UNITY_ANDROID
    /// <summary>
    /// The android bridge of the union SDK.
    /// </summary>
    public sealed class ABUAdSDK
    {
        private static AndroidJavaObject activity;
        private static AndroidJavaObject mAdManager;
        private static AndroidJavaObject mNativeAd;
        private static AndroidJavaObject mFeedAdManager;
        /// <summary>
        /// Create the advertisement native object.
        /// </summary>
        public static AndroidJavaObject GetAdManager()
        {
            if (mAdManager != null)
            {
                return mAdManager;
            }
            var jc = new AndroidJavaClass(
                "com.bytedance.ad.sdk.mediation.AdManager");
            mAdManager = jc.CallStatic<AndroidJavaObject>("getAdManager");
            return mAdManager;
        }
        
        public static AndroidJavaObject GetFeedAdManager()
        {
            if (mFeedAdManager != null)
            {
                return mFeedAdManager;
            }
            var jc = new AndroidJavaClass(
                "com.bytedance.ad.sdk.mediation.FeedAdManager");
            mFeedAdManager = jc.CallStatic<AndroidJavaObject>("getAdManager", GetActivity());
            return mFeedAdManager;
        }
        
        /// <summary>
        /// Gets the unity main activity.
        /// </summary>
        internal static AndroidJavaObject GetActivity()
        {
            if (activity == null)
            {
                var unityPlayer = new AndroidJavaClass(
                    "com.unity3d.player.UnityPlayer");
                activity = unityPlayer.GetStatic<AndroidJavaObject>(
                    "currentActivity");
            }

            return activity;
        }


        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="userInfoForSegment"></param>
        public static void SetUserInfoForSegment(ABUUserInfoForSegment userInfoForSegment)
        {
            if(userInfoForSegment == null) {
                return;
            }
            var jc = new AndroidJavaClass("com.bytedance.msdk.api.TTMediationAdSdk");
            jc.CallStatic("setUserInfoForSegment", ABUUserInfoForSegment.getCurrentAndroidObject());

        }

        /// <summary>
        /// Sets the publisher did.
        /// </summary>
        /// <param name="did">Did.</param>
        public static void SetPublisherDid(string did) 
        {
            var jc = new AndroidJavaClass("com.bytedance.msdk.api.TTMediationAdSdk");
            jc.CallStatic("setPulisherDid", did);
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        public static void LauchVisualDebugTool()
        {
            var jc = new AndroidJavaClass("com.bytedance.mtesttools.api.TTMediationTestTool");
            jc.CallStatic("launchTestTools", GetActivity(), null);
        }


        /// <summary>
        /// ??????Android imei??????????????????????????????????????????????????????????????????????????????
        /// </summary>
        public static string GetImeiForAndroid()
        {
            return GetAdManager().CallStatic<string>("getImei", GetActivity());
        }

        /// <summary>
        /// ??????Android oaid??????????????????????????????????????????????????????????????????????????????;oaid???imei?????????????????????
        /// </summary>
        public static string GetOaidForAndroid()
        {
            var jc = new AndroidJavaClass("com.bytedance.msdk.api.TTMediationAdSdk");
            return jc.CallStatic<string>("getZbh", GetActivity());
        }

        /// <summary>
        /// ??????????????????(????????????)???????????????????????????
        /// </summary>
        public static void SetThemeStatusIfCan(ABUAdSDKThemeStatus themeStatus)
        {
            int status = 0;
            if(themeStatus == ABUAdSDKThemeStatus.ABUAdSDKThemeStatusNight) {
                status = 1;
            }
            var jc = new AndroidJavaClass("com.bytedance.msdk.api.TTMediationAdSdk");
            jc.CallStatic("setThemeStatus",status);
        }

        /// <summary>
        /// ????????????????????????
        /// </summary>
        public static void SetPrivacyConfig(ABUPrivacyConfig privacyConfig)
        {
            GetAdManager().Call("updatePrivacyConfig", privacyConfig.canUseLocation, privacyConfig.canUsePhoneState, 
    privacyConfig.canUseWifiState, privacyConfig.canUseWriteExternal, privacyConfig.limitPersonalAds,
    privacyConfig.limitProgrammaticAds, privacyConfig.notAdult, privacyConfig.longitude, privacyConfig.latitude);
        }

#pragma warning disable SA1300
#pragma warning disable IDE1006

        private sealed class ExitInstallListener : AndroidJavaProxy
        {
            private readonly Action callback;

            public ExitInstallListener(Action callback)
                : base("com.bytedance.sdk.openadsdk.downloadnew.core.ExitInstallListener")
            {
                this.callback = callback;
            }

            public void onExitInstall()
            {
                UnityDispatcher.PostTask(this.callback);
            }
        }

#pragma warning restore SA1300
#pragma warning restore IDE1006
    }
#endif
}
