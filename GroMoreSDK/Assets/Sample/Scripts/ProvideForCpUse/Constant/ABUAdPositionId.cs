namespace ByteDance.Union.Constant
{
    // 测试广告位id 
    public static class ABUAdPositionId
    {
#if UNITY_IOS
        // 激励视频
        public const string REWARD_VIDEO_EXPRESS_H_CODE ="945490586";
        public const string REWARD_VIDEO_EXPRESS_V_CODE = "945494739";
        public const string REWARD_VIDEO_NORMAL_CODE = "945494747";
        // 信息流
        public const string NATIVE_NORMAL_CODE = "945494760";
        public const string NATIVE_EXPRESS_CODE = "945494759";
        // 全屏视频
        public const string FULL_SCREEN_VIDEO_EXPRESS_H_CODE = "945494751";
        public const string FULL_SCREEN_VIDEO_EXPRESS_V_CODE = "945494750";
        public const string FULL_SCREEN_VIDEO_NORMAL_V_CODE = "945494752";
        // 插屏
        public const string INTERSTITAL_CODE = "946108691";
        // 开屏
        public const string SPLASH_EXPRESS_CODE = "887418500";
        public const string SPLASH_NORMA_CODE = "887383142";
        // 开屏兜底
        public const string SPLASH_BASELINE_APPID = "5000546";
        public const string SPLASH_BASELINE_CODE = "800546808";
        // 横幅
        public const string BANNER_CODE = "945494753";
        // 插全屏
        public const string INTERSTITAL_FULL_SCREEN_CODE = "946968708";
        public const string INTERSTITAL_FULL_SCREEN_CODE_2 = "947028072";
        public const string INTERSTITAL_FULL_SCREEN_CODE_3 = "946961656";
        // draw
        public const string DRAW_CODE = "948423177";
#else
        // 激励视频
        public const string REWARD_VIDEO_EXPRESS_H_CODE = "945700410";
        public const string REWARD_VIDEO_EXPRESS_V_CODE = "945493668";
        public const string REWARD_VIDEO_NORMAL_CODE = "945493672";
        // 信息流
        public const string NATIVE_NORMAL_CODE = "945700641";
        public const string NATIVE_EXPRESS_CODE = "945683970";
        // 全屏视频
        public const string FULL_SCREEN_VIDEO_EXPRESS_H_CODE = "945700424";
        public const string FULL_SCREEN_VIDEO_EXPRESS_V_CODE = "945493674";
        public const string FULL_SCREEN_VIDEO_NORMAL_V_CODE = "945493676";
        // 插屏
        public const string INTERSTITAL_CODE = "945493679";
        // 开屏
        public const string SPLASH_EXPRESS_CODE = "887382965";
        public const string SPLASH_NORMA_CODE = "887382967";
        // 开屏兜底
        public const string SPLASH_BASELINE_APPID = "5001121";
        public const string SPLASH_BASELINE_CODE = "887382976";
        // 横幅
        public const string BANNER_CODE = "945493677";
        // 插全屏
        public const string INTERSTITAL_FULL_SCREEN_CODE = "102068464";
        public const string INTERSTITAL_FULL_SCREEN_CODE_2 = "945864901";
        public const string INTERSTITAL_FULL_SCREEN_CODE_3 = "945854486";
        // draw
        public const string DRAW_CODE = "102068429";
#endif
    }

}