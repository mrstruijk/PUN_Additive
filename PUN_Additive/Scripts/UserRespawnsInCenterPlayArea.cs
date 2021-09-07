using mrstruijk;
using Photon.Pun;
using UnityEngine;


public class UserRespawnsInCenterPlayArea : MonoBehaviourPunCallbacks
{
	[SerializeField] private Vector3 respawnPosition = new Vector3(0f, 20f, 0f);


	public override void OnEnable()
	{
		base.OnEnable();
		SceneManagement.SceneHasBeenLoaded += CheckIfPlayerIsOverGround;
	}


	private void CheckIfPlayerIsOverGround(string sceneName)
	{
		if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) 	// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
		{
			transform.position = respawnPosition;
		}
	}


	public override void OnDisable()
	{
		base.OnDisable (); // Always call the base to remove callbacks
		SceneManagement.SceneHasBeenLoaded -= CheckIfPlayerIsOverGround;
	}
}
