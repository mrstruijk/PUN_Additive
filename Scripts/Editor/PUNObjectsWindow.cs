using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;


public class PUNObjectsWindow : EditorWindow
{
    private const string prefabPath = "Assets/";

	private List<PUNType.PUNTypes> categories;
	private List<string> categoryLabels;
	private PUNType.PUNTypes selectedCategory;

	private List<PhotonView> photonViews;
	private Dictionary<PUNType.PUNTypes, List<PhotonView>> categorizedPhotonViews;

	private Dictionary<PhotonView, Texture2D> previews;

	private Vector2 scrollPosition;
	private readonly Vector2 previewSize = new Vector2(80, 90);

	public delegate void itemSelectedDelegate (PhotonView item,Texture2D preview);
	public static event itemSelectedDelegate ItemSelectedEvent;

	private static PUNObjectsWindow instance;


	[MenuItem("mrstruijk/PUN/PUN Objects _%p")] // % = cmd, # = shift, & = alt
	public static void OpenPUNObjectsWindow()
	{
		instance = (PUNObjectsWindow) GetWindow(typeof (PUNObjectsWindow));

		instance.titleContent = new GUIContent(nameof(PUNObjectsWindow));
	}


	private void OnEnable()
	{
		if (categories == null)
		{
			InitCategories();
		}

		if (categorizedPhotonViews == null)
		{
			InitContent();
		}
	}


	private void InitCategories()
	{
		categories = EditorUtils.GetListFromEnum<PUNType.PUNTypes>();
		categoryLabels = new List<string>();

		foreach (var category in categories)
		{
			categoryLabels.Add(category.ToString());
		}
	}


	private void InitContent()
	{
		photonViews = EditorUtils.GetAssetWithScript<PhotonView>(prefabPath);

		categorizedPhotonViews = new Dictionary<PUNType.PUNTypes, List<PhotonView>>();

		previews = new Dictionary<PhotonView, Texture2D>();

		foreach (var category in categories)
		{
			categorizedPhotonViews.Add(category, new List<PhotonView>());
		}

		foreach (var item in photonViews)
		{
			categorizedPhotonViews[PUNType.PUNTypes.Players].Add(item);
		}
	}


	private void OnGUI()
	{
		DrawTabs();
		DrawScroll();

		if (previews.Count != photonViews.Count) // This used to be in Update, but this seems fine as well.
		{
			GeneratePreviews();
		}
	}


	private void DrawTabs()
	{
		var index = (int) selectedCategory;
		index = GUILayout.Toolbar(index, categoryLabels.ToArray());
		selectedCategory = categories[index];
	}


	private void DrawScroll()
	{
		if (categorizedPhotonViews[selectedCategory].Count == 0)
		{
			EditorGUILayout.HelpBox("This category is empty!", MessageType.Info);
			return;
		}

		var rowCapacity = Mathf.FloorToInt(position.width / previewSize.x);

		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		var selectionGridIndex = -1;
		selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, GetGUIContentsFromItems(), rowCapacity, GetGUIStyle());
		GetSelectedItem(selectionGridIndex);
		GUILayout.EndScrollView();
	}


	private GUIContent[] GetGUIContentsFromItems()
	{
		var guiContents = new List<GUIContent>();

		if (previews.Count == photonViews.Count)
		{
			var totalItems = categorizedPhotonViews[selectedCategory].Count;

			for (var i = 0; i < totalItems; i++)
			{
				var guiContent = new GUIContent
				{
					text = categorizedPhotonViews[selectedCategory][i].name,
					image = previews[categorizedPhotonViews[selectedCategory][i]]
				};

				guiContents.Add(guiContent);
			}
		}

		return guiContents.ToArray();
	}


	private GUIStyle GetGUIStyle()
	{
		var guiStyle = new GUIStyle(GUI.skin.button)
		{
			alignment = TextAnchor.LowerCenter,
			imagePosition = ImagePosition.ImageAbove,
			fixedWidth = previewSize.x,
			fixedHeight = previewSize.y
		};

		return guiStyle;
	}


	private void GetSelectedItem(int index)
	{
		if (index != -1)
		{
			var selectedItem = categorizedPhotonViews[selectedCategory][index];

			ItemSelectedEvent?.Invoke (selectedItem, previews [selectedItem]);
		}
	}


	private void GeneratePreviews()
	{
		foreach (var item in photonViews)
		{
			if (!previews.ContainsKey(item))
			{
				var preview = AssetPreview.GetAssetPreview(item.gameObject);

				if (preview != null)
				{
					previews.Add(item, preview);
				}
			}
		}
	}
}
