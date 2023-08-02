using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YooAsset.Editor;
using YooAsset;
using System;

public class BuildAB
{
    internal static void Run(BuildTarget buildTarget)
    {
        BuildParameters buildParameters = new BuildParameters();
        buildParameters.StreamingAssetsRoot = AssetBundleBuilderHelper.GetDefaultStreamingAssetsRoot();
        buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        buildParameters.BuildTarget = buildTarget;
        buildParameters.BuildPipeline = AssetBundleBuilderSettingData.Setting.BuildPipeline;
        buildParameters.BuildMode = AssetBundleBuilderSettingData.Setting.BuildMode;
        buildParameters.PackageName = AssetBundleBuilderSettingData.Setting.BuildPackage;
        buildParameters.PackageVersion = GetBuildPackageVersion();
        buildParameters.VerifyBuildingResult = true;
        buildParameters.SharedPackRule = new ZeroRedundancySharedPackRule();
        buildParameters.EncryptionServices = CreateEncryptionServicesInstance(2);
        buildParameters.CompressOption = AssetBundleBuilderSettingData.Setting.CompressOption;
        buildParameters.OutputNameStyle = AssetBundleBuilderSettingData.Setting.OutputNameStyle;
        buildParameters.CopyBuildinFileOption = AssetBundleBuilderSettingData.Setting.CopyBuildinFileOption;
        buildParameters.CopyBuildinFileTags = AssetBundleBuilderSettingData.Setting.CopyBuildinFileTags;

        if (AssetBundleBuilderSettingData.Setting.BuildPipeline == EBuildPipeline.ScriptableBuildPipeline)
        {
            buildParameters.SBPParameters = new BuildParameters.SBPBuildParameters();
            buildParameters.SBPParameters.WriteLinkXML = true;
        }

        var builder = new AssetBundleBuilder();
        var buildResult = builder.Run(buildParameters);
        if (buildResult.Success)
        {
            EditorUtility.RevealInFinder(buildResult.OutputPackageDirectory);
        }
    } 

    // 构建版本相关
    private static string GetBuildPackageVersion()
    {
        int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
        return DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
    }
    private static List<Type> GetEncryptionServicesClassTypes()
    {
        return EditorTools.GetAssignableTypes(typeof(IEncryptionServices));
    }
    private static IEncryptionServices CreateEncryptionServicesInstance(int index)
    {
        if (index < 0)
            return null;
        var classType = GetEncryptionServicesClassTypes()[index];
        return (IEncryptionServices)Activator.CreateInstance(classType);
    }



}
