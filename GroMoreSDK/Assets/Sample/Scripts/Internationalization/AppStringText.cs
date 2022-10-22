namespace ByteDance.Union
{
    public static class AppStringText
    {
        // sdk 版本，中间的空格不能省略
        public const string SdkVersion = "MSDK-Unity";

        // sdk 描述信息
        public static string SdkDescription
        {
            get { return IsChinese() ? "功能演示" : "FUNCTION SHOW"; }
        }

        // 检测当前是否是中文环境
        private static bool IsChinese()
        {
            return LiteDataUtil.GetLanguageType() == LanguageType.Chinese;
        }
    }
}