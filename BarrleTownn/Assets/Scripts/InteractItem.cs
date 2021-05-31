using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Photon.Pun;
public class InteractItem : MonoBehaviourPunCallbacks
{
    public RecipeItems contain;

    public VillagerCharacter player;
    //public bool canHide;
    public SpriteRenderer spriteRenderer;
    public OwnerShipTranfer transfer;



    //private void OnCollisionStay2D(Collision2D other)
    //{
    //    if (other.collider.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
    //    {
    //        canHide = true;
    //        spriteRenderer.sortingOrder = 2;
    //        player.playeRenderer.enabled = false;
    //        //player.transform.position = gameObject.transform.position;
    //        player.canMove = false;
    //        transfer.PickingUp();
    //    }
    //    else if (Input.GetKeyUp(KeyCode.E))
    //    {
    //        canHide = false;
    //        spriteRenderer.sortingOrder = 0;
    //        player.canMove = true;
    //        player.playeRenderer.enabled = true;
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        canHide = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        canHide = false;
    //    }
    //}
}
