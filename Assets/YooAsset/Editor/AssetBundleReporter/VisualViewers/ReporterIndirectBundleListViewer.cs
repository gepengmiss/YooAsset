#if UNITY_2019_4_OR_NEWER
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace YooAsset.Editor
{
	internal class ReporterIndirectBundleListViewer 
	{ 
		protected enum ESortMode
		{
			BundleName,
			BundleSize,
			BundleTags
		}

		protected VisualTreeAsset _visualAsset;
		protected TemplateContainer _root;

		protected ToolbarButton _topBar1;
		protected ToolbarButton _topBar2;
		protected ToolbarButton _topBar3;
		protected ToolbarButton _topBar5;
		protected ToolbarButton _bottomBar1;
		protected ListView _bundleListView;
		protected ListView _includeListView;

		protected BuildReport _buildReport; 
		protected string _searchKeyWord;
		protected ESortMode _sortMode = ESortMode.BundleName;
		protected bool _descendingSort = false;

		/// <summary>
		/// 初始化页面
		/// </summary>
		public void InitViewer()
		{
			// 加载布局文件
			_visualAsset = UxmlLoader.LoadWindowUXML<ReporterIndirectBundleListViewer>();
			if (_visualAsset == null)
				return;

			_root = _visualAsset.CloneTree();
			_root.style.flexGrow = 1f;

			// 顶部按钮栏
			_topBar1 = _root.Q<ToolbarButton>("TopBar1");
			_topBar2 = _root.Q<ToolbarButton>("TopBar2");
			_topBar3 = _root.Q<ToolbarButton>("TopBar3");
			_topBar5 = _root.Q<ToolbarButton>("TopBar5");
			_topBar1.clicked += TopBar1_clicked;
			_topBar2.clicked += TopBar2_clicked;
			_topBar3.clicked += TopBar3_clicked;
			_topBar5.clicked += TopBar4_clicked;

			// 底部按钮栏
			_bottomBar1 = _root.Q<ToolbarButton>("BottomBar1");

			// 资源包列表
			_bundleListView = _root.Q<ListView>("TopListView");
			_bundleListView.makeItem = MakeBundleListViewItem;
			_bundleListView.bindItem = BindBundleListViewItem;
#if UNITY_2020_1_OR_NEWER
			_bundleListView.onSelectionChange += BundleListView_onSelectionChange;
#else
			_bundleListView.onSelectionChanged += BundleListView_onSelectionChange;
#endif

			// 包含列表
			_includeListView = _root.Q<ListView>("BottomListView");
			_includeListView.makeItem = MakeIncludeListViewItem;
			_includeListView.bindItem = BindIncludeListViewItem;

#if UNITY_2020_3_OR_NEWER
			SplitView.Adjuster(_root);
#endif
		}

		/// <summary>
		/// 填充页面数据
		/// </summary>
		public void FillViewData(BuildReport buildReport, string searchKeyWord)
		{
			_buildReport = buildReport; 
			_searchKeyWord = searchKeyWord;
			RefreshView();
		}
		private void RefreshView()
		{
			_bundleListView.Clear();
			_bundleListView.ClearSelection();
			_bundleListView.itemsSource = FilterAndSortViewItems();
			_bundleListView.Rebuild();
			RefreshSortingSymbol();
		}
		private void RefreshSortingSymbol()
		{
			// 刷新符号
			_topBar1.text = $"Bundle Name ({_bundleListView.itemsSource.Count})";
			_topBar2.text = "Size";
			_topBar3.text = "Hash";
			_topBar5.text = "Tags";

			if (_sortMode == ESortMode.BundleName)
			{
				if (_descendingSort)
					_topBar1.text = $"Bundle Name ({_bundleListView.itemsSource.Count}) ↓";
				else
					_topBar1.text = $"Bundle Name ({_bundleListView.itemsSource.Count}) ↑";
			}
			else if (_sortMode == ESortMode.BundleSize)
			{
				if (_descendingSort)
					_topBar2.text = "Size ↓";
				else
					_topBar2.text = "Size ↑";
			}
			else if (_sortMode == ESortMode.BundleTags)
			{
				if (_descendingSort)
					_topBar5.text = "Tags ↓";
				else
					_topBar5.text = "Tags ↑";
			}
			else
			{
				throw new System.NotImplementedException();
			}
		}

		/// <summary>
		/// 挂接到父类页面上
		/// </summary>
		public void AttachParent(VisualElement parent)
		{
			parent.Add(_root);
		}

		/// <summary>
		/// 从父类页面脱离开
		/// </summary>
		public void DetachParent()
		{
			_root.RemoveFromHierarchy();
		}
		// 顶部列表相关
		private VisualElement MakeBundleListViewItem()
		{
			VisualElement element = new VisualElement();
			element.style.flexDirection = FlexDirection.Row;

			{
				var label = new Label();
				label.name = "Label1";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				label.style.flexGrow = 1f;
				label.style.width = 280;
				element.Add(label);
			}

			{
				var label = new Label();
				label.name = "Label2";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				//label.style.flexGrow = 1f;
				label.style.width = 100;
				element.Add(label);
			}

			{
				var label = new Label();
				label.name = "Label3";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				//label.style.flexGrow = 1f;
				label.style.width = 280;
				element.Add(label);
			}

			{
				var label = new Label();
				label.name = "Label5";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				//label.style.flexGrow = 1f;
				label.style.width = 150;
				element.Add(label);
			}

			{
				var label = new Label();
				label.name = "Label6";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				label.style.flexGrow = 1f;
				label.style.width = 80;
				element.Add(label);
			}

			return element;
		}
		private void TopBar1_clicked()
		{
			if (_sortMode != ESortMode.BundleName)
			{
				_sortMode = ESortMode.BundleName;
				_descendingSort = false;
				RefreshView();
			}
			else
			{
				_descendingSort = !_descendingSort;
				RefreshView();
			}
		}
		private void TopBar2_clicked()
		{
			if (_sortMode != ESortMode.BundleSize)
			{
				_sortMode = ESortMode.BundleSize;
				_descendingSort = false;
				RefreshView();
			}
			else
			{
				_descendingSort = !_descendingSort;
				RefreshView();
			}
		}
		private void TopBar3_clicked()
		{
		}
		private void TopBar4_clicked()
		{
			if (_sortMode != ESortMode.BundleTags)
			{
				_sortMode = ESortMode.BundleTags;
				_descendingSort = false;
				RefreshView();
			}
			else
			{
				_descendingSort = !_descendingSort;
				RefreshView();
			}
		}

		private List<ReportBundleInfo> FilterAndSortViewItems()
		{
			List<ReportBundleInfo> result = new List<ReportBundleInfo>(_buildReport.IndrectBundleInfos.Count);

			// 过滤列表
			foreach (var bundleInfo in _buildReport.IndrectBundleInfos)
			{
				if (string.IsNullOrEmpty(_searchKeyWord) == false)
				{
					if (bundleInfo.BundleName.Contains(_searchKeyWord) == false)
						continue;
				}
				result.Add(bundleInfo);
			}

			// 排序列表
			if (_sortMode == ESortMode.BundleName)
			{
				if (_descendingSort)
					return result.OrderByDescending(a => a.BundleName).ToList();
				else
					return result.OrderBy(a => a.BundleName).ToList();
			}
			else if (_sortMode == ESortMode.BundleSize)
			{
				if (_descendingSort)
					return result.OrderByDescending(a => a.FileSize).ToList();
				else
					return result.OrderBy(a => a.FileSize).ToList();
			}
			else if (_sortMode == ESortMode.BundleTags)
			{
				if (_descendingSort)
					return result.OrderByDescending(a => a.GetTagsString()).ToList();
				else
					return result.OrderBy(a => a.GetTagsString()).ToList();
			}
			else
			{
				throw new System.NotImplementedException();
			}
		}
		private void BindBundleListViewItem(VisualElement element, int index)
		{
			var sourceData = _bundleListView.itemsSource as List<ReportBundleInfo>;
			var bundleInfo = sourceData[index];

			// Bundle Name
			var label1 = element.Q<Label>("Label1");
			label1.text = bundleInfo.BundleName;

			// Size
			var label2 = element.Q<Label>("Label2");
			label2.text = EditorUtility.FormatBytes(bundleInfo.FileSize);

			// Hash
			var label3 = element.Q<Label>("Label3");
			label3.text = bundleInfo.FileHash;

			// LoadMethod
			var label5 = element.Q<Label>("Label5");
			label5.text = bundleInfo.LoadMethod.ToString();

			// Tags
			var label6 = element.Q<Label>("Label6");
			label6.text = "--";
		}
		private void BundleListView_onSelectionChange(IEnumerable<object> objs)
		{
			foreach (var item in objs)
			{
				ReportBundleInfo bundleInfo = item as ReportBundleInfo;
				FillIncludeListView(bundleInfo); 
				break;
			}
		}
	 
		// 底部列表相关
		private void FillIncludeListView(ReportBundleInfo bundleInfo)
		{ 
			_includeListView.Clear();
			_includeListView.ClearSelection();
			_includeListView.itemsSource = bundleInfo.AllBuiltinAssets;
			_includeListView.Rebuild();
			_bottomBar1.text = $"Include Assets ({bundleInfo.AllBuiltinAssets.Count})";
		}

		private void BindIncludeListViewItem(VisualElement element, int index)
		{
			List<string> containsList = _includeListView.itemsSource as List<string>;
			string assetName = containsList[index];

			// Asset Path
			var label1 = element.Q<Label>("Label1");
			label1.text = assetName;

			// GUID
			var label2 = element.Q<Label>("Label2");
			label2.text = "--";
		}
		private VisualElement MakeIncludeListViewItem()
		{
			VisualElement element = new VisualElement();
			element.style.flexDirection = FlexDirection.Row;

			{
				var label = new Label();
				label.name = "Label1";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				label.style.flexGrow = 1f;
				label.style.width = 280;
				element.Add(label);
			}

			{
				var label = new Label();
				label.name = "Label2";
				label.style.unityTextAlign = TextAnchor.MiddleLeft;
				label.style.marginLeft = 3f;
				//label.style.flexGrow = 1f;
				label.style.width = 280;
				element.Add(label);
			}

			return element;
		}
	} 
}
#endif