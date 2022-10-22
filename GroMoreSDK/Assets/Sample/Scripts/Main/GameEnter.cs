using System;
using UnityEngine;

namespace ByteDance.Union
{
    // demo入口，在GameInit 之后
    public class GameEnter : MonoBehaviour
    {
        [Header("摄像机")] [SerializeField] private Camera _camera;

        private void Awake()
        {
            // 加载主页面
            var mainDeskTopUi = Instantiate(PrefabLoader.LoadMainDeskTopPrefab(), null).transform;

            // 指定摄像机
            mainDeskTopUi.GetComponent<Canvas>().worldCamera = this._camera;

            // 主页面不销毁
            DontDestroyOnLoad(mainDeskTopUi.gameObject);
        }
    }
}