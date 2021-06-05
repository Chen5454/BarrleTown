using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class ChatUI : MonoBehaviourPun
{
	public GameObject chat;
	public GameObject messagePrefab;
	public Transform parent;

	private string playerMessage;
	[SerializeField] TMP_InputField inputField;

	[SerializeField] List<GameObject> messagePool = new List<GameObject>();

	public void SetChatVisibility(bool _setActive)
	{
		chat.SetActive(_setActive);


		if (!_setActive)
		{
			for (int i = 0; i < messagePool.Count; i++)
			{
				messagePool[i].SetActive(false);
			}
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
		else
		{
			for (int i = 0; i < messagePool.Count; i++)
			{
				if (!messagePool[i].activeInHierarchy)
				{
					messagePool[i].GetComponentInChildren<TextMeshProUGUI>().text = "  " + senderName + ": " + message;
					messagePool[i].SetActive(true);
					hasUsedExisted = true;
					break;
				}

			}

		}
		if (!hasUsedExisted)
		{
			GameObject newMessage = Instantiate(messagePrefab, parent);                       
			newMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "  " + senderName + ": " + message;
			messagePool.Add(newMessage);
		}



		




	}




}
