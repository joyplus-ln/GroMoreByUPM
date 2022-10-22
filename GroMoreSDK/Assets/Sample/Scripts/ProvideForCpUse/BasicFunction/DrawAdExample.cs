using System.Collections.Generic;
using ByteDance.Union.Constant;
using UnityEngine;

namespace ByteDance.Union
{
    public sealed class DrawAdExample
    {
        public ABUDrawAd drawAd;

        /// <summary>
        ///  加载和展示Draw广告
        /// </summary>
        public void LoadDrawAd()
        {
            var adUnit = new ABUAdUnit.Builder()
                .SetCodeId(ABUAdPositionId.DRAW_CODE)
                .SetImageAcceptedSize(300 * 3, 400 * 3)
                .SetAdCount(1)
                .Build();
            ABUDrawAd.LoadDrawAd(adUnit, new DrawAdListener(this));
        }

        /// <summary>
        /// 移除掉正在展示的draw广告
        /// </summary>
        public void RemoveAllDrawAd()
        {
#if UNITY_IOS
            if (this.drawAd != null)
            {
                List<int> lists = this.drawAd.getAdIndexs();
                for (int i = 0; i < lists.Count; i++)
                {
                    this.drawAd.CloseDrawAd(i);
                }
                this.drawAd = null;
            }
#else

#endif
        }
    }


    public sealed class DrawAdListener : ABUDrawAdCallback
    {
        private DrawAdExample example;

        public DrawAdListener(DrawAdExample example)
        {
            this.example = example;
        }

        public void OnDrawAdLoad(ABUDrawAd drawAd)
        {
            var errMsg = "OnDrawAdLoad";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
            this.example.drawAd = drawAd;
            drawAd.SetAdInteractionListener(new DrawAdInteractionCallback(this.example));
            List<int> lists = drawAd.getAdIndexs();
            float y = 100;
            for (int i = 0; i < lists.Count; i++)
            {
                Debug.Log("chaors" + i + "drawAd ShowNativeAd");

                // 展示广告, 位置自己进行赋值
                y = y + 400 * i;// 100 500 900
#if UNITY_IOS
                y = (int)ABUiOSBridgeTools.getWindowSafeAreaInsetsBottom() + y;  
#endif
                drawAd.ShowDrawAd(i, 100, y);

                string ecpm = drawAd.GetPreEcpm(i);
                string ritID = drawAd.GetAdNetworkRitId(i);
                string adnName = drawAd.GetAdRitInfoAdnName(i);
                Debug.Log("<Unity Log>..." + ", ecpm:" + ecpm + ",  " + "ritID:" + ritID + ",  " + "adnName:" + adnName);
            }
        }

        public void OnError(int code, string message)
        {
            var errMsg = "DrawAd OnError-- code : " + code + "--message : " + message;
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }


        /// <summary>
        /// Ons the other rit  in waterfall occur filll error.Call back after show.
        /// fillFailMessageInfo:Json string whose outer layer is an array,and the array elements are dictionaries.
        /// The keys of Internal dictionary are the following:
        /// 1."mediation_rit": 广告代码位
        /// 2.@"adn_name": 属于哪家广告adn
        /// 3."error_message": 错误信息
        /// 4."error_code": 错误码
        /// </summary>
        public void OnWaterfallRitFillFail(string fillFailMessageInfo)
        {
            // 开发者根据上述规则解开字符串获取对应adn广告加载失败信息
            Debug.Log("<Unity Log>...fillFailMessageInfo:" + fillFailMessageInfo);
        }
    }


    public sealed class DrawAdInteractionCallback : ABUDrawAdInteractionCallback
    {
        private DrawAdExample example;

        public DrawAdInteractionCallback(DrawAdExample example)
        {
            this.example = example;
        }

        public void OnAdClicked(int index)
        {
            var errMsg = "DrawAd + OnAdClicked";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnAdDismiss(int index)
        {
            var errMsg = "DrawAd + OnAdDismiss";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnAdShow(int index)
        {
            var errMsg = "DrawAd + OnAdShow";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnRenderFail(string msg, int code)
        {
            var errMsg = "DrawAd + OnRenderFail";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }

        public void OnRenderSuccess(float width, float height)
        {
            var errMsg = "DrawAd + OnRenderSuccess";
            Debug.LogError("<Unity Log>..." + errMsg);
            ToastManager.Instance.ShowToast(errMsg);
        }
    }
}