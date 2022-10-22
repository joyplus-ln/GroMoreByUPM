using UnityEngine;

namespace ByteDance.Union
{
    // 轻量级数据存储
    public static class LiteDataUtil
    {
        // 获取语言类型，默认值1为中文
        public static LanguageType GetLanguageType()
        {
            return (LanguageType)PlayerPrefs.GetInt(PlayerPreferenceConstant.KeyLanguage, 1);
        }

        // 设置语言类型
        public static void SetLanguageType(LanguageType languageType)
        {
            PlayerPrefs.SetInt(PlayerPreferenceConstant.KeyLanguage, (int)languageType);
        }


        // 设置openId
        public static void SetOpenId(string openId)
        {
            PlayerPrefs.SetString(PlayerPreferenceConstant.KeyOpenId, openId);
        }

        // 获取存储的openId
        public static string GetOpenId()
        {
            return PlayerPrefs.GetString(PlayerPreferenceConstant.KeyOpenId, string.Empty);
        }
    }

    public static class GlobalUtil
    {
        // 是否是测试环境
        private static bool isDebug = false;

        public static bool IsDebug
        {
            get { return isDebug; }
            set { isDebug = value; }
        }
    }
}