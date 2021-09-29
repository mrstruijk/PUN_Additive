using UnityEngine;


public class PUNItem : MonoBehaviour
{
	#if UNITY_EDITOR

	public PUNTypes punType = PUNTypes.Players;
	public string itemName = "";
	public Object inspectedScript;

	public enum PUNTypes
	{
		Players,
		RoomObjects
	}

	#endif
}
