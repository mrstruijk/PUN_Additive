using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable MemberCanBeMadeStatic.Global


namespace mrstruijk
{
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField] [Range(0,10)] private int highestBaseSceneIndex = 4;
        public int HighestBaseSceneIndex => highestBaseSceneIndex;

        public List<string> sceneNames;
        public string StartScene = "1. Standard";

        public PhotonView photonView;

        public static Action<string> SceneHasBeenLoaded = delegate(string s) {  };
        public static Action BaseScenesHaveBeenLoaded = delegate { };

        private void Awake()
        {
            if (!photonView)
            {
                photonView = PhotonView.Get(this);
            }
        }


        private void OnEnable()
        {
            GetSceneNameList();
            LoadBaseScenes();
        }


        public void GetSceneNameList()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }


        private void LoadBaseScenes()
        {
            for (int i = 1; i < highestBaseSceneIndex; i++)
            {
                RPCLoadSceneAddtively(sceneNames[i]);
            }
        }


        public static string GetLatestLoadedSceneName()
        {
            return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name;
        }


        public int GetLatestLoadedSceneIndex()
        {
            return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).buildIndex;
        }


        // TODO: Check this method and ones below. Why double call to !IsMaster? Is that second method even called at all?
        public void RPCLoadSceneAddtively(string sceneName)
        {
            if (!PhotonConnectionSettingsSO.IsMaster)
            {
                return;
            }

            photonView.RPC("LoadMasterLocalScene", RpcTarget.All, sceneName);

            LoadSceneLocally(sceneName);
        }


        [PunRPC] public void LoadMasterLocalScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }


        public void LoadSceneLocally(string sceneName)
        {
            if (!PhotonConnectionSettingsSO.IsMaster)
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            }
        }


        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var sceneCount = SceneManager.sceneCount;
            var latestLoadedScene = SceneManager.GetSceneAt(sceneCount - 1);

            if (sceneName == latestLoadedScene.name)
            {
                yield break;
            }

            if (sceneCount <= highestBaseSceneIndex)
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return SceneManager.UnloadSceneAsync(latestLoadedScene.name);
            }

            SceneHasBeenLoaded(sceneName);

            if (sceneName == sceneNames[HighestBaseSceneIndex])
            {
                BaseScenesHaveBeenLoaded.Invoke();
            }
        }


        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
