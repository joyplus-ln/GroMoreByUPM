namespace ByteDance.Union
{
    public static class MainListItemId
    {
        // ############ 基础功能ID ################
        public const string BASIC_FUNCTION = "basic_function";  // 基础功能

        // 原生广告
        public const string NATIVE_AD = "native_ad";
        public const string NATIVE_RENDER = "native_render";    // 原生自渲染
        public const string NATIVE_EXPRESS = "native_express";  // 原生模板

        // 激励视频
        public const string REWARD_VIDEO_AD = "reward_video_ad";
        public const string REWARD_EXPREESS_V = "reward_express_v"; // 加载模板竖屏
        public const string REWARD_EXPRESS_H = "reward_express_h";  // 加载模板横屏
        public const string REWARD_NORMAL_V = "reward_normal_v";    // 加载普通竖屏
        public const string REWARD_SHOW = "reward_show";            // 展示激励视频

        // 全屏视频
        public const string FULLSCREEN_VIDEO_AD = "fullscreen_video_ad";
        public const string FULLSCREEN_EXPREESS_V = "fullscreen_express_v"; // 加载模板竖屏
        public const string FULLSCREEN_EXPRESS_H = "fullscreen_express_h";  // 加载模板横屏
        public const string FULLSCREEN_NORMAL_V = "fullscreen_normal_v";    // 加载普通竖屏
        public const string FULLSCREEN_SHOW = "fullscreen_show";            // 展示激励视频

        // 插屏广告
        public const string INTERSTITIAL_AD = "interstitial_ad";
        public const string INTERSTITIAL_LOAD = "interstitial_load"; // 加载插屏广告
        public const string INTERSTITIAL_SHOW = "interstitial_show"; // 展示插屏广告

        // 开屏广告
        public const string SPLASH_AD = "splash_ad";
        public const string SPLASH_NORMAL_LOAD = "splash_normal_load"; // 加载普通开屏广告
        public const string SPLASH_EXPRESS_LOAD = "splash_express_load"; // 加载模板开屏广告
        public const string SPLASH_SHOW = "splash_show"; // 展示开屏广告

        // 横幅广告(Banner)
        public const string BANNER_AD = "banner_ad";
        public const string BANNER_LOAD = "banner_load_show"; // 加载和展示banner横幅

        // 插全屏广告
        public const string INTERSTITIALFULL_AD = "interstitialFull_ad";
        public const string INTERSTITIALFULL_LOAD = "interstitialFull_load"; // 加载
        public const string INTERSTITIALFULL_LOAD_2 = "interstitialFull_load_2"; // 加载
        public const string INTERSTITIALFULL_LOAD_3 = "interstitialFull_load_3"; // 加载
        public const string INTERSTITIALFULL_SHOW = "interstitialFull_show"; // 展示

        // Draw广告
        public const string DRAW_AD = "draw_ad";
        public const string DRAW_SHOW = "draw_show"; // 展示draw广告
        public const string DRAW_All_CLOSE = "draw_all_Close"; // 移除掉展示的广告


        // ############ 辅助功能 ################
        public const string BASIC_TEST = "uitls_tool";  // 辅助功能

        //  测试工具 
        public const string UITLS_TOOL = "uitls_tool";
        public const string VISIABLE_TEST_TOOL = "visiable_test_tool";    // 可视化测试
        public const string SET_USERSEGMENT = "set_userSegment";    // 设置用户流量分组信息
        public const string SET_THEMEMODE_NORMAL = "set_theme_normal";    // 设置普通主题
        public const string SET_THEMEMODE_NIGHT = "set_theme_night";    // 设置夜间主题
        public const string SET_PRIVACY_CONFIG = "set_privacy_config";    // 设置隐私合规相关信息
        public const string GET_SYSTEM_LOCATION_PRIVILEGE = "get_system_location_privilege";    // 询问获得系统地理位置权限(iOS)

    }
}