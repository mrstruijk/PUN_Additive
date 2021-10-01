using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using mrstruijk.SimpleHelpers;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace mrstruijk.SceneManagement
{
	[CreateAssetMenu(menuName = "mrstruijk/Scenes/SceneCollection", fileName = "SceneCollection_")]
	public class SceneCollection : ScriptableObject
	{
		public List<SceneReference> scenes;

		public List<string> SceneNames
		{
			get => GetSceneNames(scenes);
		}

		private static List<string> GetSceneNames(IEnumerable<SceneReference> inCollection)
		{
			return inCollection.Select(scene => ExtractFromString.ExtractedNameFromPath(scene.ScenePath)).ToList();
		}

		#if UNITY_EDITOR
		[ContextMenu("OpenScenesInEditor")]
		private void OpenScenesInEditor()
		{
			foreach (var scene in scenes)
			{
				if (!Application.isPlaying && Application.isEditor)
				{
					EditorSceneManager.OpenScene(scene, OpenSceneMode.Additive);
				}
			}
		}
		#endif
	}
}
