namespace ByteDance.Union
{
#if UNITY_IOS
    using System.Runtime.InteropServices;

    public class ABUiOSBridgeTools
    {

        static public float getScreenWidth()
        {
            return UnionPlatform_BridgeTools_GetScreenWidth();
        }

        static public float getScreenHeight()
        {
            return UnionPlatform_BridgeTools_GetScreenHeight();
        }

        static public float getWindowSafeAreaInsetsTop()
        {
            return UnionPlatform_BridgeTools_SafeAreaInsets_Top();
        }

        static public float getWindowSafeAreaInsetsLeft()
        {
            return UnionPlatform_BridgeTools_SafeAreaInsets_Left();
        }

        static public float getWindowSafeAreaInsetsBottom()
        {
            return UnionPlatform_BridgeTools_SafeAreaInsets_Bottom();
        }

        static public float getWindowSafeAreaInsetsRight()
        {
            return UnionPlatform_BridgeTools_SafeAreaInsets_Right();
        }

        static public void getSystemLocationPrivilege()
        {
            UnionPlatform_BridgeTools_getSystemLocationPrivilege();
        }

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_GetScreenWidth();

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_GetScreenHeight();

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_SafeAreaInsets_Top();

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_SafeAreaInsets_Left();

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_SafeAreaInsets_Bottom();

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_SafeAreaInsets_Right();

        [DllImport("__Internal")]
        private static extern float UnionPlatform_BridgeTools_getSystemLocationPrivilege();
    }
#endif
}
