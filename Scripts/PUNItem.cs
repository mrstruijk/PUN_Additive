using UnityEngine;


public class PUNItem : MonoBehaviour
{
	#if UNITY_EDITOR

	public PUNType punType = PUNType.Players;
	public string itemName = "";
	public Object inspectedScript;

	public enum PUNType
	{
		Players,
		RoomObjects,
		NetworkObjects
	}

	#endif
}
