using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using System.Text;
using ShaderVariant = UnityEngine.ShaderVariantCollection.ShaderVariant;

namespace SVC
{
#if UNITY_EDITOR
    public class ParseShaderCombinationWindow : EditorWindow
    { 
        private static int cBorderWidth = 10; 
        private static int cLeftWidth = 300; 
        private static Vector2 cMinWindowSize = new Vector2(1200, 600);

        public string[] mIncludePath = { "Assets"
        // , "Packages" 
        };
        // 当前shader变体信息缓存，忽略代码执行过程中对资源进行的任何修改
        Dictionary<Shader, ShaderParsedCombinations> shaderCombinations = new Dictionary<Shader, ShaderParsedCombinations>();
 
        [MenuItem("ShaderVC2/Window", priority = 200)]
        public static void OpenWindow() {
            Window.Show();
        }

        public void OnGUI()  {
            EditorGUILayout.Space(cBorderWidth);
            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
 
            if (GUILayout.Button("分析shader keyword", GUILayout.Width(cLeftWidth))) {
                Parse();
            }
              
            GUILayout.EndHorizontal();
        }

        private void Parse() { 
            shaderCombinations.Clear();
            var guids = AssetDatabase.FindAssets("t:Shader", mIncludePath);
            foreach (Shader s in guids.Select(guid => AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath(guid)))) {
                CollectCombinations(s);
            }
            Analyse();
        }
 
        private void Analyse() {
            int index = 0;
            var sb = new StringBuilder();
            foreach(var kv in shaderCombinations) {
                index++; 
                sb.AppendLine("序号：" + index + " shaderName: " + kv.Key.name);
                var combs = kv.Value as ShaderParsedCombinations; 
                sb.AppendLine(combs.Dump());
            }
            WriteToFile(sb);
            SaveSVC();
        }
         
        private void WriteToFile(StringBuilder sb) {  
            string filePath = "Temp/ParsedCombinations.txt";      
            SVC.FileHelper.WriteToFile(sb, filePath);
        }

        // 如果不经过过滤，默认会获取所有compile和feature的组合，然而unity编译资源时并不会用到这么多key。
        private void SaveSVC()
        { 
            var svc = new ShaderVariantCollection();
            try { 
                int index = 0;
                foreach(var kv in shaderCombinations) {
                    index++; 
                    Debug.Log("序号：" + index + " shaderName: " + kv.Key.name);

                    var combs = kv.Value as ShaderParsedCombinations;  
                    var mShader = combs.shader; 
                    var dict = ShaderVariantCollectionHelper.GetShaderVariantEntries(mShader);
                    foreach(var kv2 in dict) {
                        List<ShaderVariant> variants = kv2.Value;
                        foreach(var _sv in variants) {
                            svc.Add(_sv);
                        } 
                    } 
                } 
            } finally { 
                var tempPath = "Assets/TestSVC/Tool2/AllShaderVariants2.shadervariants";
                if ( File.Exists( tempPath ) ) {
                    AssetDatabase.DeleteAsset( tempPath );
                }  
                AssetDatabase.CreateAsset(svc, tempPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(); 
            } 
        }

        private void CollectCombinations(Shader shader) {
            if ( shader != null ) {
                string shaderName = shader.name;
                if( 
                    shaderName.StartsWith( "Hidden/" ) 
                    || shaderName.StartsWith( "Universal Render Pipeline/" ) 
                    || shaderName.StartsWith( "Legacy Shaders/" ) 
                    || shaderName.StartsWith( "Particles/" ) 
                    || shaderName.StartsWith( "Shader Graphs/" ) 
                    || shaderName.StartsWith( "Mobile/" ) 
                )
			        return;

                if ( !shaderCombinations.ContainsKey( shader ) ) {
                    var info = ShaderUtils.ParseShaderCombinations( shader, true, false );
                    if ( info != null ) {
                        shaderCombinations.Add( shader, info );
                    }
                }
            }
        }
 
        public void Awake() {
            
        }

        public void OnDisable() { 
            
        }

        private static ParseShaderCombinationWindow m_window;
        private static ParseShaderCombinationWindow Window {
            get {
                if (m_window == null) {
                    m_window = EditorWindow.GetWindow<ParseShaderCombinationWindow>("ParseShaderCombinationWindow");
                    m_window.minSize = cMinWindowSize;
                }
                return m_window;
            }
        }

    }
#endif
}