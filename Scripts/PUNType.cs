using UnityEngine;


public class PUNType : MonoBehaviour
{
	public PUNTypes punType = PUNTypes.Players;

	public enum PUNTypes
	{
		Players,
		RoomObjects
	}
}
