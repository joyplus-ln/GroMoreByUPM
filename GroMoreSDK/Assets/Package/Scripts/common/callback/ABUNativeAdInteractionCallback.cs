//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The interaction listener for draw Ad.
    /// </summary>
    public interface ABUNativeAdInteractionCallback
    {
        /// <summary>
        /// Invoke when the Ad is clicked.
        /// </summary>
        void OnAdClicked(int index);

        /// <summary>
        /// Invoke when the Ad is shown.
        /// </summary>
        void OnAdShow(int index);

        /// <summary>
        /// Invokw when the Ad is dissmissed.
        /// </summary>
        void OnAdDismiss(int index);

        /**
        * 模板渲染失败
        * @param view
        */
        void OnRenderFail(string msg, int code);

        /**
        * 模板渲染成功
        * @param view
        * @param width  返回view的宽 单位 dp
        * @param height 返回view的高 单位 dp
        */
        void OnRenderSuccess(float width, float height);

    }
}
