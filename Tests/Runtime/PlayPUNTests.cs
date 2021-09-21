using System.Collections;
using NUnit.Framework;
using Photon.Pun;
using UnityEngine;
using UnityEngine.TestTools;
// ReSharper disable ClassNeverInstantiated.Global
using mrstruijk.SceneManagement;

public class PlayPUNTests
{
    public class Network
    {
        private PhotonConnectionSettingsSO settings;
        private SceneManagement sceneManagement;





        [UnitySetUp] public IEnumerator PUNSetup()
        {
            settings = ScriptableObject.CreateInstance<PhotonConnectionSettingsSO>();
            // iConnectToNetworkHost = settings;


            var go = new GameObject();
            sceneManagement = go.AddComponent<SceneManagement>();


            yield return null;
        }


        [UnityTest] public IEnumerator AutoSyncTest()
        {
            // settings.AutoSyncScenesAcrossClients();
            yield return new WaitForSeconds(1);

            Assert.AreEqual(true, PhotonNetwork.AutomaticallySyncScene);

        }


        [UnityTest] public IEnumerator DisconnectedFromMasterAfterLoadingLevelTest()
        {
            // iConnectToNetworkHost.ConnectedToMaster();
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
            var startScene = SceneManagement.GetLatestLoadedSceneName();

            // sceneManagement.LoadNetworkedScene(sceneManagement.Lobby);
            yield return null;

            var newScene = SceneManagement.GetLatestLoadedSceneName();
            Debug.LogFormat("The newly loaded scene is {0}", newScene);
            Assert.AreNotEqual(startScene, newScene);
        }



        [UnityTearDown] public IEnumerator PUNTearDown()
        {
            PhotonNetwork.Disconnect();

            settings = null;

            yield return null;
        }
    }
}
