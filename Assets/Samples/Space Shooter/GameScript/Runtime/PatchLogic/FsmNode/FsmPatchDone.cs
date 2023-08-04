using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Singleton;
using YooAsset;

/// <summary>
/// 流程更新完毕
/// </summary>
internal class FsmPatchDone : IStateNode
{
	void IStateNode.OnCreate(StateMachine machine)
	{
	}
	void IStateNode.OnEnter()
	{
		// PatchEventDefine.PatchStatesChange.SendEventMessage("开始游戏！");

		// // 创建游戏管理器
		// UniSingleton.CreateSingleton<GameManager>();

		// // 开启游戏流程
		// GameManager.Instance.Run();
		
		UnityEngine.Debug.LogError("流程更新完毕 =======================>");
 
		// PatchEventDefine.FoundUpdateFinish.SendEventMessage();
	 
		var package = YooAssets.TryGetPackage(YooAssets.NewPackage);
		package.IsReady = true;

		SceneEventDefine.ChangeToHomeScene.SendEventMessage();
	}
	void IStateNode.OnUpdate()
	{
	}
	void IStateNode.OnExit()
	{
	}
}