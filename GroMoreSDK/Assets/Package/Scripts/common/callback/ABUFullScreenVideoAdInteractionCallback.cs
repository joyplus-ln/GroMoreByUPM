//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    using System.Collections.Generic;

    /// <summary>
    /// The interaction listener for full screen video Ad.
    /// </summary>
    public interface ABUFullScreenVideoAdInteractionCallback
    {

        /// <summary>
        /// Invoke when the Ad is render fail.
        /// </summary>
        void OnViewRenderFail(int code, string message);

        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow();

        /// <summary>
        /// Invoke when the Ad video bar is clicked.
        /// </summary>
        void OnAdVideoBarClick();

        /// <summary>
        /// Invoke when the Ad is closed.
        /// </summary>
        void OnAdClose();

        /// <summary>
        /// Invoke when the video is complete.
        /// </summary>
        void OnVideoComplete();

        /// <summary>
        /// Invoke when the video is skipped.
        /// </summary>
        void OnSkippedVideo();

        /// <summary>
        /// Invoke when the video has an error.
        /// </summary>
        void OnVideoError(int errorCode, string errorMsg);

        /// <summary>
        /// Ons the other rit  in waterfall occur filll error.Call back after show.
        /// fillFailMessageInfo:Json string whose outer layer is an array,and the array elements are dictionaries.
        /// The keys of Internal dictionary are the following:
        /// 1."mediation_rit": 广告代码位
        /// 2.@"adn_name": 属于哪家广告adn
        /// 3."error_message": 错误信息
        /// 4."error_code": 错误码
        /// </summary>
        void OnWaterfallRitFillFail(string fillFailMessageInfo);

        /// <summary>
        /// Fail to show ad.Now only for iOS.
        /// errcode 错误码
        /// errorMsg 错误描述
        /// </summary>
        void OnAdShowFailed(int errcode, string errorMsg);

        /// <summary>
        /// 奖励发放验证信息. 目前支持的adn:GDT
        /// </summary>
        void OnRewardVerify(bool rewardVerify, ABUAdapterRewardAdInfo rewardInfo);
    }
}
