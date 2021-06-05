using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatUI : MonoBehaviour
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
	}





	public void OnChangeMessageValue(string _message)
	{
		playerMessage = _message;
	}


	public void OnClickSendButt()
	{
		if (playerMessage != "" && playerMessage != null)
		{
			GameManager.getInstance.SendMessage(playerMessage);
		}
		else
		{
			Debug.Log("cannot send an empty object");
		}
	}

	//put this function in a pun
	public void SendMessage(string message)
	{

		bool hasUsedExisted = false;
		if (messagePool.Count == 0)
		{
			GameObject newMessage = Instantiate(messagePrefab, parent);                       // 2 spaces
			newMessage.transform.GetComponentInChildren<TextMeshProUGUI>().text = "  " + "SilverPoop: " + message;
			messagePool.Add(newMessage);
			hasUsedExisted = true;
		}
		else
		{
			for (int i = 0; i < messagePool.Count; i++)
			{
				if (!messagePool[i].activeInHierarchy)
				{
					messagePool[i].GetComponentInChildren<TextMeshProUGUI>().text = "  " + "SilverPoop: " + message;
					messagePool[i].SetActive(true);
					hasUsedExisted = true;
					break;
				}

			}

		}
		if (!hasUsedExisted)
		{
			GameObject newMessage = Instantiate(messagePrefab, parent);                       // 2 spaces
			newMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "  " + "SilverPoop: " + message;
			messagePool.Add(newMessage);
		}



		playerMessage = "";
		inputField.text = playerMessage;




	}




}
