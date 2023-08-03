using UnityEngine;
using UniFramework.Event;
using UniFramework.Singleton;
using YooAsset;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UniFramework.Singleton;
 

public class Boot : MonoBehaviour
{
	/// <summary>
	/// 资源系统运行模式
	/// </summary>
	public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

	void Awake()
	{
		Debug.Log($"资源系统运行模式：{PlayMode}");
		Application.targetFrameRate = 60;
		Application.runInBackground = true;
	}
	void Start()
	{
		// 初始化事件系统
		UniEvent.Initalize();

		// 初始化单例系统
		UniSingleton.Initialize();

		// 初始化资源系统
		YooAssets.Initialize();
		YooAssets.SetOperationSystemMaxTimeSlice(30);

		// 创建补丁管理器
		UniSingleton.CreateSingleton<PatchManager>();

		// 开始补丁更新流程
		// PatchManager.Instance.Run(PlayMode);

		// 离线模式
		// OfflineStart();

		// 边下边玩
		PlayWithDownloadStart();
	}
 
	void PlayWithDownloadStart()
	{
		// 异步下载
		PatchManager.Instance.Run(PlayMode);
		// 开始游戏
		var go = Resources.Load<GameObject>("PatchWindow");
		GameObject.Instantiate(go);
		this.StartCoroutine(PlayWithDownloadStartCor());
	}

	private IEnumerator PlayWithDownloadStartCor()
	{
		// 创建默认的资源包 
		var package = YooAssets.CreatePackage(YooAssets.DefaultPackage);
		YooAssets.SetDefaultPackage(package);
		 
		var createParameters = new OfflinePlayModeParameters();
		createParameters.DecryptionServices = new GameDecryptionServices();
		InitializationOperation initializationOperation = package.InitializeAsync(createParameters); 
		yield return initializationOperation;

		if (initializationOperation.Status == EOperationStatus.Succeed) {
			UnityEngine.Debug.Log("operation.Status:   " + initializationOperation.Status);
		} else {
			Debug.LogWarning($"{initializationOperation.Error}");
			yield break;
		}

		var operation = package.UpdatePackageVersionAsync(true, 30);
		yield return operation;

		UnityEngine.Debug.Log("operation.Status:   " + operation.Status);

		// 开始游戏 
		PatchEventDefine.PatchStatesChange.SendEventMessage("开始游戏！");
		// 创建游戏管理器
		UniSingleton.CreateSingleton<GameManager>();
		// 开启游戏流程
		GameManager.Instance.Run(); 
	}
 
	void OfflineStart()
	{
		// 加载更新面板
		var go = Resources.Load<GameObject>("PatchWindow");
		GameObject.Instantiate(go);
		this.StartCoroutine(OfflineStartCor());
	}

	private IEnumerator OfflineStartCor()
	{
		// 创建默认的资源包
		string packageName = "DefaultPackage";
		var package = YooAssets.TryGetPackage(packageName);
		if (package == null) {
			package = YooAssets.CreatePackage(packageName);
			YooAssets.SetDefaultPackage(package);
		} 
		var createParameters = new OfflinePlayModeParameters();
		createParameters.DecryptionServices = new GameDecryptionServices();
		InitializationOperation initializationOperation = package.InitializeAsync(createParameters); 
		yield return initializationOperation;
		if (initializationOperation.Status == EOperationStatus.Succeed) {
			UnityEngine.Debug.Log("operation.Status:   " + initializationOperation.Status);
		} else {
			Debug.LogWarning($"{initializationOperation.Error}");
			yield break;
		}

		var operation = package.UpdatePackageVersionAsync(true, 30);
		yield return operation;

		UnityEngine.Debug.Log("operation.Status:   " + operation.Status);

		// if (operation.Status == EOperationStatus.Succeed)
		// {
		// 	// 如果获取远端资源版本成功，说明当前网络连接通畅，可以走正常更新流程。
		// 	// ......
		// }
		// else
		{
			// 如果获取远端资源版本失败，说明当前网络无连接。
			// 在正常开始游戏之前，需要验证本地清单内容的完整性。
			string packageVersion = package.GetPackageVersion();
			var operationDownload = package.PreDownloadContentAsync(packageVersion);
			yield return operationDownload;
			if (operationDownload.Status != EOperationStatus.Succeed)
			{
				PatchEventDefine.PatchStatesChange.SendEventMessage("请检查本地网络，有新的游戏内容需要更新！");
				// ShowMessageBox("请检查本地网络，有新的游戏内容需要更新！");
				yield break;
			}
			
			int downloadingMaxNum = 10;
			int failedTryAgain = 3;
			int timeout = 60;
			var downloader = operationDownload.CreateResourceDownloader(downloadingMaxNum, failedTryAgain, timeout);
			if (downloader.TotalDownloadCount > 0)   
			{
				// 资源内容本地并不完整，需要提示玩家联网更新。
				PatchEventDefine.PatchStatesChange.SendEventMessage("请检查本地网络，有新的游戏内容需要更新！");
				// ShowMessageBox("请检查本地网络，有新的游戏内容需要更新！");
				yield break;
			}
			
			// 开始游戏
			// StartGame();
			PatchEventDefine.PatchStatesChange.SendEventMessage("开始游戏！");
		}
	}
 
 
	/// <summary>
	/// 资源文件解密服务类
	/// </summary>
	class GameDecryptionServices : IDecryptionServices
	{
		public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
		{
			return 32;
		}

		public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
		{
			throw new NotImplementedException();
		}

		public Stream LoadFromStream(DecryptFileInfo fileInfo)
		{
			BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			return bundleStream;
		}

		public uint GetManagedReadBufferSize()
		{
			return 1024;
		}
	}
}