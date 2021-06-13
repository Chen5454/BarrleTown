using Afik.MultiProject.BarrelTown;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#region Elor's work
public enum GamePhases
{
	Day,
	Night,
	talk,
	Vote
}
public class GameManager : MonoBehaviourPunCallbacks
{
	private static GameManager _instance;
	public static GameManager getInstance => _instance;

	[Header("References")]
	[SerializeField] ItemBankSO bankSO;
	[SerializeField] FieldOfView fov;
	[SerializeField] BarrelManager barrelManager;
	[SerializeField] CameraController camera;
	[SerializeField] VotingArea votingArea;
	[SerializeField] LobbyController lobbyCon;
	[SerializeField] ChatUI chat;
	Shop shop;

	[Header("Phases")]
	public List<string> playersNameList;
	public List<VillagerCharacter> playersList;
	public VotePhase votePhase;
	[SerializeField] bool isGameActive = false;
	[Header("Current Phase")]
	[SerializeField]
	GamePhases gamePhase;

	[Header("Timers")]
	[SerializeField]
	float dayTime;
	[SerializeField]
	float nightTime;
	[SerializeField]
	float voteTime;
	[SerializeField]
	float waitForVoteTime; // this will have to be lower then vote time, how long players will have to wait untill they can vote someone

	public float wolfViewRange;
	public float VillageViewRange;

	[SerializeField]
	private float timer;
	[SerializeField]
	bool canVote;



	//to generate who is the werewolf
	public bool[] isWereWolf;

	NameHolder[] nameHolders;

	public VillagerCharacter player;
	public VillagerCharacter werewolf;
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else if (_instance != this)
		{
			Destroy(this.gameObject);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log("Game Manger is now On");
	}

	// Update is called once per frame
	void Update()
	{
		if (isGameActive)
		{
			GameTimers();
			if (fov != null)
			{
				fov.SetOrigin();
			}

			if (Input.GetKeyDown(KeyCode.V))
			{
				RPC_ShowNames();
				//photonView.RPC("RPC_ShowNames", RpcTarget.AllBufferedViaServer);
			}

		}


	}

	private void OnLevelWasLoaded(int level)
	{
		if (level == 1)
        {
			lobbyCon = FindObjectOfType<LobbyController>();
		}
		if (level == 2)
		{
			InitGame();

		}
	}

	public void GameTimers()
	{
		if (timer >= 0)
		{

			timer -= Time.deltaTime;
		}
		else
		{
			SwitchGamePhases();
		}
	}

	public void InitGame()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			isWereWolf = new bool[playersNameList.Count];
			int randomIndex = UnityEngine.Random.Range(0, playersNameList.Count);
			isWereWolf[randomIndex] = true;

