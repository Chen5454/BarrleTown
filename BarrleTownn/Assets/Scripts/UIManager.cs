using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class UIManager : MonoBehaviourPunCallbacks
{
	private static UIManager _instance;
	public static UIManager getInstance => _instance;
	public ShopUI shop;

	public GameObject villagerWinScreen;
	public GameObject wolfWinScreen;
	public TextMeshProUGUI[] playernames;
	public GameObject[] playerVillager;
	public TextMeshProUGUI werewolfName;
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}





	public void ShowWinScreen(bool isWerewolf)
	{
		photonView.RPC("RPC_ShowWinScreen", RpcTarget.AllBufferedViaServer, isWerewolf);
	}
	[PunRPC]
	void RPC_ShowWinScreen(bool isWerewolf)
	{
		if (isWerewolf)
		{
			ShowWerewolfVictoryScreen();
		}
		else
		{
			ShowVillageVictoryScreen();
		}
	}
	public void ShowVillageVictoryScreen()
	{
		villagerWinScreen.SetActive(true);
		List<string> names = new List<string>();
		for (int i = 0; i < GameManager.getInstance.playersList.Count; i++)
		{
			if (!GameManager.getInstance.playersList[i].isWerewolf)
			{
				names.Add(GameManager.getInstance.playersList[i].playerName);
			}
		}

		for (int i = 0; i < names.Count; i++)
		{
			playerVillager[i].SetActive(true);
			playernames[i].text = names[i];
		}

	}

	public void ShowWerewolfVictoryScreen()
	{
		List<string> names = new List<string>();
		wolfWinScreen.SetActive(true);
		for (int i = 0; i < GameManager.getInstance.playersList.Count; i++)
		{
			if (GameManager.getInstance.playersList[i].isWerewolf)
			{
				names.Add(GameManager.getInstance.playersList[i].playerName);
			}
		}

		for (int i = 0; i < names.Count; i++)
		{
			werewolfName.text = names[i];
		}

	}


	public void ReturnToMainMenu() 
	{
		PhotonNetwork.LeaveRoom();
	}


}
