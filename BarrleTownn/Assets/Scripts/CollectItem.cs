using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public VillagerCharacter player;

    public Transform ItemLocation;
    public Transform currentItem;

    public bool isBarrel;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isBarrel)
        {
            if (other.collider.CompareTag("Player") && player.GETIsPicked)
            {
                transform.parent = player.gameObject.transform;
            }
        }
    }

    

}
