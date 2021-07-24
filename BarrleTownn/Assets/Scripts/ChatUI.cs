using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatUI : MonoBehaviourPun
{
	public GameObject chat;
	public GameObject messagePrefab;
	public Transform parent;

	private string playerMessage;
	[SerializeField] TMP_InputField inputField;
	[SerializeField] Button butt;

	[SerializeField] List<GameObject> messagePool = new List<GameObject>();

	public void SetChatVisibility(bool _setActive)
	{
		chat.SetActive(_setActive);
		CheckIfAlive();

		if (!_setActive)
		{
			for (int i = 0; i < messagePool.Count; i++)
			{
				messagePool[i].SetActive(false);
				
			}
		}
	}

	public void CheckIfAlive()
	{
		if(GameManager.getInstance.player.currentHp > 0)
		{
			inputField.interactable = true;
			butt.interactable = true;
		}
		else
		{
			inputField.interactable = false;
			butt.interactable = false;
		}
	}
	
	


	public void OnChangeMessageValue(string _message)
	{
		playerMessage = _message;
	}


	public void OnClickSendButt()
	{
		if (playerMessage != "" && playerMessage != null)
		{
			GameManager.getInstance.SendMessageToAll(playerMessage,PhotonNetwork.NickName);
			playerMessage = "";
			inputField.text = playerMessage;
		}
		else
		{
			Debug.Log("cannot send an empty object");
		}
	}

	//put this function in a pun
	public void ShowMessage(string message,string senderName)
	{

		bool hasUsedExisted = false;
		if (messagePool.Count == 0)
		{
			GameObject newMessage = Instantiate(messagePrefab, parent);         
			newMessage.transform.GetComponentInChildren<TextMeshProUGUI>().text = "  " + senderName + ": " + message;
			messagePool.Add(newMessage);
			hasUsedExisted = true;
		}
		else if(messagePool.Count <= 4)
		{
			for (int i = 0; i < messagePool.Count; i++)
			{
				if (!messagePool[i].activeInHierarchy)
				{
					
					messagePool[i].SetActive(true);
					hasUsedExisted = true;
					break;
				}

			}
		}
		else 
		{
			messagePool[0].transform.GetComponentInChildren<TextMeshProUGUI>().text = messagePool[1].transform.GetComponentInChildren<TextMeshProUGUI>().text;
			messagePool[1].transform.GetComponentInChildren<TextMeshProUGUI>().text = messagePool[2].transform.GetComponentInChildren<TextMeshProUGUI>().text;
			messagePool[2].transform.GetComponentInChildren<TextMeshProUGUI>().text = messagePool[3].transform.GetComponentInChildren<TextMeshProUGUI>().text;
			messagePool[3].transform.GetComponentInChildren<TextMeshProUGUI>().text = messagePool[4].transform.GetComponentInChildren<TextMeshProUGUI>().text;
			messagePool[4].transform.GetComponentInChildren<TextMeshProUGUI>().text = "  " + senderName + ": " + message;
			hasUsedExisted = true;
		}
		if (!hasUsedExisted)
		{
			GameObject newMessage = Instantiate(messagePrefab, parent);                       
			newMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "  " + senderName + ": " + message;
			messagePool.Add(newMessage);
		}



		




	}




}
