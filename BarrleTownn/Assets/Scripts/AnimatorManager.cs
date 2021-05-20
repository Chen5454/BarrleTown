using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    public VillagerCharacter player;

    private void Update()
    {
        animator.SetFloat("horizontal", player.movement.x);
        animator.SetFloat("vertical", player.movement.y);
        animator.SetFloat("Speed", player.movement.sqrMagnitude);
    }



    private IEnumerator WaitAndAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

    }

}
