using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Afik.MultiProject.BarrelTown
{

	public class Launcher : MonoBehaviourPunCallbacks
	{

		#region Private Serializable Fields

		[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
		[SerializeField]
		private byte maxPlayersPerRoom = 8;
		[SerializeField] PlayerNameInputField playerInput;

		#endregion

		#region Private Fields

		string gameVersion = "1";

		/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
		/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
		/// Typically this is used for the OnConnectedToMaster() callback.
		bool isConnecting;


		#endregion

		#region Public Fields

		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		[SerializeField]
		private GameObject controlPanel;
		[Tooltip("The UI Label to inform the user that the connection is in progress")]
		[SerializeField]
		private GameObject progressLabel;

		#endregion

		#region MonoBehaviour CallBacks

		private void Awake()
		{
			// makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.AutomaticallySyncScene = true;
		}


		private void Start()
		{
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}

		#endregion

		#region Public Methods

		/// Start the connection process.
		/// - If already connected, we attempt joining a random room
		/// - if not yet connected, Connect this application instance to Photon Cloud Network
		public void Connect()
		{
			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then


			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.IsConnected)
			{
				// we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				//we must first and foremost connect to Photon Online Server.
				isConnecting = PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = gameVersion;
			}


		}

		#endregion

		#region MonoBehaviourPunCallbacks Callbacks

		public override void OnConnectedToMaster()
		{
			Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

			if (isConnecting)
			{
				// The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
				PhotonNetwork.JoinRandomRoom();
				isConnecting = false;

			}
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			isConnecting = false;

			Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
			GameManager.getInstance.AddToPlayerList(playerInput.playerName);
			// We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("We load the 'Lobby' ");


				// #Critical
				// Load the Room Level.
				PhotonNetwork.LoadLevel("Lobby");
			}

		}




		#endregion


	}
}