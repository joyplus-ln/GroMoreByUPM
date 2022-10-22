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
    internal static class XCodePostProcessSample
    {
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(
            BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            /// Modify project.pbxproj
            var projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);
           

            proj.WriteToFile(projPath);


            //// Modify Info.plist
            var plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;

            rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-3940256099942544~1458002511");
            rootDict.SetString("NSLocationWhenInUseUsageDescription", "使用时获取地理位置");
            
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif