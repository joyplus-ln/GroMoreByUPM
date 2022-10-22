//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The orientation of an Ad.
    /// </summary>
    public enum ABUAdInteractionType
    {
        /// <summary>
        /// 普通广告类型-浏览器查看详细信息
        /// The browser.
        /// </summary>
        BROWSER = 2,

        /// <summary>
        /// 普通广告类型-落地页
        /// The landing page.
        /// </summary>
        LANDING_PAGE = 3,

        /// <summary>
        /// 应用下载类型
        /// The download.
        /// </summary>
        DOWNLOAD = 4,

        /// <summary>
        /// 电话拨打类型
        /// The dial.
        /// </summary>
        DIAL = 5,

        /// <summary>
        /// 未知类型
        /// The unknown.
        /// </summary>
        UNKNOWN = -1,
    }
}
