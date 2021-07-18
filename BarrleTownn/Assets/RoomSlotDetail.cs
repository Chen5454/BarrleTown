using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class RoomSlotDetail : MonoBehaviourPunCallbacks
{
	public Button slotButton;
	public TextMeshProUGUI RoomName;
	public TextMeshProUGUI RoomAmount;


	public void RoomSlotInit(string roomName,string currentPlayersAmount,string maxPlayersAmount)
	{
		RoomName.text = roomName;
		RoomAmount.text = currentPlayersAmount + "/" + maxPlayersAmount;

		slotButton.onClick.RemoveAllListeners();
		slotButton.onClick.AddListener(()=> JoinRoom(roomName));
		
	}
	public void JoinRoom(string roomName)
	{
	
		PhotonNetwork.JoinRoom(roomName);
	}


}
