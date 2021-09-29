using System.Collections;
using System.Collections.Generic;
using mrstruijk.Events;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace mrstruijk.SceneManagement
{
	public class SceneManagement : MonoBehaviour
	{
		public SceneCollection baseSceneCollection;

		public SceneCollection areaSceneCollection;

		public PhotonView PhotonView
		{
			get;
			private set;
		}

		private Coroutine loadingScene;


		private void Awake()
		{
			GetPhotonView();
		}


		private void GetPhotonView()
		{
			if (!PhotonView)
			{
				PhotonView = PhotonView.Get(this);

				if (!PhotonView)
				{
					PhotonView = gameObject.AddComponent<PhotonView>();
				}
			}
		}


		private void OnEnable()
		{
			StartCoroutine(StartBaseAndStartingArea());
		}


		private IEnumerator StartBaseAndStartingArea()
		{
			yield return StartCoroutine(LoadScenes(baseSceneCollection.SceneNames));
			yield return StartCoroutine(LoadScenes(new List<string> {areaSceneCollection.SceneNames[0]}));
		}


		public IEnumerator LoadScenes(List<string> scenesToLoad)
		{
			for (var i = 0; i < scenesToLoad.Count; i++)
			{
				yield return loadingScene == null;
				RPCLoadSceneAddtively(scenesToLoad[i]);
			}
		}


		public void RPCLoadSceneAddtively(string sceneName)
		{
			if (PhotonConnectionSettingsSO.IsMaster)
			{
				PhotonView.RPC("LoadScene", RpcTarget.All, sceneName);
			}
			else if (!PhotonConnectionSettingsSO.IsMaster)
			{
				LoadScene(sceneName);
			}
		}


		[PunRPC]
		private void LoadScene(string sceneName)
		{
			loadingScene = StartCoroutine(LoadSceneAsync(sceneName));
		}


		private IEnumerator LoadSceneAsync(string sceneName)
		{
			var loadedSceneList = new List<string>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				loadedSceneList.Add(SceneManager.GetSceneAt(i).name);
			}

			foreach (var areaSceneName in areaSceneCollection.SceneNames)
			{
				if (loadedSceneList.Contains(areaSceneName) && areaSceneName != sceneName)
				{
					yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
					yield return SceneManager.UnloadSceneAsync(areaSceneName);
					SendSceneLoadEvents(sceneName);
					SetActiveScene();
					yield break;
				}
			}

			if (!loadedSceneList.Contains(sceneName))
			{
				yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

				SendSceneLoadEvents(sceneName);
				SetActiveScene();
			}
			else
			{
				if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
				{
					Debug.LogFormat("Scene {0} was already loaded at time: {1}", sceneName, Time.time);
				}
			}
		}


		private void SendSceneLoadEvents(string sceneName)
		{
			if (sceneName == baseSceneCollection.SceneNames[baseSceneCollection.SceneNames.Count - 1])
			{
				EventSystem.BaseScenesHaveBeenLoaded?.Invoke();
			}
			else if (areaSceneCollection.SceneNames.Contains(sceneName))
			{
				EventSystem.AreaSceneHasBeenLoaded?.Invoke(sceneName);
			}
		}


		private void SetActiveScene()
		{
			var scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

			if (scene.isLoaded)
			{
				if (areaSceneCollection.SceneNames.Contains(scene.name))
				{
					SceneManager.SetActiveScene(scene);
				}
			}
		}


		public string GetLatestLoadedSceneName()
		{
			return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name;
		}


		public int GetLatestLoadedSceneIndex()
		{
			return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).buildIndex;
		}


		public static void MoveToActiveScene(GameObject gameObject, string sceneName)
		{
			var scene = SceneManager.GetSceneByName(sceneName);
			if (scene.isLoaded)
			{
				SceneManager.MoveGameObjectToScene(gameObject, scene);
			}
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}
	}
}
