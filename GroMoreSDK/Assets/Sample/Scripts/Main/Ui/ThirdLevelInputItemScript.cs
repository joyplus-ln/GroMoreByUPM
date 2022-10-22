using UnityEngine;
using UnityEngine.UI;

namespace ByteDance.Union
{
    // 输入框类型的三级列表
    public class ThirdLevelInputItemScript : BaseUi
    {
        [Header("输入框")] [SerializeField] private InputField inputField;

        [Header("内容文本")] [SerializeField] private Text contentText;

        [Header("预制文本")] [SerializeField] private Text inputFieldPlaceholder;

        [Header("分割线布局")] [SerializeField] private GameObject divideViewLayout;

        // 而级列表实体类
        private SecondLevelFunctionsEntity _secondLevelFunctionsEntity;

        // 三级列表实体类
        private ThirdLevelFunctionsEntity _thirdLevelFunctionsEntity;

        private void Start()
        {
            // 监听文本框的数据变化
            this.inputField.onValueChanged.AddListener(val =>
            {
                if (this._thirdLevelFunctionsEntity != null)
                {
                    //switch (this._thirdLevelFunctionsEntity.NameId)
                    //{
                    //    //// 输入支付金额
                    //    //case MainListItemId.PAYMENT_ENTER_AMOUNT:

                    //    //    PaymentFunctionScript.Instance.Total_amount = val;

                    //    //    break;

                    //    //// 悬浮窗X轴坐标
                    //    //case MainListItemId.X_AXIS_COORDINATES_OF_FLOATING_WINDOW:

                    //    //    CrossPromotionFunctionScript.Instance.InputX = val;

                    //    //    break;

                    //    //// 悬浮窗Y轴坐标
                    //    //case MainListItemId.Y_AXIS_COORDINATES_OF_FLOATING_WINDOW:

                    //    //    CrossPromotionFunctionScript.Instance.InputY = val;

                    //    //    break;
                    //}
                }
            });
        }


        // 初始化三级列表数据内容
        public void InitThirdItemData(
            SecondLevelFunctionsEntity secondLevelFunctionsEntity,
            ThirdLevelFunctionsEntity thirdLevelFunctionsEntity)
        {
            this._secondLevelFunctionsEntity = secondLevelFunctionsEntity;
            this._thirdLevelFunctionsEntity = thirdLevelFunctionsEntity;
            this.OnRefreshUi();
        }

        // 隐藏分割线
        public void HideDividerViewLayout()
        {
            if (this.divideViewLayout != null && this.divideViewLayout.gameObject != null)
            {
                this.divideViewLayout.gameObject.SetActive(false);
            }
        }

        // 刷新UI
        public override void OnRefreshUi()
        {
            this.inputField.placeholder.GetComponent<Text>().text = this._thirdLevelFunctionsEntity == null
                ? string.Empty
                : this._thirdLevelFunctionsEntity.GetContent();
        }

        // 释放资源
        public override void OnRelease()
        {
        }
    }
}