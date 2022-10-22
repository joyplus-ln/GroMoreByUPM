//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_ANDROID
//#if !UNITY_EDITOR && UNITY_ANDROID
    using UnityEngine;

    /// <summary>
    /// The native Ad.
    /// </summary>
    public class NativeAd
    {
        protected readonly AndroidJavaObject ad;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeAd"/> class.
        /// </summary>
        internal NativeAd(AndroidJavaObject ad)
        {
            this.ad = ad;
        }

        /// <summary>
        /// Gets the title for this Ad.
        /// </summary>
        public string GetTitle()
        {
            return this.ad.Call<string>("getTitle");
        }

        /// <summary>
        /// Gets the description for this Ad.
        /// </summary>
        public string GetDescription()
        {
            return this.ad.Call<string>("getDescription");
        }
        
        public string GetIconUrl()
        {
            return this.ad.Call<string>("getIconUrl");
        }
        
        public string GetImageUrl()
        {
            return this.ad.Call<string>("getImageUrl");
        }
        
        public int GetImageWidth()
        {
            return this.ad.Call<int>("getImageWidth");
        }
        
        public int GetImageHeight()
        {
            return this.ad.Call<int>("getImageHeight");
        }
        
        /// <summary>
        /// Gets the Ecpm for this Ad.
        /// </summary>
        public string GetPreEcpm()
        {
            return this.ad.Call<string>("getPreEcpm");
        }

        public string GetPackageName()
        {
            return this.ad.Call<string>("getPackageName");
        }
        
        public AndroidJavaObject GetAdLogoView()
        {
            return this.ad.Call<AndroidJavaObject>("getAdLogoView");
        }
        
        public AndroidJavaObject GetExpressView()
        {
            return this.ad.Call<AndroidJavaObject>("getExpressView");
        }
         public string GetAdNetworkRitId()
        {
            return this.ad.Call<string>("getAdNetworkRitId");
        }

        /// <summary>
        /// Gets the button text.
        /// </summary>
        public string GetButtonText()
        {
            return this.ad.Call<string>("getActionText");
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExpressAd()
        {
            return this.ad.Call<bool>("isExpressAd");
        }
        
        public int GetSdkNumType()
        {
            return this.ad.Call<int>("getSdkNumType");
        }
        
        public bool HasDislike()
        {
            return this.ad.Call<bool>("hasDislike");
        }

        public int GetAdNetworkPlatformId()
        {
            return this.ad.Call<int>("getAdNetworkPlatformId");
        }
        
        public int GetAdImageMode()
        {
            return this.ad.Call<int>("getAdImageMode");
        }
        
        public void Render()
        { 
            this.ad.Call("render");
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        public string GetSource()
        {
            return this.ad.Call<string>("getSource");
        }
        
        public AndroidJavaObject GetImageList()
        {
            return this.ad.Call<AndroidJavaObject>("getImageList");
        }
        

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return this.ad.Call<int>("getInteractionType");
        }

        /// <summary>
        /// Gets the image mode.
        /// </summary>
        public int GetImageMode()
        {
            return this.ad.Call<int>("getImageMode");
        }

        // /// <summary>
        // /// Sets the download listener.
        // /// </summary>
        // public void SetDownloadListener(ABUAppDownloadCallback listener)
        // {
        //     var androidListener = new AppDownloadListener(listener);
        //     this.ad.Call("setDownloadListener", androidListener);
        // }
    }
#endif
}
