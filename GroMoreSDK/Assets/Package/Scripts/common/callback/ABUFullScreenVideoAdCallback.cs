namespace ByteDance.Union
{
    using System.Collections.Generic;
    /// <summary>
    /// 全屏视频加载回调
    /// Dou yin share callback.
    /// </summary>
    public interface ABUFullScreenVideoAdCallback
    {
        /// <summary>
        /// Invoke when load Ad error.
        /// </summary>
        void OnError(int code, string message);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnFullScreenVideoAdLoad(object ad);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnFullScreenVideoAdCached();
    }
}
