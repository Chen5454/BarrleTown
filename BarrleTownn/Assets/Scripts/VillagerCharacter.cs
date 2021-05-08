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

    public GameObject box;

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

        rb2D.velocity = new Vector2(horiznotal * speed, vertical * speed);

        Flip(horiznotal,vertical);

    }



    public void Flip(float horiznotal,float vertical)
    {
        if (horiznotal > 0 && !isFacingRight || horiznotal < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;

            Vector3 scale = transform.localScale;

            scale.x *= -1;
            transform.localScale = scale;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale,distance);

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

    private void OnDrawGizmos()  //to see the RayCast
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale * distance);
    }

    public virtual void MovementHandler()
    {
        horiznotal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }


}
