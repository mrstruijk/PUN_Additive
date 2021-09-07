using Photon.Pun;
using UnityEngine;


/// <summary>
/// Needs to be on GO with which you want to tamper
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class GetTamperedWithRemotelyHO : MonoBehaviour
{
	public Vector3 newScale = new Vector3(2f,2f,2f);
	public Vector3 newPosition = new Vector3(1f, 3f, 1f);

	private Tampering tamper;


	private void Awake()
	{
		tamper = new Tampering(transform);
	}


	/// <summary>
	/// Run via PunRPC from any game object, called by name from RemoteTampering
	/// </summary>
	[PunRPC] private void TamperScale()
	{
		tamper.TamperScale(newScale);
	}


	/// <summary>
	/// Run via PunRPC from any game object, called by name from RemoteTampering
	/// </summary>
	[PunRPC] private void TamperPosition()
	{
		tamper.TamperPosition(newPosition);
	}
}
