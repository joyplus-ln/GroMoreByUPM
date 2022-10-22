
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The android bridge of the union SDK.
    /// </summary>
    /// public sealed class ABUAdUnit
    public sealed class ABUConstantHelper
    {
        // ritscene类型的key，value为ABURitSceneType；用于激励，全屏展示
        public static string ABUShowExtroInfoKeySceneType = "ABUShowExtroInfoKeySceneType";
        // ritscene描述的key，当ABUShowExtroInfoKeySceneType=BURitSceneType_custom时需要设置该key；用于激励，全屏展示
        public static string ABUShowExtroInfoKeySceneDescription = "ABUShowExtroInfoKeySceneDescription";


        // iOS隐私配置key-CAID控制
        public static string ABUPrivacyForbiddenCAID = "ABUPrivacyForbiddenCAID";
        // iOS隐私配置key-个性化推荐
        public static string ABUPrivacyLimitPersonalAds = "ABUPrivacyLimitPersonalAds";
        // iOS隐私配置key-程序化广告
        public static string ABUPrivacyLimitProgrammaticAds = "ABUPrivacyLimitProgrammaticAds";
        // iOS隐私配置key-是否在adn中使用位置
        public static string ABUPrivacyCanLocation = "ABUPrivacyCanLocation";
        // iOS隐私配置key-设置经度值
        public static string ABUPrivacyLongitude = "ABUPrivacyLongitude";
        // iOS隐私配置key-设置经度值
        public static string ABUPrivacyLatitude = "ABUPrivacyLatitude";
        // iOS隐私配置key-成人或者儿童
        public static string ABUPrivacyNotAdult = "ABUPrivacyNotAdult";
    }
}
