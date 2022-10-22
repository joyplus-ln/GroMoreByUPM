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
    /// The interaction listener for banner Ad.
    /// </summary>
    public interface ABUBannerAdInteractionCallback
    {
        /// <summary>
        /// Invoke when the Ad is clicked.
        /// </summary>
        void OnAdClicked();

        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow();

        /// <summary>
        /// Invokw when the Ad is dissmissed.
        /// </summary>
        void OnAdDismiss();


    }
}
