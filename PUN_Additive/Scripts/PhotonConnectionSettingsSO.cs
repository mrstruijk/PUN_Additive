using System;
using mrstruijk.PUN;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


[CreateAssetMenu(fileName = "Photon Connection Settings", menuName = "mrstruijk/PUN/Photon Connection Settings")]
public class PhotonConnectionSettingsSO : ScriptableObject
{
	[Range(1, 6)] public byte maxDevicesPerRoom = 4;
	public string roomName = "123456";
	public string gameVersion = "0.1"; // This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).

	public RoomOptions roomOptions;

	public Action ConnectingToMaster = delegate { };


	private const bool autoSyncScenes = true;
	public static bool AutoSyncScenes
	{
		get => autoSyncScenes;
	}


	private const bool roomIsVisibleOnNetwork = false;
	private static bool RoomIsVisibleOnNetwork
	{
		get => roomIsVisibleOnNetwork;
	}


	private bool isConnecting;
	public bool IsConnecting
	{
		get => isConnecting;
		set => isConnecting = value;
	}


	public void CreateRoomOptions()
	{
		roomOptions = new RoomOptions
		{
			MaxPlayers = maxDevicesPerRoom,
			IsVisible = RoomIsVisibleOnNetwork
		};
	}


	public static bool IsMaster
	{
		get => PhotonNetwork.IsMasterClient;
	}


	/// <summary>
	/// Called from UI
	/// </summary>
	public void Connect()
	{
		FindObjectOfType<NetworkHostConnector>().Connect();
	}


	/// <summary>
	/// Activated from UI
	/// </summary>
	public void BecomeMaster()
	{
		FindObjectOfType<NetworkHostConnector>().BecomeMaster();
	}


	/// <summary>
	/// Called from UI
	/// </summary>
	/// <returns></returns>
	public void LeaveRoom()
	{
		FindObjectOfType<NetworkHostConnector>().LeaveRoom();
	}
}
