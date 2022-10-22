using UnityEngine;

namespace ByteDance.Union
{
    public static class PrefabLoader
    {
        #region 主页面Item prefab路径

        // 主页面一级列表布局
        private const string MainItemPath = "Prefabs/PrimaryLevelItem";

        // 主页面二级列表布局
        private const string SecondLevelItemPath = "Prefabs/SecondLevelItem";

        // 主页面三级列表布局
        private const string ThirdLevelItemPath = "Prefabs/ThirdLevelItem";

        // 输入框类型的三级列表
        private const string ThirdLevelInputItemPath = "Prefabs/ThirdLevelInputItem";

        // 首页顶部布局路径
        private const string MainHeaderLayoutPath = "Prefabs/MainHeaderLayout";

        #endregion

        // 详情页面布局路径
        private const string DetailItemViewPath = "Prefabs/DetailItemView";

        // toast 布局路径
        private const string ToastLayoutPath = "Prefabs/ToastLayout";

        // 公用的Canvas
        private const string CommonCanvasPath = "Prefabs/CommonCanvas";

        // 主桌面
        private const string MainDeskTopPath = "Prefabs/MainUICanvas";

        // 加载主页面
        public static GameObject LoadMainDeskTopPrefab()
        {
            return LoadPrefabGameObjectByResource(MainDeskTopPath);
        }

        // 加载主页面Item 的prefab
        public static GameObject LoadMainItemPrefab()
        {
            return LoadPrefabGameObjectByResource(MainItemPath);
        }


        // 加载主页面二级列表Item的 prefab
        public static GameObject LoadSecondItemPrefab()
        {
            return LoadPrefabGameObjectByResource(SecondLevelItemPath);
        }

        // 加载主页面三级列表Item的 prefab
        public static GameObject LoadThirdItemPrefab()
        {
            return LoadPrefabGameObjectByResource(ThirdLevelItemPath);
        }

        // 加载输入框类型三级列表Item的 prefab
        public static GameObject LoadThirdInputItemPrefab()
        {
            return LoadPrefabGameObjectByResource(ThirdLevelInputItemPath);
        }

        // 首页头部布局的 prefab
        public static GameObject LoadMainHeaderLayoutPrefab()
        {
            return LoadPrefabGameObjectByResource(MainHeaderLayoutPath);
        }

        // 加载详情页itemView的 prefab
        public static GameObject LoadDetailItemViewPrefab()
        {
            return LoadPrefabGameObjectByResource(DetailItemViewPath);
        }

        // 家在toast布局的prefab
        public static GameObject LoadToastLayoutPrefab()
        {
            return LoadPrefabGameObjectByResource(ToastLayoutPath);
        }

        // 加载通用canvas 的prefab
        public static GameObject LoadCommonCanvasPrefab()
        {
            return LoadPrefabGameObjectByResource(CommonCanvasPath);
        }

        // 通过Resource 加载prefab的GameObject
        private static GameObject LoadPrefabGameObjectByResource(string path)
        {
            return Resources.Load<GameObject>(path);
        }
    }
}