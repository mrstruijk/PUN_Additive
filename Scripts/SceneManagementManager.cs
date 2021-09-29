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


		private void OnEnable()
		{
			EventSystem.OnJoinedRoom += CreateSceneManagement;
		}


		private void CreateSceneManagement()
		{
			if (PhotonConnectionSettingsSO.IsMaster)
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
			sceneManagement.photonView.OwnershipTransfer = OwnershipOption.Takeover;
			sceneManagement.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
		}


		private void OnDisable()
		{
			EventSystem.OnJoinedRoom -= CreateSceneManagement;
		}
	}
}
