//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
    /// <summary>
    /// The interaction listener for dislike.
    /// </summary>
    public interface ABUDislikeInteractionListener
    {
        /// <summary>
        /// Invoke when the dislike is selected.
        /// </summary>
        void OnSelected(int var1, string var2);

        /// <summary>
        /// Invoke when the dislike is cancel.
        /// </summary>
        void OnCancel();

        /// <summary>
        /// Refuse to submit again.
        /// </summary>
        void OnRefuse();




    }
}
