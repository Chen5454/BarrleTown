using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class NotificationSystem : MonoBehaviour
{
	List<string> notification = new List<string>();
	public TextMeshProUGUI notificationText;
	public bool textShowed;
	public float textStartTimer;
	public float Timer;
	bool alphaText;
	public float alphaMultiplier;
	float alpha;
	// Start is called before the first frame update
	void Start()
	{
		alphaText = false;
		notificationText.alpha = 0;
	}


	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.G))
		//{
		//	ShowText("WereWolf Has Died");
		//	ShowText("Completed Recipe: Simple Gun");
		//	ShowText("Hello World!");
		//}


		if (notification.Count != 0)
		{
			if (!textShowed)
			{
				textShowed = true;
				Timer = textStartTimer;
				notificationText.text = notification[0];
				alphaText = false;
				notification.RemoveAt(0);

				//StartCoroutine(showText());
			}
		}
		else if (!textShowed && notification.Count == 0)
		{
			if (notificationText.text != "")
			{
				notificationText.text = "";
			}
		}


		if (textShowed)
		{
			if (Timer > 0)
			{
				
					Timer -= Time.deltaTime;
				
				//0.5f <    1/4 = 0.25f
				if (Timer < textStartTimer / 4)
				{
					alphaText = true;
				}



			}
			else if (Timer < 0)
			{
				textShowed = false;
			}
		}


		if (alphaText)
		{
			if (alpha > 0)
				alpha -= Time.deltaTime * alphaMultiplier;
		}
		else
		{
			if (alpha < 1)
				alpha += Time.deltaTime * alphaMultiplier;
		}
		if (notificationText.alpha != alpha)
			notificationText.alpha = alpha;



	}

	IEnumerator showText()
	{
		alphaText = false;

		yield return new WaitForSeconds(textStartTimer);


		if (notification.Count != 0)
			StartCoroutine(showText());
		else
		{
			textShowed = false;
		}

	}
	public void ShowText(string text)
	{
		if (!textShowed)
		{
			alphaText = true;
			alpha = 0;
		}
		notification.Add(text);
	}



}
