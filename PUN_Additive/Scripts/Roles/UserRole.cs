using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public abstract class UserRole : ScriptableObject
{
	[AssetsOnly] [BoxGroup("UserPrefab", CenterLabel = true)]
	public GameObject userPrefab;

	[BoxGroup("UserPrefab", CenterLabel = true)]
	[SerializeField] protected Vector3 prefabSpawnPosition = new Vector3(0, 3, 0);


	/// <summary>
	/// Always run base first
	/// </summary>
	public virtual void OnEnteredRoom()
	{
		InstantiatePrefab();

		SetRoleAsCustomProperty();
	}


	private void InstantiatePrefab()
	{
		if (userPrefab == null)
		{
			Debug.LogFormat("Missing {0}", userPrefab);
		}
		else
		{
			if (UserInstanceManager.LocalPlayerInstance != null)
			{
				return;
			}

			Debug.LogFormat("Instantiated new {0}", userPrefab.name);
			PhotonNetwork.Instantiate(userPrefab.name, prefabSpawnPosition, Quaternion.identity, 0);
		}
	}


	private void SetRoleAsCustomProperty()
	{
		PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"Role", this.name}});
	}
}
