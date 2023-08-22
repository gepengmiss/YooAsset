using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using ShaderVariant = UnityEngine.ShaderVariantCollection.ShaderVariant;
using Rendering = UnityEngine.Rendering;
using System.IO;

namespace YooAsset.Editor
{
	public class BuildRunner
	{
		private static Stopwatch _buildWatch;

		/// <summary>
		/// 总耗时
		/// </summary>
		public static int TotalSeconds = 0;
	
		/// <summary>
		/// 执行构建流程
		/// </summary>
		/// <returns>如果成功返回TRUE，否则返回FALSE</returns>
		public static BuildResult Run(List<IBuildTask> pipeline, BuildContext context)
		{ 
			if (pipeline == null)
				throw new ArgumentNullException("pipeline");
			if (context == null)
				throw new ArgumentNullException("context");

			BuildResult buildResult = new BuildResult();
			buildResult.Success = true;
			TotalSeconds = 0;
			for (int i = 0; i < pipeline.Count; i++)
			{
				IBuildTask task = pipeline[i];
				try
				{
					_buildWatch = Stopwatch.StartNew();
					var taskAttribute = task.GetType().GetCustomAttribute<TaskAttribute>();
					if (taskAttribute != null)
						BuildLogger.Log($"---------------------------------------->{taskAttribute.TaskDesc}<---------------------------------------");
					task.Run(context);
					_buildWatch.Stop();

					// 统计耗时
					int seconds = GetBuildSeconds();
					TotalSeconds += seconds;
					if (taskAttribute != null)
						BuildLogger.Log($"{taskAttribute.TaskDesc}耗时：{seconds}秒");
				}
				catch (Exception e)
				{
					EditorTools.ClearProgressBar();
					buildResult.FailedTask = task.GetType().Name;
					buildResult.ErrorInfo = e.ToString();
					buildResult.Success = false;
					break;
				}
			}

			// 返回运行结果
			BuildLogger.Log($"构建过程总计耗时：{TotalSeconds}秒");

			Print();

			return buildResult;
		}

		private static void Print()
		{
			System.Text.StringBuilder sb = Soco.ShaderVariantsStripper.ShaderVariantsStripperCode.sb;  
			SVC.FileHelper.WriteToFile(sb, "Temp/OnProcessShader.txt");
			
			System.Text.StringBuilder sbCache = Soco.ShaderVariantsStripper.ShaderVariantsStripperCode.sbCache;  
			SVC.FileHelper.WriteToFile(sbCache, "Temp/sbCache.txt", false);

			ShaderVariantCollection svc = new ShaderVariantCollection();
			using (System.IO.StringReader reader = new System.IO.StringReader(sbCache.ToString()))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					UnityEngine.Debug.Log(line);

					string shaderName = line.Trim(); 
					Shader mshader = Shader.Find(shaderName);
					if(mshader == null)
					{
						UnityEngine.Debug.LogError("没有找到shader: " + shaderName);
					} else {

					}

					line = reader.ReadLine();
					int count = int.Parse(line.Trim()); 

					line = reader.ReadLine();
					string mpassType = line.Trim();  
					Rendering.PassType mpass = (Rendering.PassType) Enum.Parse(typeof(Rendering.PassType), mpassType);

					for(int i = 0, iMax = count; i < iMax; i++)
					{ 
						line = reader.ReadLine();
						string keysLine = line.Trim();
						string[] keyNames = keysLine.Split(' ');
						ShaderVariant _sv = new ShaderVariant(mshader, mpass, keyNames);
						svc.Add(_sv);
					}
					 	
					line = reader.ReadLine(); // 跳过分割行 
				}
				    var tempPath = "Assets/TestSVC/Tool2/UnityBuildResultSVC.shadervariants";
				if ( File.Exists( tempPath ) ) {
				    UnityEditor.AssetDatabase.DeleteAsset( tempPath );
				}  
				UnityEngine.Debug.LogError("--------- Save" + svc);
				UnityEditor.AssetDatabase.CreateAsset(svc, tempPath);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh(); 
			}
		}

		private static int GetBuildSeconds()
		{
			float seconds = _buildWatch.ElapsedMilliseconds / 1000f;
			return (int)seconds;
		}
	}
}