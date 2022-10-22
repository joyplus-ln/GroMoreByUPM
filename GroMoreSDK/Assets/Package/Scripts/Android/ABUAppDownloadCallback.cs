// //------------------------------------------------------------------------------
// // Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// // All Right Reserved.
// // Unauthorized copying of this file, via any medium is strictly prohibited.
// // Proprietary and confidential.
// //------------------------------------------------------------------------------
//
// namespace ByteDance.Union
// {
// #if UNITY_ANDROID
// //#if !UNITY_EDITOR && UNITY_ANDROID
// #pragma warning disable SA1300
// #pragma warning disable IDE1006
//     using UnityEngine;
//
//     /// <summary>
//     /// The android proxy listener for <see cref="IAppDownloadListener"/>.
//     /// </summary>
//     internal sealed class ABUAppDownloadCallback : AndroidJavaProxy
//     {
//         private readonly ABUAppDownloadCallback callback;
//
//         /// <summary>
//         /// Initializes a new instance of the <see cref="T:ByteDance.Union.LGAppDownloadCallback"/> class.
//         /// </summary>
//         /// <param name="callback">Callback.</param>
//         // public LGAppDownloadCallback(
//         //     ILGAppDownloadCallback callback)
//         //     : base("com.ss.union.sdk.ad.callback.LGAppDownloadCallback")
//         // {
//         //     this.callback = callback;
//         // }
//
//         /// <summary>
//         /// Ons the idle.
//         /// </summary>
//         public void onIdle()
//         {
//              this.callback.OnIdle();
//         }
//
//         /// <summary>
//         /// Ons the download active.
//         /// </summary>
//         /// <param name="totalBytes">Total bytes.</param>
//         /// <param name="currBytes">Curr bytes.</param>
//         /// <param name="fileName">File name.</param>
//         /// <param name="appName">App name.</param>
//         public void onDownloadActive(
//             long totalBytes, long currBytes, string fileName, string appName)
//         {
//              this.callback.OnDownloadActive(
//                 totalBytes, currBytes, fileName, appName);
//         }
//
//         /// <summary>
//         /// Ons the download paused.
//         /// </summary>
//         /// <param name="totalBytes">Total bytes.</param>
//         /// <param name="currBytes">Curr bytes.</param>
//         /// <param name="fileName">File name.</param>
//         /// <param name="appName">App name.</param>
//         public void onDownloadPaused(
//             long totalBytes, long currBytes, string fileName, string appName)
//         {
//             this.callback.OnDownloadPaused(
//                 totalBytes, currBytes, fileName, appName);
//         }
//
//         /// <summary>
//         /// Ons the download failed.
//         /// </summary>
//         /// <param name="totalBytes">Total bytes.</param>
//         /// <param name="currBytes">Curr bytes.</param>
//         /// <param name="fileName">File name.</param>
//         /// <param name="appName">App name.</param>
//         public void onDownloadFailed(
//             long totalBytes, long currBytes, string fileName, string appName)
//         {
//              this.callback.OnDownloadFailed(
//                 totalBytes, currBytes, fileName, appName);
//         }
//
//         /// <summary>
//         /// Ons the download finished.
//         /// </summary>
//         /// <param name="totalBytes">Total bytes.</param>
//         /// <param name="fileName">File name.</param>
//         /// <param name="appName">App name.</param>
//         public void onDownloadFinished(
//             long totalBytes, string fileName, string appName)
//         {
//              this.callback.OnDownloadFinished(
//                 totalBytes, fileName, appName);
//         }
//
//         /// <summary>
//         /// Ons the installed.
//         /// </summary>
//         /// <param name="fileName">File name.</param>
//         /// <param name="appName">App name.</param>
//         public void onInstalled(string fileName, string appName)
//         {
//             this.callback.OnInstalled(fileName, appName);
//         }
//     }
//
// #pragma warning restore SA1300
// #pragma warning restore IDE1006
// #endif
// }
