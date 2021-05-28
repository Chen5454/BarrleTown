using Photon.Pun;
using UnityEngine;

public class VillagerCharacter : MonoBehaviourPunCallbacks
{

	public float speed;
	float horiznotal;
	float vertical;
	private Rigidbody2D rb2D;
	private bool isFacingRight;
	private bool isFacingUp;
	public bool canMove;
	private bool isPicked;
	public float distance;
	public int currentHp;
	public int Maxhp;
	[HideInInspector]
	public Vector2 movement;

	public InteractItem hidebehind;
	public SpriteRenderer playeRenderer;
	private GameManager gameManager;
	public GameObject box;
	//public Animator animator;
	UIManager uiManager; 

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
		canMove = true;
		
	}

	public virtual void Update()
	{
		if (photonView.IsMine)
		{
			MovementHandler();
			PickUp();





		}


	}



	public virtual void FixedUpdate()
	{
		if (photonView.IsMine)
		{


			if (horiznotal != 0 && vertical != 0) //Diagnoal movement limited makes the movement more pleasent
			{
				horiznotal *= 0.7f;
				vertical *= 0.7f;
			}

			rb2D.MovePosition(rb2D.position + movement * speed * Time.deltaTime);

			Flip(horiznotal);
		}
	}



	public void Flip(float horiznotal)
	{
		if (horiznotal > 0 && !isFacingRight || horiznotal < 0 && isFacingRight)
		{
			isFacingRight = !isFacingRight;


			Vector2 scale = transform.localScale;
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

	public void PickUp()
	{
		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, distance);



		if (Input.GetKeyDown(KeyCode.Q) && hit.collider != null && hit.collider.CompareTag("Pickup"))
		{

			GETIsPicked = true;
			box = hit.collider.gameObject;
			box.transform.parent = this.gameObject.transform;
			box.GetComponent<InteractItem>().transfer.PickingUp();
			speed = speed / 2;
		}


		else if (Input.GetKeyUp(KeyCode.Q) && GETIsPicked)
		{
			box.transform.parent = null;
			speed = speed * 2;
			GETIsPicked = false;

		}
	}




	public virtual void GetDamage(int amount)
	{

		currentHp -= amount;

	}

	public virtual void Hp()
	{

		if (currentHp > Maxhp)
		{
			currentHp = Maxhp;
		}
	}

	//private void OnDrawGizmos()  //to see the RayCast
	//{
	//    Gizmos.color = Color.yellow;
	//    Gizmos.DrawLine(transform.position,movement * distance);
	//}

	public virtual void MovementHandler()
	{
		if (canMove)
		{
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			//animator.SetFloat("horizontal",movement.x);
			//animator.SetFloat("vertical",movement.y);
			//animator.SetFloat("Speed",movement.sqrMagnitude);
		}
	}





	#region PunRPC

	#endregion





}
