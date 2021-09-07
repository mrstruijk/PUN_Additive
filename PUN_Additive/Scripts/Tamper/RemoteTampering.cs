using Photon.Pun;
using UnityEngine;


public class RemoteTampering : MonoBehaviour
{
	public string tamperWithTag = "Avatar";
	private PhotonView photonView;

	/// <summary>
	/// Can be called from UI
	/// </summary>
	/// <param name="methodName"></param>
	public void Tamper(string methodName)
	{
		var withTag = GameObject.FindWithTag(tamperWithTag);

		if (withTag != null)
		{
			var photonView = PhotonView.Get(withTag);

			if (photonView == null)
			{
				Debug.LogFormat("Could not find PhotonView on: {0}", withTag.name);
				return;
			}

			photonView.RPC(methodName, RpcTarget.All);
		}
		else
		{
			Debug.LogFormat("Could not find gameobject with tag: {0}", tamperWithTag);
		}
	}
}

