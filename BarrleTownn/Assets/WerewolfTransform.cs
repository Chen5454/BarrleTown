using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WerewolfTransform : MonoBehaviourPunCallbacks
{
    public GameObject villager;
    public GameObject wereWolf;
    //public bool isWerewolf;
    public int formSwitch;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            villager.SetActive(true);
            wereWolf.SetActive(false);
            formSwitch = 1;
        }
      
    }

    //public bool getIsWereWolf
    //{
    //    set
    //    {
    //        if (isWerewolf != value)
    //        {
    //            isWerewolf = value;
    //            villager.SetActive(false);
    //            wereWolf.SetActive(true);

    //        }
    //        else 
    //        {
    //            villager.SetActive(true);
    //            wereWolf.SetActive(false);
    //        }
    //    }

    //    get
    //    {
    //        return isWerewolf;
    //    }
    //}


    private void Update()
    {
        if (photonView.IsMine)
        {
            Transformation();

        }
    }

    public void Transformation()
    {
        if (Input.GetKeyDown(KeyCode.T) && formSwitch == 1)
        {

            villager.SetActive(false);
            wereWolf.SetActive(true);
            formSwitch = 2;
            wereWolf.transform.position = villager.transform.position;
            wereWolf.transform.rotation = villager.transform.rotation;
        }

        else if (Input.GetKeyDown(KeyCode.T) && formSwitch == 2)
        {

            villager.SetActive(true);
            wereWolf.SetActive(false);
            formSwitch = 1;
            villager.transform.position = wereWolf.transform.position;
            villager.transform.rotation = wereWolf.transform.rotation;


        }
    }
}
