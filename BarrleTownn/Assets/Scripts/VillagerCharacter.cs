using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerCharacter : MonoBehaviourPunCallbacks
{


	public string playerName;

	public float playerSpeed;

	public CameraController camera;

	float horiznotal;
	float vertical;
	[HideInInspector]
	public Rigidbody2D rb2D;
	private bool isFacingRight;
	private bool isFacingUp;
	public bool canMove;
	private bool isPicked;
	public float playerDistance;
	public int currentHp;
	public int Maxhp;
	public bool canHide;
	public bool isWerewolf;
	public bool isWerewolfState;
	public bool nightHide;
	public bool dayPickUp;
	public bool isVulnerable;
	public bool isAttack;
	public Vector2 faceDirection;
	public AnimatorManager animManager;
	[SerializeField]
	public WereWolf wereWolf;
	Vector3 previousGood = Vector3.zero;
	[HideInInspector]
	public Vector2 movement;
	//[SerializeField] AudioClip wolfAttack;



	public SoundManager soundManager;  

	private AudioSource audioSource;

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
	[SerializeField] Collider2D playerCollider;
	List<Projectile> projPool = new List<Projectile>();


    private void Awake()
    {
		audioSource = transform.GetComponent<AudioSource>();

	}

    public float getPlayerMovementSpeed
	{
		get
		{
			return playerSpeed + playerItems.GetShoeSpeed(isWerewolfState);
		}
	}

	[Header("bubble item related")]
	//bubble item related
	public GameObject itemBubble;
	public SpriteRenderer barrelInsideSprite;
	private float lookingleftposx = -0.598f;
	private float lookingRightRotationY = -180;
	public Sprite[] keys; // z = 0, c = 1, x = 2;


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

		SoundManager.instance.SubscribeVillger(this);

		camera = FindObjectOfType<CameraController>();
		gameObject.tag = "Player";
		dayPickUp = true;
		isAttack = false;
		isWerewolfState = false;
		rb2D = GetComponent<Rigidbody2D>();
		canMove = true;
		// playerRenderer = GetComponent<SpriteRenderer>();
		isVulnerable = true;
		ChangeWerewolfTag();
	}

	bool isHiding = false;
	public void Update()
	{
		if (photonView.IsMine)
		{
			if (currentHp <= 0)
				return;

           
            
				
			


			if (Input.GetKeyDown(KeyCode.Z))
			{
				PickUpItem();
				//PoolShoot();
			}


			MovementHandler();
			RotateItemBubble();

			if (!isWerewolfState)
			{
				PickUp();

				if (nightHide)
				{
					if (Input.GetKeyDown(KeyCode.C) && !isHiding && GetBarrleCollider().CompareTag("Pickup") && GetBarrleCollider() != null && GetBarrleCollider().GetComponent<InteractItem>().player == null)
					{
						isHiding = true;
						Hide(isHiding);
						box = GetBarrleCollider().gameObject;
					}
					else if (Input.GetKeyDown(KeyCode.C) && isHiding)
					{
						isHiding = false;
						Hide(isHiding);




					}




					Shoot();
					//&& GetBarrleCollider() != null && GetBarrleCollider().CompareTag("Pickup")
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
			ShowKeyToClick();
			ChangeWerewolfTag();
		}
	}




	public void FixedUpdate()
	{
		if (photonView.IsMine)
		{
			if (!isWerewolfState)
			{
				if (horiznotal != 0 && vertical != 0) //Diagnoal movement limited makes the movement more pleasent
				{
					horiznotal *= 0.7f;
					vertical *= 0.7f;
				}
				rb2D.MovePosition(rb2D.position + movement * getPlayerMovementSpeed * Time.deltaTime);
			}
			else
			{
				if (horiznotal != 0 && vertical != 0)
				{
					horiznotal *= 0.7f;
					vertical *= 0.7f;
				}
				wereWolf.WereWolfMovement();
			}
		}
	}


    private void OnDestroy()
    {
		SoundManager.instance.UnsubscribeVillger(this);
    }


    void ShowKeyToClick()
	{

		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, faceDirection, playerDistance);
		if (hit.collider != null && !GETIsPicked)
		{


			if (hit.collider.gameObject.layer == 13 || hit.collider.gameObject.layer == 31 && gameManager.getGamePhase == GamePhases.Day)
			{
				if (!itemBubble.activeInHierarchy)
					itemBubble.SetActive(true);
				// z button
				barrelInsideSprite.sprite = keys[0];
			}
			else if (hit.collider.gameObject.layer == 31 && gameManager.getGamePhase == GamePhases.Night)
			{
				if (!itemBubble.activeInHierarchy)
					itemBubble.SetActive(true);
				if (!isWerewolfState)
				{
					
					// c button
					barrelInsideSprite.sprite = keys[1];
				}
				else
				{
					barrelInsideSprite.sprite = keys[2];
				}
			}
		}
		else if (!GETIsPicked || isHiding)
		{
			if (itemBubble.activeInHierarchy)
			{
				itemBubble.SetActive(false);
			}
		}

	}



	void RotateItemBubble()
	{
		if (faceDirection.x == 1)
		{
			itemBubble.transform.localPosition = new Vector3(0.598f, -0.359f);
			itemBubble.transform.rotation = Quaternion.Euler(new Vector3(-180, -180, 0));
			barrelInsideSprite.gameObject.transform.localRotation = Quaternion.Euler(-180, 0, 0);
		}
		else if (faceDirection.x == -1)
		{
			itemBubble.transform.localPosition = new Vector3(-0.598f, -0.359f);
			itemBubble.transform.rotation = Quaternion.Euler(new Vector3(-180, 0, 0));
			barrelInsideSprite.gameObject.transform.localRotation = Quaternion.Euler(-180, -180, 0);
		}
	}
	public void PoolShoot(Vector2 direction)
	{
		bool canUsedExisted = false;
		if (projPool.Count == 0)
		{
			Projectile proj = PhotonNetwork.Instantiate("Projectile", this.transform.position, this.transform.rotation).GetComponent<Projectile>();
			proj.InitProjectile(this.transform, direction);
			this.projPool.Add(proj);
		}
		else
		{
			for (int i = 0; i < projPool.Count; i++)
			{
				if (!projPool[i].gameObject.activeInHierarchy)
				{
					this.projPool[i].InitProjectile(this.transform, direction);
					canUsedExisted = true;
					break;
				}
			}

			if (!canUsedExisted)
			{

				Projectile proj = PhotonNetwork.Instantiate("Projectile", this.transform.position, this.transform.rotation).GetComponent<Projectile>();
				proj.InitProjectile(this.transform, direction);
				this.projPool.Add(proj);
			}

		}
		gameManager.getPlayerItemsUI.UpdateAmmoText();
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




	//public void Flip(float horiznotal)
	//{
	//	if (horiznotal > 0 && !isFacingRight || horiznotal < 0 && isFacingRight)
	//	{
	//		isFacingRight = !isFacingRight;


	//		Vector2 scale = transform.localScale;
	//		scale.x *= -1;
	//		transform.localScale = scale;
	//	}

	//}

	public void PickUp()
	{
		if (dayPickUp)
		{
			if (Input.GetKeyDown(KeyCode.Z) && GetBarrleCollider() != null && GetBarrleCollider().CompareTag("Pickup"))
			{
				GETIsPicked = true;
				box = GetBarrleCollider().gameObject;
				box.transform.parent = this.gameObject.transform;
				box.GetComponent<InteractItem>().transfer.PickingUp();
				playerSpeed = 4 / 2;
				PlaySound("BarrelPickUp");


				if (!itemBubble.activeInHierarchy)
					itemBubble.SetActive(true);
				if (barrelInsideSprite.sprite != GetBarrleCollider().GetComponent<InteractItem>().ReturnSpriteByBarrelType())
				{
					barrelInsideSprite.sprite = GetBarrleCollider().GetComponent<InteractItem>().ReturnSpriteByBarrelType();
				}
			}
			else if (Input.GetKeyUp(KeyCode.Z) && GETIsPicked)
			{

				if (box != null)
					box.transform.parent = null;
				playerSpeed = 4;
				GETIsPicked = false;
				Unpick();
			}
		}
	}


	public void Unpick()
	{
		if (itemBubble.activeInHierarchy)
			itemBubble.SetActive(false);

		barrelInsideSprite.sprite = null;
		PlaySound("BarrelDrop");
	}

	public Collider2D GetBarrleCollider()
	{

		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, faceDirection, playerDistance);
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
		if (item.Length != 0)
		{
			int _index = gameManager.itemBank.itemList.FindIndex(x => x.itemName == item[0].gameObject.GetComponent<PickableItem>().pickableItem.itemName);
			if (this.playerItems.CanEquipItem(gameManager.itemBank.itemList[_index]))
			{
				Debug.LogError("Pickup!");

				//this.playerItems.EquipItem(gameManager.itemBank.itemList[_index]);
				if (gameManager.itemBank.itemList[_index] as GunSO)
				{
					GunSO gun = (GunSO)gameManager.itemBank.itemList[_index];
					this.playerItems.ammo = gun.ammoAmount;
				}
				photonView.RPC("RPC_EquipItem", RpcTarget.AllBuffered, _index);
				gameManager.getPlayerItemsUI.UpdatePlayerItemUI(gameManager.itemBank.itemList[_index]);
			}
		}
	}
	[PunRPC]
	void RPC_EquipItem(int index)
	{
		this.playerItems.EquipItem(gameManager.itemBank.itemList[index]);
		this.playerItems.UpdateItem(gameManager.itemBank.itemList[index]);
		this.gameManager.GetShop._reward.GetComponent<PickableItem>().VanishFromWorld();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (photonView.IsMine)
		{
			if (!isWerewolfState)
			{
				if (GetBarrleCollider() != null && GetBarrleCollider().CompareTag("Pickup") && other.gameObject.CompareTag("Pickup"))
				{
					canHide = true;
				}
			}


		}
	}



	private void OnTriggerExit2D(Collider2D other)
	{
		if (photonView.IsMine)
		{
			if (!isWerewolfState)
			{
				if (other.gameObject.CompareTag("Pickup"))
				{
					canHide = false;
				}
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

		if (isVulnerable && !isWerewolfState && !playerItems.CanDamageArmor(amount))
		{
			currentHp -= amount;
		}
		else if (isVulnerable && isWerewolfState)
		{
			currentHp -= amount;
		}

		if (this.currentHp <= 0 && this.photonView.IsMine)
		{
			this.camera.EnableSpectateMode();
		}

		GameManager.getInstance.CheckWinCondition();

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
		if (_canHide)
		{
			GetBarrleCollider().GetComponent<InteractItem>().PlayerHiding(this);
			photonView.RPC("RPC_Hide", RpcTarget.AllBufferedViaServer, _canHide);
		}
		else
		{
			if (box != null)
			{
				box.GetComponent<InteractItem>().PlayerHiding(null);
				box = null;
			}
			photonView.RPC("RPC_Hide", RpcTarget.AllBufferedViaServer, false);
		}
	}

	[PunRPC]
	public void RPC_Hide(bool _canHide)
	{
		isHiding = _canHide;
		Color tmp = playerRenderer.color;

		if (isHiding)
		{
			playerCollider.enabled = false;
			playerRenderer.enabled = false;
			GETcanMove = false;
			isVulnerable = false;
			canHide = false;
		}
		else
		{




			playerCollider.enabled = true;
			playerRenderer.enabled = true;
			GETcanMove = true;
			isVulnerable = true;
			canHide = true;

		}
		//	playerRenderer.color = tmp;


	}

	public void PlayerAppear(int id)
	{
		Hide(false);
		photonView.RPC("RPC_PlayerAppear", RpcTarget.AllBufferedViaServer, id);
	}



	[PunRPC]
	public void RPC_PlayerAppear(int id)
	{
		//Hide(false);
		GameObject _barrel = PhotonView.Find(id).gameObject;


		//Hide(false);




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


		if (canMove && gameManager.getGamePhase != GamePhases.talk && gameManager.getGamePhase != GamePhases.Vote && gameManager.getGamePhase != GamePhases.Vote_Result)
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

		if (movement != Vector2.zero)
		{
			faceDirection = movement;
		}

	}

	public void Shoot()
	{
		if (Input.GetKeyDown(KeyCode.X) && !isWerewolfState)
		{
			if (playerItems.CanShoot())
			{

				PoolShoot(faceDirection);
				photonView.RPC("RPC_PlaySound", RpcTarget.All, "CrossBowFire");

			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(wereWolf.attackPos.position, wereWolf.attackRange);


		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this.transform.position, playerPickUpRange);


		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(this.gameObject.transform.position, faceDirection * playerDistance);
	}


	#region PunRPC

	#endregion

	//[PunRPC] <<<<<<<<<<<<<<old one that worked>>>>>>>>>>>>
	//public void WolfAttackSound()
	//{
	//	AudioSource audioRPC = gameObject.AddComponent<AudioSource>();
	//	audioRPC.clip = wolfAttack;
	//	audioRPC.spatialBlend = 1;
	//	audioRPC.rolloffMode = AudioRolloffMode.Linear;
	//	audioRPC.minDistance = 5;
	//	audioRPC.maxDistance = 11;
	//	audioRPC.playOnAwake = false;
	//	audioRPC.Play();

	//}

	//Sound Area
	
	public void PlaySound(string soundName)
    {
		Sound sound = SoundManager.instance.GetSound(soundName);

		audioSource.clip = sound.clip;
		audioSource.playOnAwake = sound.playOnstart;
		audioSource.loop = sound.loop;
		audioSource.spatialBlend = sound.spatialBlend;
		audioSource.minDistance = sound.minDistance;
		audioSource.maxDistance = sound.maxDistance;
		audioSource.rolloffMode = sound.audioMode;
		audioSource.PlayOneShot(audioSource.clip);
	}

	[PunRPC]
	public void RPC_PlaySound(string soundName)
    {
		Sound sound = SoundManager.instance.GetSound(soundName);

		audioSource.clip = sound.clip;
		audioSource.playOnAwake = sound.playOnstart;
		audioSource.loop = sound.loop;
		audioSource.spatialBlend = sound.spatialBlend;
		audioSource.minDistance = sound.minDistance;
		audioSource.maxDistance = sound.maxDistance;
		audioSource.rolloffMode = sound.audioMode;
		audioSource.PlayOneShot(audioSource.clip);

		//FindObjectOfType<SoundManager>().Play("wolfAttack");
		//soundManager.Play("wolfAttack");

	}


	public  void ItemCreated()
    {
		
		
        if ( photonView.IsMine)
        {
			PlaySound("ItemReady");

		}

    }
	
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
	public float werewolfSpeed;
	public float attackStun;
	public bool isStunned;
	public void WereWolfMovement()
	{

		player.rb2D.MovePosition(player.rb2D.position + player.movement * werewolfSpeed * Time.deltaTime);

	}

	public void WerewolfAttack()
	{
		enemy = (1 << 10) | (1 << 31) | (1 << 12);

		if (Input.GetKeyDown(KeyCode.X) && player.isWerewolfState && !player.isAttack)
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
			player.isAttack = true;
			if (player.isWerewolfState && player.isAttack)
			{

				player.animManager.WerewolfAttack();
				player.photonView.RPC("RPC_PlaySound", RpcTarget.All,"WolfAttack");
				player.StartCoroutine(DelayAfterAttack());

				

			}
		}
	}

	

	IEnumerator DelayAfterAttack()
	{

		player.canMove = false;
		yield return new WaitForSeconds(attackStun);
		player.canMove = true;
		player.isAttack = false;
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