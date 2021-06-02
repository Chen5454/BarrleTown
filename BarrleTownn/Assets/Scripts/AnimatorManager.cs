using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AnimatorManager : MonoBehaviourPunCallbacks
{
    public Animator animator;
    
     VillagerCharacter player;
     WereWolfCharacter wereWolf;

    private void Awake()
    {
        player = GetComponent<VillagerCharacter>();
        wereWolf = GetComponent<WereWolfCharacter>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Player();
            //WerewolfAttack();
        }
    }

    public void Player()
    {
        animator.SetFloat("horizontal", player.movement.x);
        animator.SetFloat("vertical", player.movement.y);
        animator.SetFloat("Speed", player.movement.sqrMagnitude);
        if (Input.GetAxisRaw("Horizontal")==1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("LastHorizontal",Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
        }
    }

    
    


    public void WerewolfAttack()  
    {
        animator.SetTrigger("attack");
    }



    private IEnumerator WaitAndAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

    }

}
