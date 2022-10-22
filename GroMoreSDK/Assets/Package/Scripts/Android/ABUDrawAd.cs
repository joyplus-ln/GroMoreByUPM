//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using UnityEngine;

namespace ByteDance.Union
{
#if (!UNITY_EDITOR) && UNITY_ANDROID
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The draw Ad.
    /// </summary>
    public class ABUDrawAd
    {
        private AndroidJavaObject adHandle;
        private ABUDrawAdInteractionCallback interactionCallback;

        private ABUDrawAd(AndroidJavaObject ad)
        {
            adHandle = ad;
        }

        public List<int> getAdIndexs()
        {
            return new List<int>(){1};
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
        internal static void LoadDrawAd(ABUAdUnit adUnit, ABUDrawAdCallback callback)
        {
            if (adUnit == null)
            {
                return;
            }

            var adUnitId = adUnit.unitID;
            var androidListener = new GMDrawAdLoadCallback(callback);
            ABUAdSDK.GetAdManager()?.CallStatic("loadDrawAd", ABUAdSDK.GetActivity(), adUnitId, adUnit.Handle,
                androidListener);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetAdInteractionListener(ABUDrawAdInteractionCallback listener)
        {
            this.interactionCallback = listener;
        }

        /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadListener(ABUAppDownloadCallback listener)
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
            var interactionListener = new GMDrawExpressAdListener(interactionCallback);
            ABUAdSDK.GetAdManager()?.CallStatic("showDrawAd", ABUAdSDK.GetActivity(), adHandle, interactionListener);
        }

        // ADN的名称，与平台配置一致，自定义ADN时为ADN唯一标识
        public string GetAdRitInfoAdnName(int index)
        {
            if (adHandle == null)
            {
                return "";
            }

            AndroidJavaObject gmAdEcpmInfo = adHandle.Call<AndroidJavaObject>("getShowEcpm");
            if (gmAdEcpmInfo == null)
            {
                return "";
            }

            string adnName = gmAdEcpmInfo.Call<string>("getAdNetworkPlatformName");
            return adnName;
        }

        // 获取最佳广告的代码位 该接口需要在show回调之后生效
        public string GetAdNetworkRitId(int index)
        {
            if (adHandle == null)
            {
                return "";
            }

            var ritId = adHandle.Call<string>("getAdNetworkRitId");
            return ritId;
        }

        // 获取最佳广告的预设ecpm 返回显示广告对应的ecpm（该接口需要在show回调之后会返回对应的ecpm），当未在平台配置ecpm会返回-1，当广告加载中未显示会返回-2，当没有权限访问该部分会放回-3  单位：分
        public string GetPreEcpm(int index)
        {
            if (adHandle == null)
            {
                return "";
            }

            string ecpm = adHandle.Call<string>("getPreEcpm");
            return ecpm;
        }

        private class GMDrawAdLoadCallback : AndroidJavaProxy
        {
            private ABUDrawAdCallback _callback;

            public GMDrawAdLoadCallback(ABUDrawAdCallback listener) : base(
                "com.bytedance.msdk.api.v2.ad.draw.GMDrawAdLoadCallback")
            {
                _callback = listener;
            }

            //加载成功
            public void onAdLoadSuccess(AndroidJavaObject adList)
            {
                Debug.Log("onFeedAdLoad  success");
                ABUDrawAd drawAd = null;
                if (adList != null)
                {
                    var size = adList.Call<int>("size");
                    if (size <= 0) return;
                    var drawAds = new List<ABUDrawAd>();
                    for (var i = 0; i < size; ++i)
                    {
                        var ad = new ABUDrawAd(
                            adList.Call<AndroidJavaObject>("get", i));
                        drawAds.Insert(i, ad);
                    }

                    drawAd = drawAds[0];
                }

                UnityDispatcher.PostTask(
                    () => this._callback?.OnDrawAdLoad(drawAd));
            }

            //加载失败
            public void onAdLoadFail(AndroidJavaObject adError)
            {
                if (adError == null) return;
                //todo 日志输出不应该在这里
                Debug.Log("ABUFullScreenVideoAd onFullVideoLoadFail code " + adError.Get<int>("code") + " message " +
                          adError.Get<string>("message"));
                UnityDispatcher.PostTask(
                    () => this._callback?.OnError(adError.Get<int>("code"), adError.Get<string>("message")));
            }
        }

        private class GMDrawExpressAdListener : AndroidJavaProxy
        {
            private ABUDrawAdInteractionCallback _callback;

            public GMDrawExpressAdListener(ABUDrawAdInteractionCallback callback) : base(
                "com.bytedance.msdk.api.v2.ad.draw.GMDrawExpressAdListener")
            {
                _callback = callback;
            }

            /**
     * 点击回调事件
     */
            public void onAdClick()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnAdClicked(0));
            }

            /**
     * 展示回调事件
     */
            public void onAdShow()
            {
                UnityDispatcher.PostTask(() => this._callback?.OnAdShow(0));
            }

            /**
     * 模板渲染失败
     */
            public void onRenderFail(AndroidJavaObject view, string msg, int code)
            {
                UnityDispatcher.PostTask(() => this._callback?.OnRenderFail(msg, code));
            }

            /**
     * 模板渲染成功
     */
            public void onRenderSuccess(float width, float height)
            {
                UnityDispatcher.PostTask(() => this._callback?.OnRenderSuccess(width, height));
            }
        }
    }
#endif
}