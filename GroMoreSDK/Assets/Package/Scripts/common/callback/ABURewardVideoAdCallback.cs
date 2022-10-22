namespace ByteDance.Union
{
    using System.Collections.Generic;
    /// <summary>
    /// 激励视频加载回调
    /// reward adload callback.
    /// </summary>
    public interface ABURewardVideoAdCallback
    {
        /// <summary>
        /// Invoke when load Ad error.
        /// </summary>
        void OnError(int code, string message);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnRewardVideoAdLoad(object ad);

        /// <summary>
        /// Invoke when the Ad load success.
        /// </summary>
        void OnRewardVideoAdCached();
    }
}
