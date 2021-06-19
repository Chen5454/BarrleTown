using Photon.Pun;
using UnityEngine;

public class VillagerCharacter : MonoBehaviourPunCallbacks
{

	public float speed;

	public float getPlayerMovementSpeed
	{
		get
		{
			return speed + playerItems.GetShoeSpeed();
		}
	}

	float horiznotal;
	float vertical;
	[HideInInspector]
	public Rigidbody2D rb2D;
	private bool isFacingRight;
	private bool isFacingUp;
	public bool canMove;
	private bool isPicked;
	public float distance;
	public int currentHp;
	public int Maxhp;
	public bool canHide;
	public bool isWerewolf;
	public bool isWerewolfState;

	[SerializeField]
	public WereWolf wereWolf;

	[HideInInspector]
	public Vector2 movement;

	public InteractItem hidebehind;
	public SpriteRenderer playeRenderer;
	private GameManager gameManager;
	public GameObject box;
	UIManager uiManager;

	[Header("Player Items")]
	[SerializeField]PlayerItems playerItems;

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
		playerItems = new PlayerItems();

		gameObject.tag = "Player";
		isWerewolfState = false;
		rb2D = GetComponent<Rigidbody2D>();
		canMove = true;
		ChangeWerewolfTag();
	}

	public void Update()
	{
		if (photonView.IsMine)
		{
			MovementHandler();
			if (!isWerewolf)
			{
				PickUp();
				Hide();
			}
			else
			{
				// didn't transformed
				if (!isWerewolfState)
					PickUp();

				//if (Input.GetKeyDown(KeyCode.T))
				//	wereWolf.Transform();

				//transformed
				if (isWerewolfState)
					wereWolf.WerewolfAttack();
			}
		}
	}

	public void ChangeWerewolfTag()
	{

			if (this.isWerewolfState)
			{
			if(this.gameObject.tag != "Werewolf")
				this.gameObject.tag = "Werewolf";
			}
			else if (!this.isWerewolfState)
			{
			if (this.gameObject.tag != "Player")
				this.gameObject.tag = "Player";
			}
		
	}

	public void FixedUpdate()
	{
		if (photonView.IsMine)
		{
			if (horiznotal != 0 && vertical != 0) //Diagnoal movement limited makes the movement more pleasent
			{
				horiznotal *= 0.7f;
				vertical *= 0.7f;
			}
			rb2D.MovePosition(rb2D.position + movement * getPlayerMovementSpeed * Time.deltaTime);

			Flip(horiznotal);
		}


		ChangeWerewolfTag();



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

	}

	public void PickUp()
	{

		if (Input.GetKeyDown(KeyCode.Q) && GetBarrleCollider() != null && GetBarrleCollider().CompareTag("Pickup"))
		{
			GETIsPicked = true;
			box = GetBarrleCollider().gameObject;
			box.transform.parent = this.gameObject.transform;
			box.GetComponent<InteractItem>().transfer.PickingUp();
			speed = speed / 2;
		}


		else if (Input.GetKeyUp(KeyCode.Q) && GETIsPicked)
		{
			if (box != null)
				box.transform.parent = null;
			speed = speed * 2;
			GETIsPicked = false;

		}
	}


	public Collider2D GetBarrleCollider()
	{
		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, distance);
		return hit.collider;

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (GetBarrleCollider() != null && GetBarrleCollider().CompareTag("Pickup"))
		{
			canHide = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (GetBarrleCollider())
		{
			canHide = false;
		}
	}


	public void Hide()
	{
		if (canHide)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				hidebehind.spriteRenderer.sortingOrder = 2;
				playeRenderer.enabled = false;
				canMove = false;
				hidebehind.transfer.PickingUp();
				canHide = false;
			}


		}

		else if (!canHide)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{

				hidebehind.spriteRenderer.sortingOrder = 0;
				canMove = true;
				playeRenderer.enabled = true;
			}
		}

	}

	public void GetDamage(int amount)
	{
		photonView.RPC("RPC_GetDamage", RpcTarget.AllBufferedViaServer, amount);
	}


	[PunRPC]
	public void RPC_GetDamage(int amount)
	{
		if(!playerItems.CanDamageArmor(amount))
			currentHp -= amount;

	}

	public void Hp()
	{
		if (currentHp > Maxhp)
		{
			currentHp = Maxhp;
		}
	}

	public void MovementHandler()
	{
		if (canMove)
		{
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
		}
		else
		{
			movement.x = 0;
			movement.y = 0;
			canMove = false;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(wereWolf.attackPos.position, wereWolf.attackRange);
	}


	#region PunRPC

	#endregion

}
[System.Serializable]
public class WereWolf
{

	[SerializeField]
	public Rigidbody2D rb2DWereWolf;
	public VillagerCharacter player;


	public Transform attackPos;
	public float attackRange;
	public LayerMask enemy;

	public void WerewolfAttack()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl) && player.isWerewolfState)
		{
			Collider2D[] enemiestoDmg = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

			for (int i = 0; i < enemiestoDmg.Length; i++)
			{
				if (enemiestoDmg[i].transform.tag == "Player")
				{
					enemiestoDmg[i].GetComponent<VillagerCharacter>().GetDamage(1);
				}
			}
		}
	}


	public void Transform()
	{
		if (player.isWerewolf)
		{
			if (!player.isWerewolfState)
			{
				player.isWerewolfState = true;
			}
			else
			{
				if (player.isWerewolfState)
				{
					player.isWerewolfState = false;
				}
			}
		}


	}
}