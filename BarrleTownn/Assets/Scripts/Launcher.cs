using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Afik.MultiProject.BarrelTown
{

	public class Launcher : MonoBehaviourPunCallbacks
	{
		public GameManager gameManager;
		public GameObject loginPanel;
		[Header("Lobby List Related")]

		public GameObject lobbyListPanel;
		public Transform lobbyListParent;
		public GameObject roomSlotPF;
		public List<RoomSlotDetail> roomPool = new List<RoomSlotDetail>();
		public bool isOnLobbyScreen;
		[Header("New Room Setting")]
		public GameObject newRoomPanel;

		public TMP_InputField roomNameInput;
		[SerializeField] int maxPlayersAllowed;
		[SerializeField] int minPlayersAllowed;

		public int maxPlayersAmount;
		public string roomName;
		public TextMeshProUGUI maxAmountText;


		[Header("Other")]

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
			cachedRoomList = new Dictionary<string, RoomInfo>();
			roomListEntries = new Dictionary<string, GameObject>();

			

			if (FindObjectOfType<GameManager>() == null)
			{
				Instantiate(gameManager);
			}

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
		/// 
	

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.G))
			{
				RefreshLobbyList();
			}
		}

		public void Connect()
		{

			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then


			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.IsConnected)
			{
				// we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
				//PhotonNetwork.JoinRandomRoom();
				
				ShowLobby();
				//show lobby list

			}
			else
			{


				//we must first and foremost connect to Photon Online Server
				
				PhotonNetwork.GameVersion = "0.1";
				isConnecting = PhotonNetwork.ConnectUsingSettings();

				//PhotonNetwork.GameVersion = gameVersion;
			}


		}


		#endregion

		#region Create_New_Room_Related

		public void OnChangeRoomName()
		{
			roomName = roomNameInput.text;
		}

		public void OnClickAddMax()
		{
			maxPlayersAmount += 1;

			if (maxPlayersAmount > maxPlayersAllowed)
			{
				maxPlayersAmount = minPlayersAllowed;
			}
			ChangeAmountText();
		}

		public void OnClickRemoveMax()
		{
			maxPlayersAmount -= 1;

			if (maxPlayersAmount < minPlayersAllowed)
			{
				maxPlayersAmount = maxPlayersAllowed;
			}
			ChangeAmountText();
		}

		public void ChangeAmountText()
		{
			maxAmountText.text = maxPlayersAmount.ToString();
		}

		public void OnClickCancelNewRoom()
		{
			newRoomPanel.SetActive(false);
		}

		public void OnClickCreateNewRoom()
		{
			newRoomPanel.SetActive(true);
		}

	
		public void CreateNewRoom()
		{

			PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = (byte)maxPlayersAmount, IsVisible = true, IsOpen = true });
		}

		#endregion


		#region lobby_Related

		public override void OnJoinedLobby()
		{
			Debug.Log("In lobby");
			ShowLobby();
			cachedRoomList.Clear();
			ClearRoomListView();
		}


		public void ShowLobby()
		{
			
			newRoomPanel.SetActive(false);
			loginPanel.SetActive(false);
			lobbyListPanel.SetActive(true);

			isOnLobbyScreen = true;
		}



		//public override void OnRoomListUpdate(List<RoomInfo> roomList)
		//{

		//	Debug.Log("Update ROOOOOOOOOOOOOOOOM");
		//	//bool canUseExisted = false;

		//	string roomName;
		//	string currentPlayers;
		//	string maxPlayer;

		//	//foreach (RoomInfo info in roomList)
		//	//{
		//	//	RoomSlotDetail newRoom = Instantiate(roomSlotPF, lobbyListParent).GetComponent<RoomSlotDetail>();
		//	//	roomName = info.Name;
		//	//	currentPlayers = info.PlayerCount.ToString();
		//	//	maxPlayer = info.MaxPlayers.ToString();

		//	//	newRoom.RoomSlotInit(roomName, currentPlayers, maxPlayer);
		//	//	roomPool.Add(newRoom);
		//	//}

		//	for (int i = 0; i < roomList.Count; i++)
		//	{
		//		RoomSlotDetail newRoom = Instantiate(roomSlotPF, lobbyListParent).GetComponent<RoomSlotDetail>();
		//		roomName = roomList[i].Name;
		//		currentPlayers = roomList[i].PlayerCount.ToString();
		//		maxPlayer = roomList[i].MaxPlayers.ToString();

		//		newRoom.RoomSlotInit(roomName, currentPlayers, maxPlayer);
		//		roomPool.Add(newRoom);
		//	}




		//	//if (roomPool.Count > roomList.Count)
		//	//{
		//	//	// room list 2, room pool 3;
		//	//	// if 3 > 2 true
		//	//	// if 2 > 2 false
		//	//	for (int i = roomPool.Count; i > roomList.Count; i--)
		//	//	{
		//	//		roomPool[i].gameObject.SetActive(false);
		//	//	}
		//	//}


		//	//for (int i = 0; i < roomList.Count; i++)
		//	//{

		//	//	if (roomPool.Count == 0)
		//	//	{
		//	//		//RoomSlotDetail newRoom = Instantiate(roomSlotPF, lobbyListParent).GetComponent<RoomSlotDetail>();
		//	//		//roomName = roomList[i].Name;
		//	//		//currentPlayers = roomList[i].PlayerCount.ToString();
		//	//		//maxPlayer = roomList[i].MaxPlayers.ToString();

		//	//		//newRoom.RoomSlotInit(roomName, currentPlayers, maxPlayer);
		//	//		//roomPool.Add(newRoom);
		//	//	}
		//	//	else
		//	//	{
		//	//		for (int j = 0; j < roomPool.Count; j++)
		//	//		{
		//	//			if (!roomPool[j].gameObject.activeInHierarchy)
		//	//			{
		//	//				RoomSlotDetail newRoom = Instantiate(roomSlotPF).GetComponent<RoomSlotDetail>();
		//	//				roomName = roomList[i].Name;
		//	//				currentPlayers = roomList[i].PlayerCount.ToString();
		//	//				maxPlayer = roomList[i].MaxPlayers.ToString();

		//	//				newRoom.RoomSlotInit(roomName, currentPlayers, maxPlayer);

		//	//				canUseExisted = true;
		//	//				roomPool[j].gameObject.SetActive(true);

		//	//				break;
		//	//			}
		//	//		}


		//	//		if (!canUseExisted)
		//	//		{
		//	//			RoomSlotDetail newRoom = Instantiate(roomSlotPF, lobbyListParent).GetComponent<RoomSlotDetail>();
		//	//			roomName = roomList[i].Name;
		//	//			currentPlayers = roomList[i].PlayerCount.ToString();
		//	//			maxPlayer = roomList[i].MaxPlayers.ToString();

		//	//			newRoom.RoomSlotInit(roomName, currentPlayers, maxPlayer);
		//	//			roomPool.Add(newRoom);
		//	//		}



		//	//	}



		//	//}

		//}
		private Dictionary<string, RoomInfo> cachedRoomList;
		private Dictionary<string, GameObject> roomListEntries;
		public override void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			Debug.Log("Room Update");
			ClearRoomListView();

			UpdateCachedRoomList(roomList);
			UpdateRoomListView();
		}

		private void ClearRoomListView()
		{
			foreach (GameObject entry in roomListEntries.Values)
			{
				Destroy(entry.gameObject);
			}

			roomListEntries.Clear();
		}

		private void UpdateCachedRoomList(List<RoomInfo> roomList)
		{
			foreach (RoomInfo info in roomList)
			{
				// Remove room from cached room list if it got closed, became invisible or was marked as removed
				if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
				{
					if (cachedRoomList.ContainsKey(info.Name))
					{
						cachedRoomList.Remove(info.Name);
					}

					continue;
				}

				// Update cached room info
				if (cachedRoomList.ContainsKey(info.Name))
				{
					cachedRoomList[info.Name] = info;
				}
				// Add new room info to cache
				else
				{
					cachedRoomList.Add(info.Name, info);
				}
			}
		}

		private void UpdateRoomListView()
		{
			foreach (RoomInfo info in cachedRoomList.Values)
			{
				GameObject entry = Instantiate(roomSlotPF);
				entry.transform.SetParent(lobbyListParent);
				entry.transform.localScale = Vector3.one;
				entry.GetComponent<RoomSlotDetail>().RoomSlotInit(info.Name, info.PlayerCount.ToString(), info.MaxPlayers.ToString());

				roomListEntries.Add(info.Name, entry);
			}
		}



		public void RefreshLobbyList()
		{
			//TypedLobby lobbytype = new TypedLobby("Main Lobby", LobbyType.Default);
			Debug.Log(PhotonNetwork.CountOfRooms);
		}

		public override void OnLeftLobby()
		{
			cachedRoomList.Clear();
			ClearRoomListView();
		}

		#endregion

		#region MonoBehaviourPunCallbacks Callbacks

		public override void OnConnectedToMaster()
		{
			PhotonNetwork.JoinLobby();
			//ShowLobby();
			//Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

			//if (isConnecting)
			//{
			//	// The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
			//	//PhotonNetwork.JoinRandomRoom();
			//	isConnecting = false;

			//}
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			cachedRoomList.Clear();
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			isConnecting = false;

			SceneManager.LoadScene(0);

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

		public void disconnect()
		{
			PhotonNetwork.Disconnect();
		}


		#endregion


	}
}