using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerCharacter : MonoBehaviour
{
    public float speed;
    float horiznotal;
    float vertical;
    Rigidbody2D rb2D;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        MovementHandler();

    }


    public virtual void FixedUpdate()
    {
        if (horiznotal!=0 && vertical!=0) //Diagnoal movement limited makes the movement more pleasent
        {
            horiznotal *= 0.7f;
            vertical *= 0.7f;
        }
        rb2D.velocity = new Vector2(horiznotal, vertical * speed);
    }



    public virtual void MovementHandler()
    {
        horiznotal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }


}
