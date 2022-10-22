//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------
#if !UNITY_ANDROID
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace ByteDance.Union
{
    /// <summary>
    /// The post processor for xcode.
    /// </summary>
    internal static class XCodePostProcess
    {
        [PostProcessBuild(700)]
        public static void OnPostProcessBuild(
            BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            var projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);
            var targetGUID = "";
#if UNITY_2019_3_OR_NEWER
            targetGUID = proj.GetUnityFrameworkTargetGuid();
#else
            targetGUID = proj.TargetGuidByName("Unity-iPhone");
#endif
            proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
            proj.SetBuildProperty(targetGUID, "ENABLE_BITCODE", "NO");

            // framework to project
            proj.AddFrameworkToProject(targetGUID, "WebKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "VideoToolbox.framework", false);
            proj.AddFrameworkToProject(targetGUID, "SystemConfiguration.framework", false);
            proj.AddFrameworkToProject(targetGUID, "StoreKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Security.framework", false);
            proj.AddFrameworkToProject(targetGUID, "ReplayKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Photos.framework", false);
            proj.AddFrameworkToProject(targetGUID, "MultipeerConnectivity.framework", false);
            proj.AddFrameworkToProject(targetGUID, "MobileCoreServices.framework", false);
            proj.AddFrameworkToProject(targetGUID, "MetalPerformanceShaders.framework", false);
            proj.AddFrameworkToProject(targetGUID, "MediaToolbox.framework", false);
            proj.AddFrameworkToProject(targetGUID, "MediaPlayer.framework", false);
            proj.AddFrameworkToProject(targetGUID, "libresolv.9.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libiconv.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libcompression.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libc++abi.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "JavaScriptCore.framework", false);
            proj.AddFrameworkToProject(targetGUID, "GLKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreTelephony.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AVFoundation.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AudioToolbox.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AssetsLibrary.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Accelerate.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AuthenticationServices.framework", false);
            proj.AddFrameworkToProject(targetGUID, "libil2cpp.a", false);
            proj.AddFrameworkToProject(targetGUID, "libiPhone-lib.a", false);
            proj.AddFrameworkToProject(targetGUID, "AuthenticationServices.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreText.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AVKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CFNetwork.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreGraphics.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreMedia.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreMotion.framework", false);
            proj.AddFrameworkToProject(targetGUID, "CoreVideo.framework", false);
            proj.AddFrameworkToProject(targetGUID, "Foundation.framework", false);
            proj.AddFrameworkToProject(targetGUID, "OpenAL.framework", false);
            proj.AddFrameworkToProject(targetGUID, "OpenGLES.framework", false);
            proj.AddFrameworkToProject(targetGUID, "QuartzCore.framework", false);
            proj.AddFrameworkToProject(targetGUID, "UIKit.framework", false);
            proj.AddFrameworkToProject(targetGUID, "libiconv.2.dylib", false);
            proj.AddFrameworkToProject(targetGUID, "Metal.framework", false);
            proj.AddFrameworkToProject(targetGUID, "libresolv.9.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libc++.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libz.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libbz2.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "libxml2.tbd", false);
            proj.AddFrameworkToProject(targetGUID, "CoreLocation.framework", false);
            proj.AddFrameworkToProject(targetGUID, "ImageIO.framework", false);
            proj.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);

            proj.WriteToFile(projPath);

            //// Modify Info.plist
            var plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;

            rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-3940256099942544~1458002511");
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif