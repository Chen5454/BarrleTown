using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerCharacter : MonoBehaviour
{

    public float speed;
    float horiznotal;
    float vertical;
    Rigidbody2D rb2D;
    private bool isFacingRight;
    private bool isFacingUp;
    public CollectItem item;
    private bool isPicked;
    public float distance;
    public int currentHp;
    public int Maxhp;
    private Vector2 movement;

    public GameObject box;
    public Animator animator;
    public bool GETIsPicked
    {
        set
        {
            if (isPicked != value)
            {
                isPicked = value;
            }
        }
        get
        {
            return isPicked;
        }
    }

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        MovementHandler();
        PickUp();
    }


    public virtual void FixedUpdate()
    {
        if (horiznotal != 0 && vertical != 0) //Diagnoal movement limited makes the movement more pleasent
        {
            horiznotal *= 0.7f;
            vertical *= 0.7f;
        }

        rb2D.MovePosition(rb2D.position+movement*speed*Time.deltaTime);
        Flip(horiznotal,vertical);

    }



    public void Flip(float horiznotal,float vertical)
    {
        if (horiznotal > 0 && !isFacingRight || horiznotal < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;


            //Vector2 scale = transform.localScale;

            //scale.x *= -1;
            //transform.localScale = scale;
        }




        //if (vertical > 0 &&!isFacingUp || vertical<0 && isFacingUp)
        //{
        //    isFacingUp = !isFacingUp;

        //    Vector3 scale = transform.localScale;
        //    scale.y *= -1;
        //    transform.localScale = scale;
        //}
    }

    public virtual void PickUp()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,movement,distance);

        if (Input.GetKeyDown(KeyCode.Q) && hit.collider != null)
        {
            GETIsPicked = true;
            box = hit.collider.gameObject;
           box.transform.parent = gameObject.transform;
           speed = speed / 2;
        }
        else if (Input.GetKeyUp(KeyCode.Q)&& GETIsPicked)
        {
            box.transform.parent = null;
            speed = speed * 2;
            GETIsPicked = false;

        }
    }


    public virtual void GetDamage(int amount)
    {


    }

    public virtual void Hp()
    {
        currentHp = Maxhp;
    }

    private void OnDrawGizmos()  //to see the RayCast
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,movement * distance);
    }

    public virtual void MovementHandler()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("horizontal",movement.x);
        animator.SetFloat("vertical",movement.y);
        animator.SetFloat("Speed",movement.sqrMagnitude);



    }


}
