using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class InteractItem : MonoBehaviour
{
    public RecipeItems contain;

    public VillagerCharacter player;
    public bool canHide;
    public bool barrel;
    public bool bush;
    public SpriteRenderer spriteRenderer;

    private void OnCollisionStay2D(Collision2D other)
    {
        if (barrel)
        {
            if (other.collider.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                canHide = true;
                spriteRenderer.sortingOrder = 2;
                player.playeRenderer.enabled = false;
                //player.transform.position = gameObject.transform.position;
                player.canMove = false;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                canHide = false;
                spriteRenderer.sortingOrder = 0;
                player.canMove = true;
                player.playeRenderer.enabled = true;
            }
        }
    }

    


    
}
