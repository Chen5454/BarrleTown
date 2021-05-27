using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolfCharacter : VillagerCharacter
{
    public AnimatorManager animator;
    public Rigidbody2D rb2DWereWolf;
    
    public override void Update()
    {
        base.Update();
        Attack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.WerewolfAttack();
        }
    }
    

}
