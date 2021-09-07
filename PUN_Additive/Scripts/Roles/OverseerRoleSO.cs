using mrstruijk.PUN;
using UnityEngine;


[CreateAssetMenu(fileName = "OverseerRole", menuName = "mrstruijk/PUN/Roles/Overseer")]
public class OverseerRoleSO : UserRole
{
	[Space(15)]
	[SerializeField] private bool becomeMasterAutomatically = true;


	public override void OnEnteredRoom()
	{
		base.OnEnteredRoom();

		BecomeMaster(becomeMasterAutomatically);
	}


	private void BecomeMaster(bool autoMaster)
	{
		if (autoMaster == true)
		{
			FindObjectOfType<NetworkHostConnector>().BecomeMaster();
		}
	}
}
