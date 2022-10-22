namespace ByteDance.Union
{
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// 横幅加载回调
    /// Dou yin share callback.
    /// </summary>
    public interface ABUBannerAdCallback
    {
        /// <summary>
        /// Invoke when load Ad error.
        /// </summary>
        void OnError(int code, string message);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnBannerAdLoad(ABUBannerAd ad);

        /// <summary>
        /// Ons the other rit  in waterfall occur filll error，Call back after show.
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
