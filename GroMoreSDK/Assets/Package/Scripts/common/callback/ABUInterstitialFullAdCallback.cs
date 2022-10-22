//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The listener for interactionFull Ad.
    /// </summary>
    public interface ABUInterstitialFullAdCallback
    {
        /// <summary>
        /// Invoke when load Ad error.
        /// </summary>
        void OnError(int code, string message);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnInterstitialFullAdLoad(object ad);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnInterstitialFullAdCached();
    }
}