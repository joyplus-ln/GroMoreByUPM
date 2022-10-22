//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------
namespace ByteDance.Union
{
    /// <summary>
    /// The listener for native Ad.
    /// </summary>
    public interface ABUNativeAdCallback
    {
        /// <summary>
        /// Invoke when load Ad error.
        /// </summary>
        void OnError(int code, string message);

        /// <summary>
        /// Invoke when the Ad load success.
        /// nativeAd 广告类
        /// </summary>
        void OnNativeAdLoad(ABUNativeAd nativeAd);

        /// <summary>
        /// Ons the other rit  in waterfall occur filll error.Call back after load.
        /// fillFailMessageInfo:Json string whose outer layer is an array,and the array elements are dictionaries.
        /// The keys of Internal dictionary are the following:
        /// 1."mediation_rit": 广告代码位
        /// 2.@"adn_name": 属于哪家广告adn
        /// 3."error_message": 错误信息
        /// 4."error_code": 错误码
        /// </summary>
        void OnWaterfallRitFillFail(string fillFailMessageInfo);
    }
}
