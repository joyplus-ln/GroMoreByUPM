using System;
using UnityEngine;
using UnityEngine.UI;

namespace ByteDance.Union
{
    // header Layout
    public class MainHeaderLayoutScript : BaseUi
    {
        [Header("版本号文本框")] [SerializeField] private Text appVersionText;

        [Header("首页app描述文本")] [SerializeField] private Text appDescriptionText;

        [Header("国际化切换按钮")] [SerializeField] private Button internationalizationSwitchButton;

        [Header("首页Logo")] [SerializeField] private Button logoImage;

        private MultipleClickDelegate multipleClickDelegate;

        // 初始化
        public void Init(Action<bool> internationalizationSwitchButtonListener, Action reloadViewListener)
        {
            // 语言切换
            this.internationalizationSwitchButton.onClick.AddListener(() =>
            {
                // 当前如果是中文就切换为英文，如果是英文就切换会中文
                //LiteDataUtil.SetLanguageType(
                //    LiteDataUtil.GetLanguageType() == LanguageType.Chinese
                //        ? LanguageType.English
                //        : LanguageType.Chinese);

                // 回调
                if (internationalizationSwitchButtonListener != null)
                {
                    internationalizationSwitchButtonListener.Invoke(
                        LiteDataUtil.GetLanguageType() == LanguageType.Chinese);
                }
            });

            // debug环境切换
            if (!GlobalUtil.IsDebug)
            {
                this.multipleClickDelegate = new MultipleClickDelegate(3000, 6, () =>
                {
                    ToastManager.Instance.ShowToast("开启debug环境");
                    GlobalUtil.IsDebug = true;
                    this.logoImage.onClick.RemoveAllListeners();

                    // 回调
                    if (reloadViewListener != null)
                    {
                        reloadViewListener.Invoke();
                    }
                });
                this.logoImage.onClick.AddListener(() => { this.multipleClickDelegate.OnClick(); });
            }

            this.OnRefreshUi();
        }

        // 刷新Ui
        public override void OnRefreshUi()
        {
            this.internationalizationSwitchButton.GetComponent<Image>().sprite =
                LocalImageLoader.LoadMainSwitchIcon(
                    LiteDataUtil.GetLanguageType() == LanguageType.Chinese
                        ? "icon_switch_cn"
                        : "icon_switch_en");

            this.appVersionText.text = AppStringText.SdkVersion;
            this.appDescriptionText.text = AppStringText.SdkDescription;
        }

        // 资源释放
        public override void OnRelease()
        {
        }
    }
}