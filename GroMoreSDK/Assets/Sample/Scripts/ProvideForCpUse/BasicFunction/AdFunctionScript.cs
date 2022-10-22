using ByteDance.Union.Constant;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ByteDance.Union
{
    // 广告模块
    public class AdFunctionScript : Singeton<AdFunctionScript>, IFunctionDispatcher
    {
        // 开屏广告样例
        private SplashAdExample splashAdExample = new SplashAdExample();
        // 横幅广告样例
        private BannerAdExample bannerAdExample = new BannerAdExample();
        // 信息流广告样例
        private NativeAdExample nativeAdExample = new NativeAdExample();
        // 激励视频广告样例
        private RewardVideoAdExample rewardVideoAdExample = new RewardVideoAdExample();
        // 全屏视频广告样例
        private FullscreenAdExample fullscreenAdExample = new FullscreenAdExample();
        // 插屏广告样例   
        private InterstitalAdExample interstitalAdExample = new InterstitalAdExample();
        // draw广告样例
        private DrawAdExample drawAdExample = new DrawAdExample();
        // 插全屏广告样例
        private InterstitalFullAdExample interstitalFullAdExample = new InterstitalFullAdExample();

        // 功能分发
        public void FunctionDispatch(string itemNameId)
        {
            switch (itemNameId)
            {
                // 加载和展示原生自渲染
                case MainListItemId.NATIVE_RENDER:
                    nativeAdExample.LoadNormalNativeAd();
                    break;
                // 加载和展示原生模板广告
                case MainListItemId.NATIVE_EXPRESS:
                    nativeAdExample.LoadExpressNativeAd();
                    break;
                // 加载模板竖版激励视频
                case MainListItemId.REWARD_EXPREESS_V:
                    rewardVideoAdExample.LoadExpressRewardAdV();
                    break;
                // 加载模板横版激励视频
                case MainListItemId.REWARD_EXPRESS_H:
                    rewardVideoAdExample.LoadExpressRewardAdH();
                    break;
                // 加载普通竖版激励视频
                case MainListItemId.REWARD_NORMAL_V:
                    rewardVideoAdExample.LoadNormalRewardAdV();
                    break;
                // 展示激励视频
                case MainListItemId.REWARD_SHOW:
                    rewardVideoAdExample.ShowRewardAd();
                    break;
                // 加载模板竖版全屏视频
                case MainListItemId.FULLSCREEN_EXPREESS_V:
                    fullscreenAdExample.LoadExpressFullscreenV();
                    break;
                // 加载模板横版全屏视频
                case MainListItemId.FULLSCREEN_EXPRESS_H:
                    fullscreenAdExample.LoadExpressFullscreenH();
                    break;
                // 加载普通竖版全屏视频
                case MainListItemId.FULLSCREEN_NORMAL_V:
                    fullscreenAdExample.LoadNormalFullscreenV();
                    break;
                // 展示全屏视频
                case MainListItemId.FULLSCREEN_SHOW:
                    fullscreenAdExample.ShowFullscreenAd();
                    break;
                // 加载插屏广告
                case MainListItemId.INTERSTITIAL_LOAD:
                    interstitalAdExample.LoadInterstitalAd(true);
                    break;
                // 展示插屏广告
                case MainListItemId.INTERSTITIAL_SHOW:
                    interstitalAdExample.ShowInterstitalAd();
                    break;
                // 加载普通开屏广告
                case MainListItemId.SPLASH_NORMAL_LOAD:
                    splashAdExample.LoadNormalSplashAd();
                    break;
                // 加载模板开屏广告
                case MainListItemId.SPLASH_EXPRESS_LOAD:
                    splashAdExample.LoadExpressSplashAd();
                    break;
                // 展示开屏广告
                case MainListItemId.SPLASH_SHOW:
                    splashAdExample.ShowSplashAd();
                    break;
                // 加载和展示banner横幅
                case MainListItemId.BANNER_LOAD:
                    bannerAdExample.LoadBannerAd();
                    break;
                // 加载插全屏广告
                case MainListItemId.INTERSTITIALFULL_LOAD:
                    interstitalFullAdExample.LoadInterstitalFullAd(ABUAdPositionId.INTERSTITAL_FULL_SCREEN_CODE);
                    break;
                // 加载插全屏广告
                case MainListItemId.INTERSTITIALFULL_LOAD_2:
                    interstitalFullAdExample.LoadInterstitalFullAd(ABUAdPositionId.INTERSTITAL_FULL_SCREEN_CODE_2);
                    break;
                // 加载插全屏广告
                case MainListItemId.INTERSTITIALFULL_LOAD_3:
                    interstitalFullAdExample.LoadInterstitalFullAd(ABUAdPositionId.INTERSTITAL_FULL_SCREEN_CODE_3);
                    break;
                // 展示插全屏广告
                case MainListItemId.INTERSTITIALFULL_SHOW:
                    interstitalFullAdExample.ShowInterstitalFullAd();
                    break;
                // 加载和展示Draw广告
                case MainListItemId.DRAW_SHOW:
                    drawAdExample.LoadDrawAd();
                    break;
                // 移除掉展示的Draw广告
                case MainListItemId.DRAW_All_CLOSE:
                    drawAdExample.RemoveAllDrawAd();
                    break;
                // 可视化测试
                case MainListItemId.VISIABLE_TEST_TOOL:
                    this.LauchVisiableTestTool();
                    break;
                // 配置流量分组信息
                case MainListItemId.SET_USERSEGMENT:
                    this.setUserSegment();
                    break;
                // 主题模式设置_普通
                case MainListItemId.SET_THEMEMODE_NORMAL:
                    this.setThemeMode(ABUAdSDKThemeStatus.ABUAdSDKThemeStatusNormal);
                    break;
                // 主题模式设置_暗夜
                case MainListItemId.SET_THEMEMODE_NIGHT:
                    this.setThemeMode(ABUAdSDKThemeStatus.ABUAdSDKThemeStatusNight);
                    break;
                // 配置隐私合规相关信息
                case MainListItemId.SET_PRIVACY_CONFIG:
                    this.setPrivacyConfig();
                    break;
                // 询问获得系统地理位置权限(iOS)
                case MainListItemId.GET_SYSTEM_LOCATION_PRIVILEGE:
                    this.getSystemLocationPrivilege();
                    break;
            }
        }

        // 可视化测试工具
        public void LauchVisiableTestTool()
        {
            // 目前仅Android生效
            ABUAdSDK.LauchVisualDebugTool();
            Debug.LogError("<Unity Log>开启可视化测试工具");
        }

        // 配置流量分组信息
        public void setUserSegment()
        {
            // 自定义信息
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");
            dict.Add("key3", "value3");
            // 开发者可根据该接口传入相关配置来进行流量分组
            // 流量分组相关配置
            //设置的自定义分组信息为字母、下划线、连字符、数字等组成，不能为空格，长度小于100
            var segment = new ABUUserInfoForSegment.Builder()
                .SetUserID("1234")//用户id
                .SetChannel("Apple")//渠道
                .SetSubChannel("AppleStore")//子渠道
                .SetUserGroup("group1")//用户组别
                .SetAge(30)//年龄
                .SetGender(ABUUserInfoGender.ABUUserInfoGenderFemale)//性别
                .setCustomDataDictionary(dict)
                .Build();
            string logs = "userID:" + segment.userID + ", channel:" + segment.channel + ", age:" + segment.age + ", custom:" + segment.customDataDictionary;
            ABUAdSDK.SetUserInfoForSegment(segment);
            Debug.LogError("<Unity Log>setUserSegment completed.请在埋点中验证设置成功(测试验证):" + logs);
        }

        // 主题(夜间)模式设置，目前仅支持穿山甲adn
        public void setThemeMode(ABUAdSDKThemeStatus themeStatus)
        {
            ABUAdSDK.SetThemeStatusIfCan(themeStatus);
            Debug.LogError("<Unity Log>themeStatus change to: " + themeStatus);
        }

        // 配置隐私相关信息
        public void setPrivacyConfig()
        {
            var privacyConfig = new ABUPrivacyConfig.Builder()
                .SetLimitPersonalAds(false)         // 是否限制个性化广告,默认为不限制。官方维护版本中只适用于CSJ、Ks、Sigmob、百度、GDT。
                .SetCanUseLocation(false)           // 是否在adn中使用位置,官方维护版本中只适用于CSJ。 
                .SetLongitude(0.5)                  // 当canUseLocation为false时，使用传入的经度的值。默认值是0.0。官方维护版本中只适用于CSJ。
                .SetLatitude(0.5)                   // 当canUseLocation为false时，使用传入的纬度的值。默认值是0.0。官方维护版本中只适用于CSJ。
                .SetNotAdult(false)                 // 是成人或者儿童，true-儿童, false-成人，默认为true(成人)。官方维护版本中只适用于sigmob。
                .SetLimitProgrammaticAds(false)     // 是否限制程序化广告，默认为不限制。官方维护版本中只适用于CSJ、Ks、Sigmob、百度、GDT。
                .SetLimitCAID(false)                // 是否禁止CAID，默认为false。官方维护版本中只适用于百度。
                .SetCanUsePhoneState(false)
                .SetCanUseWifiState(true)
                .SetCanUseWriteExternal(true)
                .Build();

            ABUAdSDK.SetPrivacyConfig(privacyConfig);

            var logStr = "<Unity Log>..." + "SetPrivacyConfig"
                + "；limitPersonalAds:" + privacyConfig.limitPersonalAds
                + "；canUseLocation:" + privacyConfig.canUseLocation
                + "；SetLongitude:" + privacyConfig.longitude
                + "；SetLatitude:" + privacyConfig.latitude
                + "；SetNotAdult:" + privacyConfig.notAdult
                + "；SetLimitProgrammaticAds:" + privacyConfig.limitProgrammaticAds
                + "；limitCAID:" + privacyConfig.limitCAID
                + "；canUsePhoneState:" + privacyConfig.canUsePhoneState
                + "；canUseWifiState:" + privacyConfig.canUseWifiState
                + "；canUseWriteExternal:" + privacyConfig.canUseWriteExternal
                ;
            Log.D(logStr);
            ToastManager.Instance.ShowToast(logStr);
        }

        public void getSystemLocationPrivilege()
        {
#if UNITY_IOS
            ABUiOSBridgeTools.getSystemLocationPrivilege();
#endif
        }

    }

}