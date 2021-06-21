﻿using Photon.Pun;
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
	public bool nightHide;
	public bool dayPickUp;
	public bool isVulnerable;
	[SerializeField]
	public WereWolf wereWolf;

	[HideInInspector]
	public Vector2 movement;

	public InteractItem hidebehind;

	private Shop shop;
	public SpriteRenderer playerRenderer;
	private GameManager gameManager => GameManager.getInstance;
	public GameObject box;
	UIManager uiManager;

	[Header("Player Items")]
	[SerializeField] PlayerItems playerItems;
	public PlayerItems getPlayerItems => playerItems;
	[SerializeField] Vector2 playerPickUpRange;
	[SerializeField] LayerMask ItemLayers;



	#region Getters Setters
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


	public bool GETcanMove
	{

		set
		{
			if (canMove != value)
			{
				canMove = value;
			}
		}

		get { return canMove; }
	}

	#endregion

	private void Start()
	{

		gameObject.tag = "Player";
		dayPickUp = true;
		isWerewolfState = false;
		rb2D = GetComponent<Rigidbody2D>();
		canMove = true;
		// playerRenderer = GetComponent<SpriteRenderer>();
		isVulnerable = true;
		ChangeWerewolfTag();
	}


	public void Update()
	{
		if (photonView.IsMine)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				PickUpItem();
			}
			MovementHandler();
			if (!isWerewolfState)
			{
				PickUp();

				if (nightHide)
				{
					if (Input.GetKeyDown(KeyCode.E) && GetBarrleCollider() != null)
					{
						Hide(canHide);
					}

				}




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

			ChangeWerewolfTag();
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
		}
	}



	public void ChangeWerewolfTag()
	{
		if (this.isWerewolfState)
		{
			if (this.gameObject.tag != "Werewolf")
				this.gameObject.tag = "Werewolf";
		}
		else if (!this.isWerewolfState)
		{
			if (this.gameObject.tag != "Player")
				this.gameObject.tag = "Player";
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

	}

	public void PickUp()
	{
		if (dayPickUp)
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
	}


	public Collider2D GetBarrleCollider()
	{

		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, distance);
		if (hit.collider != null && hit.collider.gameObject.CompareTag("Pickup"))
		{
			return hit.collider;
		}
		else
		{
			return null;
		}


	}



	public void PickUpItem()
	{
		Collider2D[] item = Physics2D.OverlapBoxAll(this.transform.position, playerPickUpRange, 0, ItemLayers);
		if(item.Length != 0)
		{
			int _index = gameManager.itemBank.itemList.FindIndex(x => x.itemName == item[0].gameObject.GetComponent<PickableItem>().pickableItem.itemName);
			if (playerItems.CanEquipItem(gameManager.itemBank.itemList[_index]))
			{
				playerItems.EquipItem(gameManager.itemBank.itemList[_index]);
				photonView.RPC("RPC_EquipItem", RpcTarget.AllBuffered, _index);
				gameManager.getPlayerItemsUI.UpdatePlayerItemUI(gameManager.itemBank.itemList[_index]);
			}
		}
	}
	[PunRPC]
	void RPC_EquipItem(int index)
	{
		playerItems.EquipItem(gameManager.itemBank.itemList[index]);
		gameManager.GetShop._reward.GetComponent<PickableItem>().VanishFromWorld();
		
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (photonView.IsMine)
		{
			if (GetBarrleCollider() != null && GetBarrleCollider().CompareTag("Pickup"))
			{
				canHide = true;
			}
		}
	}



	private void OnTriggerExit2D(Collider2D other)
	{
		if (photonView.IsMine)
		{
			if (other.gameObject.CompareTag("Pickup"))
			{
				canHide = false;
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

		if (isVulnerable && !playerItems.CanDamageArmor(amount))
		{
			currentHp -= amount;
		}
	}


	public void SetWereWolfHP(int amount, bool setMax)
	{
		photonView.RPC("RPC_SetWereWolfHP", RpcTarget.AllBuffered, amount, setMax);
	}
	[PunRPC]
	public void RPC_SetWereWolfHP(int amount, bool setMax)
	{
		if (setMax)
		{
			Maxhp = amount;
			currentHp = amount;
		}
		else
		{
			currentHp += amount;
			if (currentHp > Maxhp)
				currentHp = Maxhp;
		}
	}

	public void Hide(bool _canHide)
	{

		photonView.RPC("RPC_Hide", RpcTarget.AllBufferedViaServer, _canHide);

	}

	[PunRPC]
	public void RPC_Hide(bool _canHide)
	{
		canHide = _canHide;

		if (canHide)
		{
			playerRenderer.enabled = false;
			GETcanMove = false;
			isVulnerable = false;
			canHide = false;
		}
		else
		{
			GETcanMove = true;
			playerRenderer.enabled = true;
			isVulnerable = true;
			canHide = true;

		}


	}

	public void PlayerAppear(int id)
	{
		photonView.RPC("RPC_PlayerAppear", RpcTarget.AllBufferedViaServer, id);
	}



	[PunRPC]
	public void RPC_PlayerAppear(int id)
	{
		Hide(false);
		GameObject _barrel = PhotonView.Find(id).gameObject;
		_barrel.GetComponent<InteractItem>().DestoryBerrel();

		//canHide = true;
		//GETcanMove = true;
		//playerRenderer.enabled = true;
		//isVulnerable = true;

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


		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this.transform.position, playerPickUpRange);


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
		enemy = (1 << 10) | (1 << 31) | (1 << 12);

		if (Input.GetKeyDown(KeyCode.LeftControl) && player.isWerewolfState)
		{

			Collider2D[] enemiestoDmg = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

			for (int i = 0; i < enemiestoDmg.Length; i++)
			{
				if (enemiestoDmg[i].transform.tag == "Player")
				{
					enemiestoDmg[i].GetComponent<VillagerCharacter>().GetDamage(1);
				}
				else if (enemiestoDmg[i].transform.tag == "Pickup")
				{
					enemiestoDmg[i].GetComponent<InteractItem>().BerrelGetDamage(1);
				}
				else if (enemiestoDmg[i].transform.tag == "ShopDoor")
				{
					GameManager.getInstance.GetShop.DamageDoor(1);
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