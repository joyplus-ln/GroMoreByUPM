using UnityEngine;

namespace ByteDance.Union
{
    // toast 管理类
    public class ToastManager : MonoBehaviour
    {
        private static ToastManager _toastManager;


        // 通用canvas 的 transform
        private Transform _commonCanvasTransform;

        // 通用toast GameObject
        private GameObject _toastGameObject;

        // 单例
        public static ToastManager Instance
        {
            get
            {
                _toastManager = FindObjectOfType(typeof(ToastManager)) as ToastManager;
                if (_toastManager == null)
                {
                    var o = new GameObject(typeof(ToastManager).Name);
                    _toastManager = o.AddComponent<ToastManager>();
                    DontDestroyOnLoad(o);
                }

                return _toastManager;
            }
        }

        private void Awake()
        {
            // 加载一个不销毁的通用canvas
            this._commonCanvasTransform = Instantiate(PrefabLoader.LoadCommonCanvasPrefab(), null).transform;
            DontDestroyOnLoad(this._commonCanvasTransform.gameObject);
        }


        // 展示toast
        public void ShowToast(string message)
        {
            // android 中的主线程和unity中的主线程不是一个线程
            UnitThreadUtil.QueueOnMainThread(() =>
            {
                var toastLayoutScript = this.GetToastLayoutScript();
                if (toastLayoutScript != null)
                {
                    toastLayoutScript.SetToastContent(message);
                }
            });
        }

        // 获取toast布局脚本
        private ToastLayoutScript GetToastLayoutScript()
        {
            if (this._commonCanvasTransform == null)
            {
                Debug.LogError("_commonCanvasTransform is null...");
                return null;
            }

            if (this._toastGameObject == null)
            {
                this._toastGameObject = Instantiate(PrefabLoader.LoadToastLayoutPrefab(), this._commonCanvasTransform);
            }

            return this._toastGameObject == null ? null : this._toastGameObject.GetComponent<ToastLayoutScript>();
        }

        // 关闭ToastUI
        public void HideToast()
        {
            if (this._toastGameObject != null)
            {
                Destroy(this._toastGameObject);
                this._toastGameObject = null;
            }
        }
    }
}