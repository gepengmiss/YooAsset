using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildTool
{
#region 菜单
    [MenuItem("BuildTool/AssetBundles for Windows-Mac")]
    internal static void BuildAssetBundles()
    {
        BuildTarget buildTarget = BuildTarget.StandaloneWindows;
#if UNITY_STANDALONE_OSX
        buildTarget = BuildTarget.StandaloneOSX;
#endif
        Ab(buildTarget); 
    }
    [MenuItem("BuildTool/AssetBundles for Android")]
    static void BuildAndroidAssetBundles()
    {
        Ab(BuildTarget.Android); 
    }

    [MenuItem("BuildTool/AssetBundles for iOS")]
    static void BuildiOSAssetBundles()
    {
        Ab(BuildTarget.iOS);  
    }

    [MenuItem("BuildTool/Exe")]
    static void ReleaseWindows()
    {
        BuildTarget buildTarget = BuildTarget.StandaloneWindows;
#if UNITY_STANDALONE_OSX
        buildTarget = BuildTarget.StandaloneOSX;
#endif
        Apk(buildTarget); 
    }

    [MenuItem("BuildTool/Apk")]
    static void ReleaseAndroid()
    {
        Apk(BuildTarget.Android);
    }

    [MenuItem("BuildTool/Ipa")]
    static void ReleaseIOS()
    {
        Apk(BuildTarget.iOS);
    }
#endregion  


#region AssetBundle
    static void Ab(BuildTarget buildTarget)
    {
        BuildAB.Run(buildTarget);
    }
#endregion  

#region 包体
    static void Apk(BuildTarget buildTarget)
    {
        BuildAPK.Run(buildTarget);
    }
#endregion 

}
