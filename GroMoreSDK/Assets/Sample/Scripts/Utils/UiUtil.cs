using UnityEngine;
using UnityEngine.UI;

namespace ByteDance.Union
{
    // UI 相关工具类
    public static class UiUtil
    {
        // 通过Font获取文本的宽度
        public static float GetTextWidth(string text, Font font, int fontSize)
        {
            font.RequestCharactersInTexture(text, fontSize, FontStyle.Normal);
            var width = 0f;
            foreach (var t in text)
            {
                CharacterInfo info;
                font.GetCharacterInfo(t, out info, fontSize);
                width += info.advance;
            }

            return width;
        }


        // 重制GameObject的 posZ 为0
        public static void ResetPosZTo0(GameObject gameObject)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            var anchoredPosition3D = rectTransform.anchoredPosition3D;
            var x = anchoredPosition3D.x;
            var y = anchoredPosition3D.y;
            anchoredPosition3D = new Vector3(x, y, 0);
            rectTransform.anchoredPosition3D = anchoredPosition3D;
        }
    }
}