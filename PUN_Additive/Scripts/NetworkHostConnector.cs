using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;


namespace mrstruijk.PUN
{
    public class NetworkHostConnector : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject sceneManagementPrefab;

        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public PhotonConnectionSettingsSO connectionSettings;

        private SceneManagement sceneManagement;


        private void Start()
        {
            AutoSyncScenesAcrossClients(PhotonConnectionSettingsSO.AutoSyncScenes);

            connectionSettings.CreateRoomOptions();

            connectionSettings.ConnectingToMaster();

            ConnectUsingSettings();
        }


        private static void AutoSyncScenesAcrossClients(bool autoSync)
        {
            PhotonNetwork.AutomaticallySyncScene = autoSync;
        }


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt to join a specific room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            if (PhotonNetwork.IsConnected == true)
            {
                JoinOrCreateRoom();
            }
        }


        private void JoinOrCreateRoom()
        {
            PhotonNetwork.JoinOrCreateRoom(connectionSettings.roomName, connectionSettings.roomOptions, null);
        }


        private void ConnectUsingSettings()
        {
            if (PhotonNetwork.IsConnected == true)
            {
                return;
            }

            PhotonNetwork.GameVersion = connectionSettings.gameVersion;

            connectionSettings.IsConnecting = PhotonNetwork.ConnectUsingSettings();
        }


        public override void OnConnectedToMaster()
        {
            if (connectionSettings.IsConnecting == false)
            {
                return;
            }

            JoinOrCreateRoom();

            connectionSettings.IsConnecting = false;
        }


        public override void OnJoinedRoom()
        {
            if (PhotonConnectionSettingsSO.IsMaster)
            {
                if (sceneManagement == null)
                {
                    PhotonNetwork.InstantiateRoomObject(sceneManagementPrefab.name, transform.position, Quaternion.identity, 0);
                    sceneManagement = FindObjectOfType<SceneManagement>();
                    sceneManagement.transform.parent = transform.parent;
                    sceneManagement.RPCLoadSceneAddtively(sceneManagement.StartScene);
                }
            }
            else
            {
                StartCoroutine(FindSceneManagement());
            }
        }


        private IEnumerator FindSceneManagement()
        {
            bool found = false;

            while (found == false)
            {
                found = FindObjectOfType<SceneManagement>();
                yield return null;
            }

            sceneManagement = FindObjectOfType<SceneManagement>();
            sceneManagement.LoadSceneLocally(sceneManagement.StartScene);
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);

            PhotonNetwork.CreateRoom(connectionSettings.roomName, connectionSettings.roomOptions);
        }


        [Button]
        private void TakeoverSceneManager()
        {
            sceneManagement.photonView.OwnershipTransfer = OwnershipOption.Takeover;
            sceneManagement.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }


        public void BecomeMaster()
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        private static void KickAllUsers()
        {
            if (PhotonConnectionSettingsSO.IsMaster == false)
            {
                return;
            }

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.IsMasterClient)
                {
                    PhotonNetwork.LeaveRoom();
                }
                else
                {
                    PhotonNetwork.CloseConnection(player);
                }
            }
        }


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            // sceneManagement.LoadLocalScene(sceneManagement.Lobby);
        }


        public override void OnPlayerEnteredRoom(Player other)
        {
            if (!PhotonConnectionSettingsSO.IsMaster)
            {
                return;
            }

            DoStuffBasedOnOtherRole(other);
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            if (!PhotonConnectionSettingsSO.IsMaster)
            {
                return;
            }

            DoStuffBasedOnOtherRole(other);
        }


        private static void DoStuffBasedOnOtherRole(Player other)
        {
            var role = "PatientRole";

            if ((string) other.CustomProperties["Role"] == role) // Some other code. Using this, we can instantiate / load specific levels or gameobjects based on what role is entering
            {

            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            connectionSettings.IsConnecting = false;

            Debug.LogWarningFormat("PUN OnDisconnected() was called by PUN with reason {0}", cause);
        }
    }
}


