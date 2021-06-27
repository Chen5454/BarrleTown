using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Shop : MonoBehaviourPunCallbacks
{

	[Header("Drop site Related")]
	public Transform dropSite;
	public Vector2 dropSiteRadius;
	public LayerMask barrelMask;
	public LayerMask playerMask;

	[Header("Recipe")]
	public ShopRecipe shopRecipe;
	public Recipe currentRecipe;
	public Recipe amountRequired;
	public List<InteractItem> itemInside;

	[Header("Reward Related")]
	public Transform rewardSpawnPosition;

	[Header("references")]
	public UIManager uiManager;
	public ItemBankSO itemBank;
	public GameObject _reward;
	[SerializeField] GameObject shopDoor;
	public NotificationSystem notification;
	//public SpriteRenderer[] recipeItemShow;
	//public Color[] tempColor; //for now the item sprites will be barrels with different color, metal = gray, wood = brown, leather = orange;
	//public Sprite[] amountSprites;
	[Header("Debug")]
	public int doorStartHP;
	public int doorHP;
	int playersInsideShopRegion;
	int barrelsInsideShopRegion;
	public bool canGetReward;
	public bool canGenerateNewRecipe;
	Collider2D[] insidePlayers;
	[SerializeField] Transform kickFromShopPlace;
	private void Awake()
	{
		_reward.SetActive(false);
		canGetReward = true;
		doorHP = doorStartHP;
	}

	void Update()
	{
		Collider2D[] _insideBarrels = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius, 0, barrelMask);
		//Debug.Log(_insideBarrels.Length);
		insidePlayers = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius, 0, playerMask);
		//barrels
		if (_insideBarrels.Length > 0)
		{

			if (barrelsInsideShopRegion != _insideBarrels.Length)
			{
				barrelsInsideShopRegion = _insideBarrels.Length;
				if (PhotonNetwork.IsMasterClient)
				{
					CheckDropSite();
					//GameManager.getInstance.CheckIfRecipeCompleted();
					//GameManager.getInstance.CheckIfRecipeCompleted();photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);

				}
				CheckIfRecipeCompleted();
			}

		}
		else
		{
			barrelsInsideShopRegion = 0;
		}

		//players
		if (insidePlayers.Length > 0)
		{
			if (playersInsideShopRegion != insidePlayers.Length)
			{
				playersInsideShopRegion = insidePlayers.Length;

				CheckDropSite();
				//GameManager.getInstance.CheckIfRecipeCompleted();
				//photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);
				CheckIfRecipeCompleted();
				int playerIndex = -1;
				for (int i = 0; i < insidePlayers.Length; i++)
				{
					if (insidePlayers[i].gameObject.GetPhotonView().IsMine)
					{
						playerIndex = i;
					}
				}

				if (playerIndex != -1)
				{
					uiManager.shop.ShowRecipePanel(true);
				}
				else
				{
					uiManager.shop.ShowRecipePanel(false);
				}

			}
		}
		else
		{
			if (uiManager.shop.recipeUI.activeInHierarchy)
				uiManager.shop.ShowRecipePanel(false);
			playersInsideShopRegion = 0;
		}


		if (canGenerateNewRecipe)
			if (PhotonNetwork.IsMasterClient)
				GenerateNewRecipe();

	}


	public void SetDoor(bool _setActive)
	{
		if (insidePlayers.Length > 0)
		{
			shopDoor.SetActive(false);
		}
		else
			shopDoor.SetActive(_setActive);
	}
	void checkIfDoorDestroyed()
	{
		if (doorHP <= 0)
		{
			GameManager.getInstance.SetShopDoorActive(false);
		}
	}
	public void DamageDoor(int damage)
	{
		doorHP -= damage;
		checkIfDoorDestroyed();
	}
	public void GenerateNewRecipe()
	{
		canGenerateNewRecipe = false;
		int randomizer = UnityEngine.Random.Range(0, shopRecipe.RecipeList.Count);
		if (PhotonNetwork.IsMasterClient)
		{
			GameManager.getInstance.ShowRecipeToAll(randomizer);
		}
		//currentRecipe = shopRecipe.RecipeList[randomizer];
		//canGenerateNewRecipe = false;
	}

	public void GetNewGeneratedRecipeIndex(int _index)
	{
	
		currentRecipe = shopRecipe.RecipeList[_index];
		Debug.LogError(" the new recipe is for: " + currentRecipe.recipeReward.itemName);
		canGenerateNewRecipe = false;
		notification.ShowText("New recipe is: " + currentRecipe.recipeReward.itemName);
	}

	public void GenerateNewShopRecipe()
	{
		canGetReward = true;
	}


	public void CheckDropSite()
	{
		itemInside = new List<InteractItem>();
		Collider2D[] insideBarrels = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius, 0, barrelMask);
		for (int i = 0; i < insideBarrels.Length; i++)
		{
			if (!insideBarrels[i].isTrigger)
				itemInside.Add(insideBarrels[i].GetComponent<InteractItem>());
		}


	}
	public void CheckIfRecipeCompleted()
	{
		int correctAmountOfItems = 0;

		amountRequired = currentRecipe;
		int[] test = new int[amountRequired.amountRequired.Count];
		for (int i = 0; i < amountRequired.recipe.Count; i++)
		{
			for (int j = 0; j < itemInside.Count; j++)
			{
				if (amountRequired.recipe[i] == itemInside[j].contain)
				{
					test[i] += 1;

				}


			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			GameManager.getInstance.ShowRecipeOnUI(test);

		}
		if (CheckIfCompletedRecipe(test) && canGetReward)
		{
			canGetReward = false;
			canGenerateNewRecipe = true;


			if (PhotonNetwork.IsMasterClient)
				SpawnItemRecipe();

			DeleteBarrelsFromDropZone();

			notification.ShowText("Item has crafted: " + currentRecipe.recipeReward.itemName);

		}
		else
		{
		}


	}


	public void DeleteBarrelsFromDropZone()
	{

		List<InteractItem> destroyableBarrels = new List<InteractItem>();


		for (int i = 0; i < itemInside.Count; i++)
		{
			//Debug.Log("Preparing to destroy barrels");
			//destroyableBarrels.Add(itemInside[i]);
			//checkList(destroyableBarrels);
			//itemInside[i].gameObject.SetActive(false);
			itemInside[i].GetComponent<InteractItem>().SetGameObjectActive(false, new Vector3(0, 0, 0),RecipeItems.Empty);
		}






	}

	void checkList(List<InteractItem> _barrels)
	{
		int[] amountChecker = new int[currentRecipe.amountRequired.Count];

		for (int i = 0; i < amountChecker.Length; i++)
		{
			for (int j = 0; j < _barrels.Count; j++)
			{
				if (_barrels[j].contain == currentRecipe.recipe[i])
				{

					//_barrels[j].transfer.ReturnToMaster();
					amountChecker[i]++;

					if (amountChecker[i] <= currentRecipe.amountRequired[i])
					{

						Debug.Log("Destroying barrels: " + i);
						if (_barrels[i].photonView.IsMine)
						{
							if (_barrels[i] != null)
								PhotonNetwork.Destroy(_barrels[j].gameObject.GetPhotonView());
						}

					}

				}
			}
		}




		//for (int i = itemInside.Count; i > 0; i--)
		//{
		//	if(itemInside[i] == null)
		//	{
		//		itemInside.RemoveAt(i);
		//	}
		//}
	}



	bool CheckIfCompletedRecipe(int[] intArray)
	{
		int checker = 0;
		for (int i = 0; i < currentRecipe.amountRequired.Count; i++)
		{
			if (intArray[i] >= currentRecipe.amountRequired[i])
				checker += 1;
		}
		if (checker == currentRecipe.amountRequired.Count)
			return true;
		else
			return false;

	}
	public void SpawnItemRecipe()
	{


		int _index = itemBank.itemList.FindIndex(x => x.itemName == currentRecipe.recipeReward.itemName);

		GameManager.getInstance.ShowDroppedItemInfo(_index);

		//reward.GetComponent<PickableItem>().ShowItemOnFloor(currentRecipe.recipeReward);
	}
	public void ChangeRewardInfo(ItemSO _itemInfo)
	{
		_reward.GetComponent<PickableItem>().ShowItemOnFloor(_itemInfo);
		Debug.Log("Reward: " + _reward.GetComponent<PickableItem>().pickableItem.itemName);
	}
	public void TeleportPlayersOutsideOfShop()
	{
		for (int i = 0; i < insidePlayers.Length; i++)
		{
			if (insidePlayers[i].gameObject.GetPhotonView().IsMine)
				insidePlayers[i].transform.position = kickFromShopPlace.position;
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(dropSite.position, dropSiteRadius);


	}
}
public enum RecipeItems
{
	Empty,
	Wood,
	Leather,
	Metal
}

[Serializable]
public class ShopRecipe
{
	public List<Recipe> RecipeList = new List<Recipe>();
}

[Serializable]
public class Recipe
{
	public List<RecipeItems> recipe;
	public List<int> amountRequired;
	public ItemSO recipeReward;


	//item 
}