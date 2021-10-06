using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PUNObjectsWindow : EditorWindow
{
	private const string prefabPath = "Assets/";
	//"Assets/_mrstruijk/Components/_SceneManagement/Prefabs/Resources";

	private List<PUNItem.PUNType> punTypes;
	private List<string> typeLabels;
	private PUNItem.PUNType selectedType;

	private List<PUNItem> punItems;
	private Dictionary<PUNItem.PUNType, List<PUNItem>> categorizedPUNItems;

	private Dictionary<PUNItem, Texture2D> previews;

	private Vector2 scrollPosition;
	private readonly Vector2 previewSize = new Vector2(80, 90);

	// public delegate void itemSelectedDelegate (PhotonView item,Texture2D preview);
	// public static event itemSelectedDelegate ItemSelectedEvent;

	private static PUNObjectsWindow instance;


	[MenuItem("mrstruijk/PUN/PUN Objects _%p")] // % = cmd, # = shift, & = alt
	public static void OpenPUNObjectsWindow()
	{
		instance = (PUNObjectsWindow) GetWindow(typeof (PUNObjectsWindow));

		instance.titleContent = new GUIContent(nameof(PUNObjectsWindow));
	}


	private void OnEnable()
	{
		if (punTypes == null)
		{
			InitCategories();
		}

		if (categorizedPUNItems == null)
		{
			InitContent();
		}
	}


	private void InitCategories()
	{
		punTypes = EditorUtils.GetListFromEnum<PUNItem.PUNType>();
		typeLabels = new List<string>();

		foreach (var type in punTypes)
		{
			typeLabels.Add(type.ToString());
		}
	}


	private void InitContent()
	{
		punItems = EditorUtils.GetAssetWithScript<PUNItem>(prefabPath);

		categorizedPUNItems = new Dictionary<PUNItem.PUNType, List<PUNItem>>();

		previews = new Dictionary<PUNItem, Texture2D>();

		foreach (var punType in punTypes)
		{
			categorizedPUNItems.Add(punType, new List<PUNItem>());
		}

		foreach (var item in punItems)
		{
			categorizedPUNItems[item.punType].Add(item);
		}
	}


	private void OnGUI()
	{
		DrawTabs();
		DrawScroll();
	}

	private void Update()
	{
		if (previews.Count != punItems.Count) // This used to be in Update, but this seems fine as well.
		{
			GeneratePreviews();
		}
	}


	private void DrawTabs()
	{
		var index = (int) selectedType;
		index = GUILayout.Toolbar(index, typeLabels.ToArray());
		selectedType = punTypes[index];
	}


	private void DrawScroll()
	{
		if (categorizedPUNItems[selectedType].Count == 0)
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

		if (previews.Count == punItems.Count)
		{
			var totalItems = categorizedPUNItems[selectedType].Count;

			for (var i = 0; i < totalItems; i++)
			{
				var guiContent = new GUIContent
				{
					text = categorizedPUNItems[selectedType][i].itemName,
					image = previews[categorizedPUNItems[selectedType][i]]
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
			var selectedItem = categorizedPUNItems[selectedType][index];

			// ItemSelectedEvent?.Invoke (selectedItem, previews [selectedItem]);
		}
	}


	private void GeneratePreviews()
	{
		foreach (var item in punItems)
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
