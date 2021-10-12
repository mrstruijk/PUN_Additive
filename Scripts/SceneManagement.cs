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
		private string[] baseArray;
		public SceneCollection areaSceneCollection;
		private string[] areaArray;

		public List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

		private LoadingScreen loadingScreen;

		public PhotonView PhotonView
		{
			get;
			private set;
		}

		private Coroutine sceneIsLoading;


		private void Awake()
		{
			GetPhotonView();
			loadingScreen = FindObjectOfType<LoadingScreen>();

			baseArray = new string[baseSceneCollection.SceneNames.Count];

			for (var i = 0; i < baseSceneCollection.SceneNames.Count; i++)
			{
				baseArray[i] = baseSceneCollection.SceneNames[i];
			}

			areaArray = new string[] {areaSceneCollection.SceneNames[0]};
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
			RPCLoadSceneAddtively(baseArray);

			RPCLoadSceneAddtively(areaArray);
		}


		public void RPCLoadSceneAddtively(string[] sceneNames)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				if (sceneNames.Length > 1)
				{
					PhotonView.RPC("LoadScenesAsync", RpcTarget.All, sceneNames);
				}
				else if (sceneNames.Length == 1)
				{
					PhotonView.RPC("LoadSceneAsync", RpcTarget.All, sceneNames[0]);
				}
			}
			else if (!PhotonNetwork.IsMasterClient)
			{
				if (sceneNames.Length > 1)
				{
					LoadScenesAsync(sceneNames);
				}
				else if (sceneNames.Length == 1)
				{
					LoadSceneAsync(sceneNames[0]);
				}
			}
		}


		[PunRPC] private void LoadScenesAsync(string[] sceneNames)
		{
			var loadedSceneList = new List<string>();

			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				loadedSceneList.Add(SceneManager.GetSceneAt(i).name);
			}

			foreach (var scene in sceneNames)
			{
				if (loadedSceneList.Contains(scene))
				{
					// scenesLoading.Add(SceneManager.UnloadSceneAsync(scene));

					if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
					{
						Debug.LogFormat("Scene {0} was already loaded at time: {1}", scene, Time.time);
					}

					continue;
				}

				scenesLoading.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
				SendSceneLoadEvents(scene);
				SetActiveScene();
			}


			StartCoroutine(GetSceneLoadProgress());
		}


		[PunRPC] private void LoadSceneAsync(string sceneName)
		{
			var loadedSceneList = new List<string>();

			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				loadedSceneList.Add(SceneManager.GetSceneAt(i).name);
			}

			if (loadedSceneList.Contains(sceneName))
			{
				// scenesLoading.Add(SceneManager.UnloadSceneAsync(scene));

				if (PhotonNetwork.LogLevel >= PunLogLevel.Full)
				{
					Debug.LogFormat("Scene {0} was already loaded at time: {1}", sceneName, Time.time);
				}

				return;
			}

			scenesLoading.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
			SendSceneLoadEvents(sceneName);
			SetActiveScene();


			StartCoroutine(GetSceneLoadProgress());
		}


		private IEnumerator GetSceneLoadProgress()
		{
			for (var i = 0; i < scenesLoading.Count; i++)
			{
				while (!scenesLoading[i].isDone)
				{
					yield return null;
				}
			}

			yield return new WaitForSeconds(3f);
			SceneManager.UnloadSceneAsync(loadingScreen.loadingScreen.SceneNames[0]);
		}


		private void SendSceneLoadEvents(string sceneName)
		{
			EventSystem.SceneHasBeenLoaded?.Invoke(sceneName);

			if (areaSceneCollection.SceneNames.Contains(sceneName))
			{
				EventSystem.BaseScenesHaveBeenLoaded?.Invoke();
			}
		}


		private void SetActiveScene()
		{
			var scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

			if (scene.isLoaded)
			{
				SceneManager.SetActiveScene(scene);
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
