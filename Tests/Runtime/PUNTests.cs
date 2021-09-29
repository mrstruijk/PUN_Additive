using System.Collections;
using mrstruijk.PUN;
using mrstruijk.SceneManagement;
using NUnit.Framework;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


public class PUNTests
{
	private const string lobbySceneName = "Lobby";
	private const string networkHostObjectName = "NetworkHost";
	private const string sceneManagementManagerObjectName = "SceneManagementManager";
	private const string userAvatarManagerObjectName = "UserAvatarManager";


	public class Lobby
	{
		private SceneManagement sceneManagement;


		[UnitySetUp] public IEnumerator Setup()
		{
			yield return SceneManager.LoadSceneAsync(lobbySceneName, LoadSceneMode.Single);
		}


		[Test] public void ContainsNetworkHost()
		{
			var hostGameObject = GameObject.Find(networkHostObjectName);
			Assert.IsNotNull(hostGameObject);

			var host = hostGameObject.GetComponent<NetworkHostConnector>();
			Assert.IsNotNull(host);
		}


		[Test] public void NetworkHostHasConnectionSettings()
		{
			var hostGameObject = GameObject.Find(networkHostObjectName);
			var host = hostGameObject.GetComponent<NetworkHostConnector>();

			Assert.IsNotNull(host.connectionSettings);
		}


		[Test] public void ContainsSceneManagementManager()
		{
			var sceneManagementManagerGameObject = GameObject.Find(sceneManagementManagerObjectName);
			Assert.IsNotNull(sceneManagementManagerGameObject);

			var sceneManagementManager = sceneManagementManagerGameObject.GetComponent<SceneManagementManager>();
			Assert.IsNotNull(sceneManagementManager);
		}


		[Test] public void ContainsUserAvatarManager()
		{
			var avatarManagerGameObject = GameObject.Find(userAvatarManagerObjectName);
			Assert.IsNotNull(avatarManagerGameObject);

			var avatarManager = avatarManagerGameObject.GetComponent<UserAvatarManager>();
			Assert.IsNotNull(avatarManager);
		}


		[Test] public void DoesNotContainNonsense()
		{
			var nonsenseGameObject = GameObject.Find("Nonsense");
			Assert.IsNull(nonsenseGameObject);
		}


		[UnityTearDown] public IEnumerator TearDown()
		{
			yield return null;
		}
	}


	public class Photon
	{
		[UnitySetUp] public IEnumerator Setup()
		{
			yield return SceneManager.LoadSceneAsync(lobbySceneName, LoadSceneMode.Single);
		}


		[Test] public void ICanChangeAutoSyncMode()
		{
			var host = Object.FindObjectOfType<NetworkHostConnector>();
			var connectionSettings = host.connectionSettings;

			connectionSettings.AutoSyncScenes = true;
			Assert.AreEqual(true, PhotonNetwork.AutomaticallySyncScene);

			connectionSettings.AutoSyncScenes = false;
			Assert.AreEqual(false, PhotonNetwork.AutomaticallySyncScene);
		}


		[UnityTest] public IEnumerator NetworkIsConnected()
		{
			yield return new WaitForSeconds(1);

			Assert.AreEqual(true, PhotonNetwork.IsConnected);
		}


		[UnityTearDown] public IEnumerator TearDown()
		{
			yield return null;
		}
	}
}
