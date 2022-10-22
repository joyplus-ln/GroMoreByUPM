using UnityEngine;

namespace ByteDance.Union
{
    /**
     * 平台Util
     */
    public static class PlatformUtil
    {
        /**
         * 是否是Android 平台
         */
        public static bool IsAndroid()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        /**
         * 是否是Ios平台
         */
        public static bool IsIos()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }
}