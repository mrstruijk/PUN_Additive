using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;


public class UserCameraManager : MonoBehaviourPunCallbacks
{
	private void Start()
	{
		var cameraWork = gameObject.GetComponent<CameraWork>();

		if (cameraWork != null)
		{
			if (photonView.IsMine == true)
			{
				cameraWork.OnStartFollowing();
			}
		}
		else
		{
			Debug.LogError("No cameraWork can be found on user prefab ", this);
		}
	}
}
