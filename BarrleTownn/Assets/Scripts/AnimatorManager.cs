using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
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
        Player();
        //WerewolfAttack();
    }

    public void Player()
    {
        animator.SetFloat("horizontal", player.movement.x);
        animator.SetFloat("vertical", player.movement.y);
        animator.SetFloat("Speed", player.movement.sqrMagnitude);
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
