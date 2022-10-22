//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The draw Ad.
    /// </summary>
    public class ABUDrawAd
    {
        /// <summary>
        /// Only used by iOS.
        /// </summary>
        /// <returns></returns>
        public List<int> getAdIndexs()
        {
            return null;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 异步加载广告
        /// Loads the draw ad.
        /// </summary>
        /// <param name="adUnit">draw ad dto.</param>
        /// <param name="callback">Callback.</param>
        internal static void LoadDrawAd(ABUAdUnit adUnit, ABUDrawAdCallback listener)
        {
        }

        /// <summary>
        /// 移除掉序号index位置的广告
        /// </summary>
        /// <param name="index">序号</param>
        public void CloseDrawAd(int index)
        {
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetAdInteractionListener(ABUDrawAdInteractionCallback listener)
        {
        }

        /// <summary>
        /// show the express Ad
        /// <param name="type">the type of ad</param>
        /// <param name="x">the origin x of th ad</param>
        /// <param name="y">the origin y of th ad</param>
        /// </summary>
        public void ShowDrawAd(int index, float x, float y)
        {
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public string GetAdRitInfoAdnName(int index)
        {
            return null;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public string GetAdNetworkRitId(int index)
        {
            return null;
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public string GetPreEcpm(int index)
        {
            return null;
        }
    }
#endif
}
