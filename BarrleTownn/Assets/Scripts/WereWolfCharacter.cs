using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolfCharacter : MonoBehaviour
{
    public AnimatorManager animator;
    public Rigidbody2D rb2DWereWolf;
    
    public  void Update()
    {
        Attack();
    }

    public  void FixedUpdate()
    {
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.WerewolfAttack();
        }
    }
    

}
