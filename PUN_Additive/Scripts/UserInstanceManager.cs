using Photon.Pun;
using UnityEngine;


public class UserInstanceManager : MonoBehaviour
{
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;

	private PhotonView photonView;

	private void Awake()
	{
		photonView = PhotonView.Get(this);

		if (photonView.IsMine == true)
		{
			LocalPlayerInstance = gameObject;
		}

		DontDestroyOnLoad(this.gameObject);
	}
}