			photonView.RPC("RPC_ChooseWereWolf", RpcTarget.AllBufferedViaServer, isWereWolf);
		}

		if (fov == null)
			fov = FindObjectOfType<FieldOfView>();
		if (barrelManager == null)
			barrelManager = FindObjectOfType<BarrelManager>();
		if (camera == null)
			camera = FindObjectOfType<CameraController>();
		if (votingArea == null)
			votingArea = FindObjectOfType<VotingArea>();
		if (chat == null)
			chat = FindObjectOfType<ChatUI>();
		if (shop == null)
			shop = FindObjectOfType<Shop>();

		timer = dayTime;
		gamePhase = GamePhases.Day;
		isGameActive = true;

		barrelManager.canStartGeneration = true;
		barrelManager.GenerateBarrels();


		shop.canGenerateNewRecipe = true;
		if (shop.canGenerateNewRecipe)
			if (PhotonNetwork.IsMasterClient)
			{
				shop.canGetReward = true;
				shop.GenerateNewRecipe();
			}




		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("RPC_test", RpcTarget.AllBufferedViaServer);
		}
		StartCoroutine(delayedList());


	}

	[PunRPC]
	void RPC_ChooseWereWolf(bool[] boolArray)
	{
		this.isWereWolf = boolArray;

		for (int i = 0; i < isWereWolf.Length; i++)
		{
			// iswerewolf[0]= true && (playernamelist = "silverpoop" == nickname = "silverpoop") = true 
			if (isWereWolf[i] && playersNameList[i] == PhotonNetwork.NickName)
			{
				Debug.Log("werewolf at index of: " + i + " playername: " + playersNameList[i] + " network nickname: " + PhotonNetwork.NickName);
				InstantiatePlayer(true);
				break;
			}
			else if (!isWereWolf[i] && playersNameList[i] == PhotonNetwork.NickName)
			{
				InstantiatePlayer(false);
				break;
			}
		}
	}

	void InstantiatePlayer(bool _IsWereWolf)
	{
		int spawnIndex = -1;
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
			{
				spawnIndex = i + 1;
				break;
			}

		}
		Transform spawn = GameObject.Find("Spawn" + spawnIndex.ToString()).transform;

		GameObject _player = PhotonNetwork.Instantiate("WereWolf", spawn.position, new Quaternion(0, 0, 0, 0));
		player = _player.GetComponent<VillagerCharacter>();
		player.isWerewolf = _IsWereWolf;

		
	}



	IEnumerator delayedList()
	{
		yield return new WaitForSeconds(0.4f);
		playersList = new List<VillagerCharacter>(FindObjectsOfType<VillagerCharacter>());
		photonView.RPC("RPC_GetPlayerList", RpcTarget.AllBufferedViaServer);
		yield return new WaitForSeconds(0.4f);
		RPC_ShowNames();



	}
	public void SwitchGamePhases()
	{
		switch (gamePhase)
		{
			case GamePhases.Day: //switches to night
				gamePhase = GamePhases.Night;
				if (player.isWerewolf)
					player.wereWolf.Transform();

				ShowNames(false);


				timer = nightTime;
				Debug.Log("Switching to Night");


				fov.SetNightFOV(player.isWerewolf);

				if (!barrelManager.canStartGeneration)
					barrelManager.canStartGeneration = true;
				break;
			case GamePhases.Night://switches to talk
				gamePhase = GamePhases.talk;
				if (player.isWerewolf)
					player.wereWolf.Transform();

				ShowNames(true);

				if (player.GETIsPicked)
					if (player.box != null)
					{
						player.box.transform.parent = null;
						player.speed = player.speed * 2;
						player.GETIsPicked = false;
					}



				chat.SetChatVisibility(true);
				timer = waitForVoteTime;
				StartTalkPhase();
				Debug.Log("Switching to Talk");
				fov.SetDayFOV();
				UIManager.getInstance.shop.shopRef.GenerateNewShopRecipe();
				if (UIManager.getInstance.shop.shopRef.canGenerateNewRecipe)
					if (PhotonNetwork.IsMasterClient)
						shop.GenerateNewRecipe();
				barrelManager.GenerateBarrels();

				camera.setCameraToGamePhase(true);
				SetPlayersAtVotingPosition();
				break;
			case GamePhases.talk://switches to Vote
				votingArea.ShowVotingButtons(true);



				gamePhase = GamePhases.Vote;
				timer = voteTime;
				canVote = true;
				Debug.Log("Players can vote");
				break;
			case GamePhases.Vote: //switches to Day


				chat.SetChatVisibility(false);
				camera.setCameraToGamePhase(false);
				votingArea.ShowVotingButtons(false);
				votingArea.PlayersCanMove();
				votingArea.CheckVotes();

				canVote = false;
				gamePhase = GamePhases.Day;
				timer = dayTime;
				Debug.Log("Switching to Day");
				break;
			default:
				break;
		}
	}



	public void ShowNames(bool _show)
	{
		for (int i = 0; i < nameHolders.Length; i++)
		{
			if (nameHolders[i] != null)
				nameHolders[i].gameObject.SetActive(_show);
		}
	}




	#region Day_Region

	public void StartDayPhase()
	{
		//players see normal, reset barrel/shop, ETC
	}

	#endregion

	#region Night_Region

	public void StartNightPhase()
	{
		//werewolf transforms, villagers visions get thinner, ETC
	}

	#endregion

	#region Talk_And_Vote_Region

	public void StartTalkPhase()
	{
		//votePhase.SetPlayersAtTheirVotingSpots(playersList);
		//disable players movement


		//let players talk in chat, move players physically to the campfire(or vote site) each in their own seat
	}

	public void StartVotePhase()
	{
		//let them vote
	}
	#endregion

	#region Afik's Work

	#region Photon Callbacks


	/// Called when the local player left the room. We need to load the launcher scene.
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene(0);
	}

	public override void OnPlayerEnteredRoom(Player other)
	{


		//Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

		if (photonView.IsMine)
			AddToPlayerList(other.NickName);

		if (PhotonNetwork.IsMasterClient)
		{
			//Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


			//LoadArena();
		}

		
		

	}
	public override void OnPlayerLeftRoom(Player other)
	{
		//Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

		RemovePlayerFromList(other.NickName);


	


		if (PhotonNetwork.IsMasterClient)
		{
			//Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

			//LoadArena();


			//future::Check win condition 
		}
	}



	#endregion

	#region Public Methods


	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}


	#endregion

	#region Private Methods


	void LoadArena()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
		}
		Debug.LogFormat("Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
		PhotonNetwork.LoadLevel("Lobby");
	}


	#endregion









	#endregion

	#region chat 

	public void SendMessageToAll(string message, string senderName)
	{
		photonView.RPC("RPC_SendMessage", RpcTarget.AllBufferedViaServer, message, senderName);
	}

	[PunRPC]
	void RPC_SendMessage(string message, string senderName)
	{
		chat.ShowMessage(message, senderName);
	}


	#endregion

	#region vote pun
	public void KillVotedPlayer(int playerIndex)
	{
		photonView.RPC("RPC_KillVotedPlayer", RpcTarget.AllBufferedViaServer, playerIndex);
	}

	[PunRPC]
	void RPC_KillVotedPlayer(int playerIndex)
	{
		playersList[playerIndex].currentHp = 0;
		playersList[playerIndex].canMove = false;
	}





	public void GetPlayerVotes(int index, int amount)
	{
		photonView.RPC("RPC_GetVotes", RpcTarget.AllBufferedViaServer, index, amount);
	}
	[PunRPC]
	void RPC_GetVotes(int index, int amount)
	{
		votingArea.VoteToPlayer(index, amount);
	}



	public void ResetVotes()
	{
		photonView.RPC("RPC_ResetVotes", RpcTarget.AllBufferedViaServer);
	}
	[PunRPC]
	void RPC_ResetVotes()
	{
		votingArea.ResetVotes();
	}


	[PunRPC]
	void RPC_test()
	{
		//StartCoroutine(delayedList());
	}

	public void SetPlayersAtVotingPosition()
	{
		if (PhotonNetwork.IsMasterClient)
			photonView.RPC("RPC_SetPlayersAtVotingPosition", RpcTarget.AllBufferedViaServer);
	}

	[PunRPC]
	void RPC_SetPlayersAtVotingPosition()
	{
		votingArea.MovePlayersToVotingArea(this.playersList);
	}

	#endregion

	#region player list
	[PunRPC]
	void RPC_GetPlayerList()
	{
		playersList = playersList.OrderBy(x => x.photonView.ViewID).ToList();

		for (int i = 0; i < playersList.Count; i++)
		{
			if (playersList[i].isWerewolf)
				playersList[i].tag = "Werewolf";
			else
				playersList[i].tag = "Player";
		}
	}


	public GameObject playerName;
	void RPC_ShowNames()
	{
		for (int i = 0; i < playersList.Count; i++)
		{
			Transform parent = GameObject.Find("PlayerNameHolders").transform;
			GameObject nameHolder = Instantiate(playerName, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), parent);

			//nameHolder.transform.SetParent(parent);
			nameHolder.GetComponent<NameHolder>().playerPos = playersList[i].transform;
			nameHolder.GetComponent<NameHolder>().nameText.text = playersNameList[i];
		}


		nameHolders = FindObjectsOfType<NameHolder>();
	}


	public void AddToPlayerList(string playerName)
	{
		photonView.RPC("RPC_AddToPlayerList", RpcTarget.AllBufferedViaServer, playerName);
	}
	[PunRPC]
	void RPC_AddToPlayerList(string playerName)
	{
		playersNameList.Add(playerName);
		Debug.Log("Added name: " + playerName);
	}
	public void RemovePlayerFromList(string playerName)
	{
		if (lobbyCon == null)
			lobbyCon = FindObjectOfType<LobbyController>();


		int listIndex = playersNameList.IndexOf(playerName);
		playersNameList.RemoveAt(listIndex);
		playersList.RemoveAt(listIndex);
		//photonView.RPC("RPC_RemovePlayerFromList", RpcTarget.AllBufferedViaServer, playerName);

	}


	public void TryToShowPlayerNames()
	{
		if (lobbyCon == null && FindObjectOfType<LobbyController>() != null)
			lobbyCon = FindObjectOfType<LobbyController>();
		if (lobbyCon != null)
		{
			lobbyCon.ShowPlayerName();
		}
	}
	#endregion







	#region shop
	public PickableItem item;
	public void ShowDroppedItemInfo(int _index)
	{
		if (PhotonNetwork.IsMasterClient)
			photonView.RPC("RPC_ShowDroppedItemInfo", RpcTarget.AllBufferedViaServer, _index);
	}

	[PunRPC]
	public void RPC_ShowDroppedItemInfo(int _index)
	{
		shop.ChangeRewardInfo(bankSO.itemList[_index]);

	}

	public void ShowRecipeToAll(int _index)
	{
		photonView.RPC("RPC_ShowRecipeToAll", RpcTarget.AllBufferedViaServer, _index);
	}

	[PunRPC]
	void RPC_ShowRecipeToAll(int _index)
	{
		shop.GetNewGeneratedRecipeIndex(_index);
	}

	#endregion

	#region PUNRPC
	public void ShowNewGeneratedRecipe()
	{
		if (PhotonNetwork.IsMasterClient)
			photonView.RPC("RPC_ShowNewGeneratedRecipe", RpcTarget.AllBufferedViaServer);
	}

	public void ShowRecipeOnUI(int[] test)
	{
		photonView.RPC("RPC_ShowRecipeOnUI", RpcTarget.AllBufferedViaServer, test);
	}
	public void CheckIfRecipeCompleted()
	{
		photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);
	}
	[PunRPC]
	void RPC_ShowNewGeneratedRecipe()
	{
		UIManager.getInstance.shop.shopRef.GenerateNewRecipe();
	}

	[PunRPC]
	void RPC_ShowRecipeOnUI(int[] _array)
	{
		UIManager.getInstance.shop.ShowRecipe(_array);
	}
	[PunRPC]
	void RPC_CheckIfRecipeCompleted()
	{
		UIManager.getInstance.shop.shopRef.CheckIfRecipeCompleted();
	}
	#endregion


	public override void OnJoinedRoom()
	{
		TryToShowPlayerNames();
	}
	public override void OnJoinedLobby()
	{
		TryToShowPlayerNames();
	}

	public override void OnLeftLobby()
	{
		TryToShowPlayerNames();
	}



}
[Serializable]
public class VotePhase
{
	[SerializeField]
	private Transform[] playerVoteSpots;

	public void SetPlayersAtTheirVotingSpots(List<GameObject> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			players[i].transform.position = playerVoteSpots[i].position;
			//player will not have the ability to move
		}
	}
}

[Serializable]
public class NightPhase
{

}

[Serializable]
public class DayPhase
{

}
#endregion


