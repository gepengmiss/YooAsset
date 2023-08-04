using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace YooAsset
{
	// 边下边玩：
	// 1.每一个方法都需要重构一下。
	// 2.获取资源需要检查两个package，看属于哪个package，再加载资源, 如果两个package都有该资源，属于资源需要更新的情况，使用newpackage里面的。
	public static partial class YooAssets
	{
		private static ResourcePackage _defaultPackage; // 边下边玩， 内置在包体里面的资源
		private static ResourcePackage _newPackage; // 边下边玩， 后台更新的资源

		/// <summary>
		/// 设置默认的资源包
		/// </summary>
		public static void SetDefaultPackage(ResourcePackage package)
		{
			_defaultPackage = package;
		}

		/// <summary>
		/// 设置更新到的资源包
		/// </summary>
		public static void SetNewPackage(ResourcePackage package)
		{
			_newPackage = package;
		}

		private static ResourcePackage GetResPackage(string location)
		{
			if(_newPackage != null && _newPackage.IsReady)
			{
				AssetInfo assetInfo = _newPackage.ConvertLocationToAssetInfo(location, null);  
				if(!assetInfo.IsInvalid) {
					return _newPackage;
				} 
			}
			return _defaultPackage;
		}

		private static ResourcePackage GetResPackage(AssetInfo assetInfo)
		{
			if(!assetInfo.IsInvalid) {
				return assetInfo.GetPackageAsset().PackageName == YooAssets.DefaultPackage ? _defaultPackage : _newPackage;
			}
			return _defaultPackage;
		}

		#region 资源信息
		/// <summary>
		/// 是否需要从远端更新下载
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public static bool IsNeedDownloadFromRemote(string location)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null)
				return _defaultPackage.IsNeedDownloadFromRemote(location);
			return _newPackage.IsNeedDownloadFromRemote(location) || _defaultPackage.IsNeedDownloadFromRemote(location);
		}

		/// <summary>
		/// 是否需要从远端更新下载
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public static bool IsNeedDownloadFromRemote(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null)
				return _defaultPackage.IsNeedDownloadFromRemote(assetInfo);
			return _newPackage.IsNeedDownloadFromRemote(assetInfo) || _defaultPackage.IsNeedDownloadFromRemote(assetInfo);
		}

		/// <summary>
		/// 获取资源信息列表
		/// 两个package有共同资源名，优先使用newpackage里面的资源
		/// </summary>
		/// <param name="tag">资源标签</param>
		public static AssetInfo[] GetAssetInfos(string tag)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.GetAssetInfos(tag);
			} 
			List<AssetInfo> result = new List<AssetInfo>(100); 
			var array2 =_newPackage.GetAssetInfos(tag);
			var cache = new Dictionary<string, AssetInfo>();
			foreach (var assetInfo in array2) {  
				result.Add(assetInfo); 
				cache.Add(assetInfo.AssetPath, assetInfo);
			}
			var array1 =_defaultPackage.GetAssetInfos(tag);
			foreach (var assetInfo in array1) { 
				if (!cache.ContainsKey(assetInfo.AssetPath)) 
					result.Add(assetInfo); 
			}
			return result.ToArray(); 
		}

		/// <summary>
		/// 获取资源信息列表
		/// 两个package有共同资源名，优先使用newpackage里面的资源
		/// </summary>
		/// <param name="tags">资源标签列表</param>
		public static AssetInfo[] GetAssetInfos(string[] tags)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.GetAssetInfos(tags);
			}
			List<AssetInfo> result = new List<AssetInfo>(100); 
			var array2 =_newPackage.GetAssetInfos(tags);
			var cache = new Dictionary<string, AssetInfo>(); 
			foreach (var assetInfo in array2) {  
				result.Add(assetInfo);
				cache.Add(assetInfo.AssetPath, assetInfo); 
			}
			var array1 =_defaultPackage.GetAssetInfos(tags);
			foreach (var assetInfo in array1) { 
				if (!cache.ContainsKey(assetInfo.AssetPath))  
					result.Add(assetInfo); 
			}
			return result.ToArray(); 
		}

		/// <summary>
		/// 获取资源信息
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public static AssetInfo GetAssetInfo(string location)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.GetAssetInfo(location);
			} 
			var assetInfo2 = _newPackage.GetAssetInfo(location);
			return assetInfo2.IsInvalid ? assetInfo2 : _defaultPackage.GetAssetInfo(location);
		}

		/// <summary>
		/// 检查资源定位地址是否有效
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public static bool CheckLocationValid(string location)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.CheckLocationValid(location);
			} 
			return _newPackage.CheckLocationValid(location) || _defaultPackage.CheckLocationValid(location);
		}
		#endregion

		#region 原生文件
		/// <summary>
		/// 同步加载原生文件
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static RawFileOperationHandle LoadRawFileSync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadRawFileSync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadRawFileSync(assetInfo);
		}

		/// <summary>
		/// 同步加载原生文件
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public static RawFileOperationHandle LoadRawFileSync(string location)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadRawFileSync(location);
			} 
			var package = GetResPackage(location);
			return package.LoadRawFileSync(location);
		}

		/// <summary>
		/// 异步加载原生文件
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static RawFileOperationHandle LoadRawFileAsync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadRawFileSync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadRawFileSync(assetInfo);
		}

		/// <summary>
		/// 异步加载原生文件
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public static RawFileOperationHandle LoadRawFileAsync(string location)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadRawFileAsync(location);
			} 
			var package = GetResPackage(location);
			return package.LoadRawFileAsync(location);
		}
		#endregion

		#region 场景加载
		/// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="location">场景的定位地址</param>
		/// <param name="sceneMode">场景加载模式</param>
		/// <param name="suspendLoad">场景加载到90%自动挂起</param>
		/// <param name="priority">优先级</param>
		public static SceneOperationHandle LoadSceneAsync(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.LoadSceneAsync(location, sceneMode, suspendLoad, priority);
			} 
			var package = GetResPackage(location);

			YooLogger.Error("============ LoadSceneAsync: " + package.PackageName + "   location:" + location);


			return package.LoadSceneAsync(location, sceneMode, suspendLoad, priority);
		}

		/// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="assetInfo">场景的资源信息</param>
		/// <param name="sceneMode">场景加载模式</param>
		/// <param name="suspendLoad">场景加载到90%自动挂起</param>
		/// <param name="priority">优先级</param>
		public static SceneOperationHandle LoadSceneAsync(AssetInfo assetInfo, LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSceneAsync(assetInfo, sceneMode, suspendLoad, priority);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadSceneAsync(assetInfo, sceneMode, suspendLoad, priority);
		}
		#endregion

		#region 资源加载
		/// <summary>
		/// 同步加载资源对象
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static AssetOperationHandle LoadAssetSync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAssetSync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadAssetSync(assetInfo);
		}

		/// <summary>
		/// 同步加载资源对象
		/// </summary>
		/// <typeparam name="TObject">资源类型</typeparam>
		/// <param name="location">资源的定位地址</param>
		public static AssetOperationHandle LoadAssetSync<TObject>(string location) where TObject : UnityEngine.Object
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.LoadAssetSync<TObject>(location);
			} 
			var package = GetResPackage(location);
			return package.LoadAssetSync<TObject>(location);
		}

		/// <summary>
		/// 同步加载资源对象
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="type">资源类型</param>
		public static AssetOperationHandle LoadAssetSync(string location, System.Type type)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAssetSync(location, type);
			} 
			var package = GetResPackage(location);
			return package.LoadAssetSync(location, type);
		}


		/// <summary>
		/// 异步加载资源对象
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static AssetOperationHandle LoadAssetAsync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAssetAsync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadAssetAsync(assetInfo);
		}

		/// <summary>
		/// 异步加载资源对象
		/// </summary>
		/// <typeparam name="TObject">资源类型</typeparam>
		/// <param name="location">资源的定位地址</param>
		public static AssetOperationHandle LoadAssetAsync<TObject>(string location) where TObject : UnityEngine.Object
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAssetAsync<TObject>(location);
			} 
			var package = GetResPackage(location);
			return package.LoadAssetAsync<TObject>(location);
		}

		/// <summary>
		/// 异步加载资源对象
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="type">资源类型</param>
		public static AssetOperationHandle LoadAssetAsync(string location, System.Type type)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAssetAsync(location, type);
			} 
			var package = GetResPackage(location);
			return package.LoadAssetAsync(location, type);
		}
		#endregion

		#region 资源加载
		/// <summary>
		/// 同步加载子资源对象
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static SubAssetsOperationHandle LoadSubAssetsSync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSubAssetsSync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadSubAssetsSync(assetInfo);
		}

		/// <summary>
		/// 同步加载子资源对象
		/// </summary>
		/// <typeparam name="TObject">资源类型</typeparam>
		/// <param name="location">资源的定位地址</param>
		public static SubAssetsOperationHandle LoadSubAssetsSync<TObject>(string location) where TObject : UnityEngine.Object
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSubAssetsSync<TObject>(location);
			} 
			var package = GetResPackage(location);
			return package.LoadSubAssetsSync<TObject>(location);
		}

		/// <summary>
		/// 同步加载子资源对象
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="type">子对象类型</param>
		public static SubAssetsOperationHandle LoadSubAssetsSync(string location, System.Type type)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSubAssetsSync(location, type);
			} 
			var package = GetResPackage(location);
			return package.LoadSubAssetsSync(location, type);
		}


		/// <summary>
		/// 异步加载子资源对象
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static SubAssetsOperationHandle LoadSubAssetsAsync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSubAssetsAsync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadSubAssetsAsync(assetInfo);
		}

		/// <summary>
		/// 异步加载子资源对象
		/// </summary>
		/// <typeparam name="TObject">资源类型</typeparam>
		/// <param name="location">资源的定位地址</param>
		public static SubAssetsOperationHandle LoadSubAssetsAsync<TObject>(string location) where TObject : UnityEngine.Object
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSubAssetsAsync<TObject>(location);
			} 
			var package = GetResPackage(location);
			return package.LoadSubAssetsAsync<TObject>(location);
		}

		/// <summary>
		/// 异步加载子资源对象
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="type">子对象类型</param>
		public static SubAssetsOperationHandle LoadSubAssetsAsync(string location, System.Type type)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadSubAssetsAsync(location, type);
			} 
			var package = GetResPackage(location);
			return package.LoadSubAssetsAsync(location, type);
		}
		#endregion

		#region 资源加载
		/// <summary>
		/// 同步加载资源包内所有资源对象
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static AllAssetsOperationHandle LoadAllAssetsSync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAllAssetsSync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadAllAssetsSync(assetInfo);
		}

		/// <summary>
		/// 同步加载资源包内所有资源对象
		/// </summary>
		/// <typeparam name="TObject">资源类型</typeparam>
		/// <param name="location">资源的定位地址</param>
		public static AllAssetsOperationHandle LoadAllAssetsSync<TObject>(string location) where TObject : UnityEngine.Object
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAllAssetsSync<TObject>(location);
			} 
			var package = GetResPackage(location);
			return package.LoadAllAssetsSync<TObject>(location);
		}

		/// <summary>
		/// 同步加载资源包内所有资源对象
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="type">子对象类型</param>
		public static AllAssetsOperationHandle LoadAllAssetsSync(string location, System.Type type)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAllAssetsSync(location, type);
			} 
			var package = GetResPackage(location);
			return package.LoadAllAssetsSync(location, type);
		}


		/// <summary>
		/// 异步加载资源包内所有资源对象
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		public static AllAssetsOperationHandle LoadAllAssetsAsync(AssetInfo assetInfo)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAllAssetsAsync(assetInfo);
			} 
			var package = GetResPackage(assetInfo);
			return package.LoadAllAssetsAsync(assetInfo);
		}

		/// <summary>
		/// 异步加载资源包内所有资源对象
		/// </summary>
		/// <typeparam name="TObject">资源类型</typeparam>
		/// <param name="location">资源的定位地址</param>
		public static AllAssetsOperationHandle LoadAllAssetsAsync<TObject>(string location) where TObject : UnityEngine.Object
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAllAssetsAsync<TObject>(location);
			} 
			var package = GetResPackage(location);
			return package.LoadAllAssetsAsync<TObject>(location);
		}

		/// <summary>
		/// 异步加载资源包内所有资源对象
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		/// <param name="type">子对象类型</param>
		public static AllAssetsOperationHandle LoadAllAssetsAsync(string location, System.Type type)
		{
			DebugCheckDefaultPackageValid(); 
			if(_newPackage == null) {
				return _defaultPackage.LoadAllAssetsAsync(location, type);
			} 
			var package = GetResPackage(location);
			return package.LoadAllAssetsAsync(location, type);
		}
		#endregion

		#region 资源下载
		/// <summary>
		/// 创建资源下载器，用于下载当前资源版本所有的资源包文件
		/// </summary>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateResourceDownloader(int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateResourceDownloader(downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateResourceDownloader(downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateResourceDownloader(downloadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建资源下载器，用于下载指定的资源标签关联的资源包文件
		/// </summary>
		/// <param name="tag">资源标签</param>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateResourceDownloader(string tag, int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateResourceDownloader(new string[] { tag }, downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateResourceDownloader(new string[] { tag }, downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateResourceDownloader(new string[] { tag }, downloadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建资源下载器，用于下载指定的资源标签列表关联的资源包文件
		/// </summary>
		/// <param name="tags">资源标签列表</param>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateResourceDownloader(string[] tags, int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateResourceDownloader(tags, downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateResourceDownloader(tags, downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateResourceDownloader(tags, downloadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建资源下载器，用于下载指定的资源依赖的资源包文件
		/// </summary>
		/// <param name="location">资源定位地址</param>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateBundleDownloader(string location, int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateBundleDownloader(location, downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateBundleDownloader(location, downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateBundleDownloader(location, downloadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建资源下载器，用于下载指定的资源列表依赖的资源包文件
		/// </summary>
		/// <param name="locations">资源定位地址列表</param>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateBundleDownloader(string[] locations, int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateBundleDownloader(locations, downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateBundleDownloader(locations, downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateBundleDownloader(locations, downloadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建资源下载器，用于下载指定的资源依赖的资源包文件
		/// </summary>
		/// <param name="assetInfo">资源信息</param>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateBundleDownloader(AssetInfo assetInfo, int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateBundleDownloader(assetInfo, downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateBundleDownloader(assetInfo, downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateBundleDownloader(assetInfo, downloadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建资源下载器，用于下载指定的资源列表依赖的资源包文件
		/// </summary>
		/// <param name="assetInfos">资源信息列表</param>
		/// <param name="downloadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public static ResourceDownloaderOperation CreateBundleDownloader(AssetInfo[] assetInfos, int downloadingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateBundleDownloader(assetInfos, downloadingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateBundleDownloader(assetInfos, downloadingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateBundleDownloader(assetInfos, downloadingMaxNumber, failedTryAgain);
		}
		#endregion

		#region 资源解压
		/// <summary>
		/// 创建内置资源解压器
		/// </summary>
		/// <param name="tag">资源标签</param>
		/// <param name="unpackingMaxNumber">同时解压的最大文件数</param>
		/// <param name="failedTryAgain">解压失败的重试次数</param>
		public static ResourceUnpackerOperation CreateResourceUnpacker(string tag, int unpackingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateResourceUnpacker(tag, unpackingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateResourceUnpacker(tag, unpackingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateResourceUnpacker(tag, unpackingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建内置资源解压器
		/// </summary>
		/// <param name="tags">资源标签列表</param>
		/// <param name="unpackingMaxNumber">同时解压的最大文件数</param>
		/// <param name="failedTryAgain">解压失败的重试次数</param>
		public static ResourceUnpackerOperation CreateResourceUnpacker(string[] tags, int unpackingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateResourceUnpacker(tags, unpackingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateResourceUnpacker(tags, unpackingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateResourceUnpacker(tags, unpackingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建内置资源解压器
		/// </summary>
		/// <param name="unpackingMaxNumber">同时解压的最大文件数</param>
		/// <param name="failedTryAgain">解压失败的重试次数</param>
		public static ResourceUnpackerOperation CreateResourceUnpacker(int unpackingMaxNumber, int failedTryAgain, bool useNewPackage = true)
		{
			DebugCheckDefaultPackageValid();
			if(_newPackage == null) {
				return _defaultPackage.CreateResourceUnpacker(unpackingMaxNumber, failedTryAgain);
			} 
			if (useNewPackage)
				return _newPackage.CreateResourceUnpacker(unpackingMaxNumber, failedTryAgain);
			return _defaultPackage.CreateResourceUnpacker(unpackingMaxNumber, failedTryAgain);
		}
		#endregion

		#region 调试方法
		[Conditional("DEBUG")]
		private static void DebugCheckDefaultPackageValid()
		{
			if (_defaultPackage == null)
				throw new Exception($"Default package is null. Please use {nameof(YooAssets.SetDefaultPackage)} !");
		}
		#endregion
	}
}