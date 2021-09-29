using System.Collections;
using JetBrains.Annotations;
using mrstruijk.Events;
using NUnit.Framework;
using Photon.Pun;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
// ReSharper disable ClassNeverInstantiated.Global
using System.Collections.Generic;
using mrstruijk.SceneManagement;

public class PlayPUNTests
{
    public class Network
    {
        private PhotonConnectionSettingsSO settings;
        //private EventSystem _eventSystem;

        private SceneManagement sceneManagement;
        private Scene lobby;




        [UnitySetUp] public IEnumerator PUNSetup()
        {
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);
            yield return settings = AssetDatabase.LoadAssetAtPath<PhotonConnectionSettingsSO>("Assets/_mrstruijk/Components/_Packages/PUN_Additive/ScriptableObjects/Photon Connection Settings.asset");

            Debug.Log(settings);

            yield return new WaitForSeconds(10);
        }


        [Test] public void Test()
        {
            Assert.AreEqual(1,1);
        }
/*
        [UnityTest] public IEnumerator CanChangeAutoSyncTest()
        {
            yield return new WaitForSeconds(1);

            settings.AutoSyncScenes = true;
            Assert.AreEqual(true, PhotonNetwork.AutomaticallySyncScene);

            settings.AutoSyncScenes = false;
            Assert.AreEqual(false, PhotonNetwork.AutomaticallySyncScene);
        }


        [UnityTest] public IEnumerator DisconnectedFromMasterAfterLoadingLevelTest()
        {
            yield return new WaitForSeconds(1);

            Assert.AreEqual(false, settings.IsConnecting);
        }


        [UnityTest] public IEnumerator JoinOrCreateRoomTest()
        {
            Assert.AreEqual(false, PhotonNetwork.IsConnected);

            settings.Connect();
            yield return new WaitForSeconds(1);

            Assert.AreEqual(true, PhotonNetwork.IsConnected);
        }


        [UnityTest] public IEnumerator DefaultSceneLoadedOnLeavingRoom()
        {
            yield return null;
            var startScene = smTestHelper.GetLatestLoadedSceneName();

            //sceneManagement.Load(sceneManagement.Lobby);
            //yield return smTestHelper.LoadScenes(smTestHelper.areaSceneCollection.SceneNames);
            yield return new WaitForSeconds(5);

            var newScene = smTestHelper.GetLatestLoadedSceneName();
            Debug.LogFormat("The newly loaded scene is {0}", newScene);
            Assert.AreNotEqual(startScene, newScene);
        }


*/

        [UnityTearDown] public IEnumerator PUNTearDown()
        {
            // PhotonNetwork.Disconnect();

            settings = null;

            yield return null;
        }
    }
}
