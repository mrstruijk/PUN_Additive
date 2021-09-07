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
        [SerializeField] [Range(0,5)] private int maxLoadedBaseScenes = 1;
        public int MaxLoadedBaseScenes => maxLoadedBaseScenes;

        public List<string> sceneNames;
        public string DefaultScene => sceneNames[1];

        public PhotonView photonView;

        public static Action<string> SceneHasBeenLoaded = delegate(string s) {  };


        private void Awake()
        {
            photonView = PhotonView.Get(this);
        }


        private void OnEnable()
        {
            GetSceneNameList();
        }


        public void GetSceneNameList()
        {
            var sceneNumber = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneNumber; i++)
            {
                sceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }


        public static string GetLatestLoadedSceneName()
        {
            return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name;
        }


        public static int GetLatestLoadedSceneIndex()
        {
            return SceneManager.GetSceneAt(SceneManager.sceneCount - 1).buildIndex;
        }


        public void RPCLoadSceneAddtively(string sceneName)
        {
            if (!PhotonConnectionSettingsSO.IsMaster)
            {
                return;
            }

            photonView.RPC("LoadMasterLocalScene", RpcTarget.All, sceneName);

            LoadClientLocalScene(sceneName);
        }


        [PunRPC] public void LoadMasterLocalScene(string sceneName)
        {
            StartCoroutine(LoadLocalSceneAsync(sceneName));
        }


        public void LoadClientLocalScene(string sceneName)
        {
            if (!PhotonConnectionSettingsSO.IsMaster)
            {
                StartCoroutine(LoadLocalSceneAsync(sceneName));
            }
        }


        private IEnumerator LoadLocalSceneAsync(string sceneName)
        {
            var sceneCount = SceneManager.sceneCount;
            var latestLoadedScene = SceneManager.GetSceneAt(sceneCount - 1);

            if (sceneName == latestLoadedScene.name)
            {
                yield break;
            }

            if (sceneCount <= maxLoadedBaseScenes)
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return SceneManager.UnloadSceneAsync(latestLoadedScene.name);
            }

            SceneHasBeenLoaded(sceneName);
        }


        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
