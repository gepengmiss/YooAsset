using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAPK
{
    internal static void Run(BuildTarget buildTarget)
    {
//         var args = System.Environment.GetCommandLineArgs();
//         if (args != null && args.Length > 0)
//         {
//             var lastArg = args[args.Length - 1];
//             if (lastArg.Contains("://"))
//             {
//                 url = lastArg;
//                 Logger.Log("Streaming bundles url changed to " + url);
//             }

//             if (args.Length > 1)
//                 int.TryParse(args[args.Length - 2], out buildNumber);

//             var pidx = Array.IndexOf(args, "-pkgPath");
//             if (pidx > 0)
//             {
//                 PathPrefix = args[pidx + 1];
//             }

//             if (args.Contains("development"))
//                 isDevelopment = true;
            
//             if (args.Contains("dontDownload"))
//                 dontDownload = true;
//         }
        
//         if (buildNumber <= 0)
//             buildNumber = 1;

//         AssetDatabase.Refresh();

        
//         PlayerSettings.applicationIdentifier = "com.camelgames.ig";
//         PlayerSettings.productName = "Infinite Galaxy";

//         EditorUserBuildSettings.androidCreateSymbolsZip = true;
//         PlayerSettings.Android.useAPKExpansionFiles = true;  //用于设置是否分包成obb
//         SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android,
//             "ENABLE_OBB",
//             "RELEASE",
//             "ENABLE_APPSFLYER",
//             "ENABLE_TRACKER_FIREBASE",
//             "CHANNEL_GOOGLE_CHECKOUT",
//             "FACEBOOK",
//             "ENABLE_VK"
//             );

        
//         UnityEngine.Debug.Log(string.Format("Game Name {0} pkgName {1}", PlayerSettings.productName, PlayerSettings.applicationIdentifier));

//         //PlayerSettings.Android.targetDevice = AndroidTargetDevice.ARMv7;
//         PlayerSettings.Android.androidIsGame = true;
//         PlayerSettings.bundleVersion = GameConfig.Version;
//         PlayerSettings.Android.bundleVersionCode = buildNumber;
//         PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
//         PlayerSettings.Android.maxAspectRatio = 2.4f;

// #if UNITY_ANDROID
//         UnityEditor.Android.AndroidExternalToolsSettings.gradlePath = "/Users/builder2018/devtools/gradle-6.1.1";
// #endif
//         EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
//         PlayerSettings.keystorePass = "123456";
//         PlayerSettings.keyaliasPass = "123456";
        
        
//         if (BuilderTools.packageType != BuilderTools.PackageType.HMS)
//         {
//             EditorUtil.DeleteFileOrDirectory("Assets/Plugins/Android/HMS.androidlib");
//         }
//         if (BuilderTools.packageType != BuilderTools.PackageType.CN)
//         {
//             EditorUtil.DeleteFileOrDirectory("Assets/Plugins/Android/CNMain.androidlib");
//         }
//         else
//         {
//             EditorUtil.DeleteFileOrDirectory("Assets/Channel/Google/source/camelgames/GoogleBridge.java");
//             EditorUtil.DeleteFileOrDirectory("Assets/Channel/Google/source/camelgames/googleplatform/GoogleReferReceiver.java");
//         }
        
//         var manifestFile = "Assets/Plugins/Android/AndroidManifest.xml";
//         FileUtil.ReplaceFile("Assets/Channel/Google/AndroidManifest.xml", manifestFile);
//         FileUtil.ReplaceDirectory("Assets/Channel/Google/res/", "Assets/Plugins/Android/res/");

//         FileUtil.DeleteFileOrDirectory("Assets/Plugins/Android/res/values-zh-rCN/");
//         FileUtil.DeleteFileOrDirectory("Assets/Plugins/Android/res/values-zh-rCN.meta");
        
//         // copy SDK
//         var remains = new string[] {"GooglePlayGames", "Firebase", "FacebookSDK", "VKSDK", "AppsFlyer"};
//         CleanupSDKs(remains);

//         ReplaceGradleMainTemplate("/*Getui Start*/", "/*");
//         ReplaceGradleMainTemplate("/*Getui End*/", "*/");
//         ReplaceGradleLauncherTemplate("/*Getui Start*/", "/*");
//         ReplaceGradleLauncherTemplate("/*Getui End*/", "*/");
 
//         AssetDatabase.Refresh();

 
//         if (EditorUserBuildSettings.activeBuildTarget != this.buildTarget)
//         {
//             EditorUserBuildSettings.SwitchActiveBuildTarget(this.buildTarget);
//         }

//         var scenesToBuild = GetScenePaths();
//         packagePath = GetPackagePath();
//         UnityEngine.Debug.Log("packagePath " + packagePath);

//         var opt = BuildOptions.None;
//         if (isDevelopment)
//         {
//             opt = BuildOptions.Development; // | BuildOptions.EnableDeepProfilingSupport;
//             UnityEngine.Debug.Log("This is a Development build.");
//         }

//         BuildPipeline.BuildPlayer(scenesToBuild.ToArray(), GetPackagePath(), this.buildTarget, opt);


//         if (PlayerSettings.Android.useAPKExpansionFiles)
//         {
//             // apk和obb文件名格式如下：
//             //Odyssey-Android-Official-2002082015-28.apk
//             //Odyssey-Android-Official-2002082015-28.main.obb
//             //main.2.com.Demo.ABC.obb
//             var fn = Path.GetFileName(packagePath);
//             var path = packagePath.Substring(0, packagePath.Length - fn.Length);

//             var obbPath = packagePath.Substring(0, packagePath.Length - 3) + "main.obb";
//             Debug.Log("PostProccess " + obbPath + " path " + path);
//             if (File.Exists(obbPath))
//             {
//                 var obbName = string.Format("main.{0}.{1}.obb", buildNumber, PlayerSettings.applicationIdentifier);
//                 var newObbPath = Path.Combine(path, obbName);
//                 Debug.Log(string.Format(" path = {0}, obbName = {1}, newObbPath = {2}", path, obbName, newObbPath));
//                 if (File.Exists(newObbPath))
//                 {
//                     File.Delete(newObbPath);
//                 }
//                 File.Move(obbPath, newObbPath);
//             }
//         }


//         FileUtil.DeleteFileOrDirectory("Assets/Plugins/Android/AndroidManifest.xml");
//         FileUtil.DeleteFileOrDirectory("Assets/Plugins/Android/res");
//         AssetDatabase.Refresh();

    } 

}
