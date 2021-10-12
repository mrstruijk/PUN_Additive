using System.Collections;
using mrstruijk.Events;
using Photon.Pun;
using UnityEngine;


namespace mrstruijk.SceneManagement
{
	public class SceneManagementManager : MonoBehaviour
	{
		[SerializeField] private GameObject sceneManagementPrefab;
		private static SceneManagement sceneManagement;
		[SerializeField] private PhotonConnectionSettingsSO connectionSettings;


		private void OnEnable()
		{
			EventSystem.OnJoinedRoom += CreateSceneManagement;
		}


		private void CreateSceneManagement()
		{
			if (PhotonNetwork.IsMasterClient)
			{
				if (sceneManagement == null)
				{
					PhotonNetwork.InstantiateRoomObject(sceneManagementPrefab.name, transform.position, Quaternion.identity);
					sceneManagement = FindObjectOfType<SceneManagement>();
				}
			}
			else
			{
				StartCoroutine(FindSceneManagement());
			}
		}


		private static IEnumerator FindSceneManagement()
		{
			var found = false;

			while (found == false)
			{
				found = FindObjectOfType<SceneManagement>();
				yield return null;
			}

			sceneManagement = FindObjectOfType<SceneManagement>();
		}


		private void TakeoverSceneManager()
		{
			sceneManagement.PhotonView.OwnershipTransfer = OwnershipOption.Takeover;
			sceneManagement.PhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);
		}


		private void OnDisable()
		{
			EventSystem.OnJoinedRoom -= CreateSceneManagement;
		}
	}
}
