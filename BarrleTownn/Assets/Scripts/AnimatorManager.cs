using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AnimatorManager : MonoBehaviourPunCallbacks
{
    public Animator animator;
     VillagerCharacter player;
    private void Awake()
    {
        player = GetComponent<VillagerCharacter>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Player();
            WereWolf();
            PlayerDeadAnimation();
            WereWolfDeadAnimation();
            WerewolfAttack();
        }
    }

    public void PlayerDeadAnimation()
    {
        if (!player.isWerewolfState)
        {
            if (player.currentHp == 0)
            {
                player.canMove = false;
                animator.SetBool("isDead",true);
            }
        }
    }

    public void WereWolfDeadAnimation()
    {
        if (player.isWerewolfState)
        {
            if (player.currentHp == 0)
            {
                player.canMove = false;
                animator.SetBool("isWolfDead", true);
            }
        }
    }

    public void Player()
    {
        if (!player.isWerewolfState)
        {
            animator.SetBool("isHuman",true);
            animator.SetBool("isWolf",false);
            animator.SetFloat("horizontal", player.movement.x);
            animator.SetFloat("vertical", player.movement.y);
            animator.SetFloat("Speed", player.movement.sqrMagnitude);
            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 ||
                Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
            }
        }
     
    }
    public void WereWolf()
    {
        if (player.isWerewolfState)
        {
            animator.SetBool("isWolf",true);
            animator.SetBool("isHuman",false);
            animator.SetFloat("horizontal", player.movement.x);
            animator.SetFloat("vertical", player.movement.y);
            animator.SetFloat("Speed", player.movement.sqrMagnitude);
            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 ||
                Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
            }
        }
       
    }
    public void WerewolfAttack()  
    {
        if (player.isWerewolfState)
        {


            if (player.isAttack)
            {
                animator.SetTrigger("attack");
                player.isAttack = false;
            }
            
        }
    }

    private IEnumerator WaitAndAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

    }

}
