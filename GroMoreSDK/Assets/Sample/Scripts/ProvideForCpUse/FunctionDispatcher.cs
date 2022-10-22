using UnityEngine;

namespace ByteDance.Union
{
    // 所有demo中的功能使用方式，都由此类分发
    public static class FunctionDispatcher
    {
        // 分发所有功能 Item的点击
        public static void HandleItemClick(string secondListNameId, string itemNameId)
        {
            Debug.Log("itemNameId:" + itemNameId + ":  " + secondListNameId);
            AdFunctionScript.Instance.FunctionDispatch(itemNameId);
        }
    }
}