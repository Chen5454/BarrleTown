using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Afik.MultiProject.BarrelTown
{
	public class LobbyController : MonoBehaviourPunCallbacks
	{
		private PhotonView myPhotonView; //updates timer

		[SerializeField]
		private int multiplayerSceneIndex; //
		[SerializeField]
		private int menuSceneIndex;//scene navigation indexes

		private int playerCount;
		private int roomSize;

		[SerializeField]
		private int minPlayersToStart;

		//display
		[SerializeField]
		private Text roomCountDisplay;
		[SerializeField]
		private Text timerToStartDisplay;
		[SerializeField]
		private GameObject[] playerNameFields;


		// timer bools
		private bool readyToCountDown;
		private bool readyToStart;
		private bool startingGame;

		//countdown variables
		private float timerToStartGame;
		private float notFullGameTimer;
		private float fullGameTimer;


		//countdown timer reset variables
		[SerializeField]
		private float maxWaitTime;
		[SerializeField]
		private float maxFullGameWaitTime;

		GameManager GM => GameManager.getInstance;


		void Start()
		{
			myPhotonView = GetComponent<PhotonView>();
			fullGameTimer = maxFullGameWaitTime;
			notFullGameTimer = maxWaitTime;
			timerToStartGame = maxWaitTime;

			PlayerCountUpdate();
		}

		void PlayerCountUpdate()
		{
			playerCount = PhotonNetwork.PlayerList.Length;
			roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
			roomCountDisplay.text = playerCount + "/" + roomSize;

			GameManager.getInstance.playersNameList = new System.Collections.Generic.List<string>();

			for (int i = 0; i < playerCount; i++)
			{
				GameManager.getInstance.playersNameList.Add(PhotonNetwork.PlayerList[i].NickName);
			}


			if (playerCount == roomSize)
			{
				readyToStart = true;
			}
			else if (playerCount >= minPlayersToStart)
			{
				readyToCountDown = true;
			}
			else
			{
				readyToCountDown = false;
				readyToStart = false;
			}
			ShowPlayerName();
		}


		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			PlayerCountUpdate();

			/*if (PhotonNetwork.IsMasterClient)
			{
				myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
			}*/

			fullGameTimer = maxFullGameWaitTime;
			notFullGameTimer = maxWaitTime;
			timerToStartGame = maxWaitTime;
		}

		[PunRPC]
		public void RPC_SendTimer(float timeIn)
		{
			timerToStartGame = timeIn;
			notFullGameTimer = timeIn;
			if (timeIn < fullGameTimer)
			{
				fullGameTimer = timeIn;
			}
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			fullGameTimer = maxFullGameWaitTime;
			notFullGameTimer = maxWaitTime;
			timerToStartGame = maxWaitTime;

			PlayerCountUpdate();
			ShowPlayerName();

		}

		private void Update()
		{
			WaitingForMorePlayers();
		}

		void WaitingForMorePlayers()
		{
			if (playerCount <= 1)
			{
				ResetTimer();
			}
			if (readyToStart)
			{
				fullGameTimer -= Time.deltaTime;
				timerToStartGame = fullGameTimer;
			}
			else if (readyToCountDown)
			{
				notFullGameTimer -= Time.deltaTime;
				timerToStartGame = notFullGameTimer;
			}

			string tempTimer = string.Format("{0:00}", timerToStartGame);
			timerToStartDisplay.text = tempTimer;

			if (timerToStartGame <= 0f)
			{
				if (startingGame)
					return;
				StartGame();
			}
		}


		public void LeaveRoom()
		{
			Destroy(GameManager.getInstance.gameObject);
			PhotonNetwork.LeaveRoom();
			SceneManager.LoadScene(menuSceneIndex);
		}
		private void ResetTimer()
		{
			timerToStartGame = maxWaitTime;
			notFullGameTimer = maxWaitTime;
			fullGameTimer = maxFullGameWaitTime;
		}

		public void StartGame()
		{
			startingGame = true;
			if (!PhotonNetwork.IsMasterClient)
				return;
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.LoadLevel(multiplayerSceneIndex);
		}

		public void ShowPlayerName()
		{
			photonView.RPC("RPC_ShowPlayerName", RpcTarget.AllBufferedViaServer);
		}

		[PunRPC]
		void RPC_ShowPlayerName()
		{
			for (int i = 0; i < playerNameFields.Length; i++)
			{
				if (i < GameManager.getInstance.playersNameList.Count)
				{
					playerNameFields[i].SetActive(true);
					playerNameFields[i].transform.GetChild(0).GetComponent<Text>().text = GM.playersNameList[i];
				}
				else
				{
					playerNameFields[i].SetActive(false);
				}
			}
		}


	}
}