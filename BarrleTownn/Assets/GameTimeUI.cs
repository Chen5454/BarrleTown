﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameTimeUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public bool isTimerShown;

	GamePhases phase => GameManager.getInstance.getGamePhase;



	void Update()
    {
		if (isTimerShown)
		{
			if (!timerText.gameObject.activeInHierarchy)
			{
				timerText.gameObject.SetActive(true);
			}


			timerText.text = "Time: " +  Mathf.Round(GameManager.getInstance.getTimer) + "      Untill: " + GetNextPhase().ToString();
		}
		else
		{
			if (timerText.gameObject.activeInHierarchy)
			{
				timerText.gameObject.SetActive(false);
			}
		}
			
    }


	GamePhases GetNextPhase()
	{
		int phaseIndex = (int)GameManager.getInstance.getGamePhase;
		int phaseLenght = Enum.GetValues(typeof(GamePhases)).Length;

		if (phaseIndex >= phaseLenght - 1) 
		{
			phaseIndex = 0;
		}
		else
		{
			Debug.Log("sssss");
			return (GamePhases)phaseIndex+ 1;
		}
		Debug.Log("ddddd");
		return (GamePhases)phaseIndex;
	}



}
