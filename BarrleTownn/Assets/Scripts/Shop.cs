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

	//public SpriteRenderer[] recipeItemShow;
	//public Color[] tempColor; //for now the item sprites will be barrels with different color, metal = gray, wood = brown, leather = orange;
	//public Sprite[] amountSprites;
	int playersInsideShopRegion;
	int barrelsInsideShopRegion;
	public bool canGetReward;
	public bool canGenerateNewRecipe;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (PhotonNetwork.IsMasterClient)
			{
				
				//CheckDropSite();
				//GameManager.getInstance.CheckIfRecipeCompleted();
				//photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);
			}
		}


		Collider2D[] insideBarrels = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius, 0, barrelMask);
		Collider2D[] insidePlayers = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius, 0, playerMask);
		//barrels
		if (insideBarrels.Length > 0)
		{
			Debug.Log("Barrel Inside: " + insideBarrels.Length);
			if (barrelsInsideShopRegion != insideBarrels.Length)
			{
				barrelsInsideShopRegion = insideBarrels.Length;
				if (PhotonNetwork.IsMasterClient)
				{
					CheckDropSite();
					GameManager.getInstance.CheckIfRecipeCompleted();
					//GameManager.getInstance.CheckIfRecipeCompleted();photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);
				}
			}

		}
		else
		{
			barrelsInsideShopRegion = 0;
		}

		//players
		if (insidePlayers.Length > 0)
		{
			Debug.Log("People Inside: " + insidePlayers.Length);
			if (playersInsideShopRegion != insidePlayers.Length)
			{
				playersInsideShopRegion = insidePlayers.Length;

				CheckDropSite();
				GameManager.getInstance.CheckIfRecipeCompleted();
				//photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);

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

	}


	public void GenerateNewRecipe()
	{
		int randomizer = UnityEngine.Random.Range(0, shopRecipe.RecipeList.Count);
		currentRecipe = shopRecipe.RecipeList[randomizer];
		canGenerateNewRecipe = false;
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
			Debug.Log("Aquired: " + test[i]);
		}
		if (PhotonNetwork.IsMasterClient)
		{
			GameManager.getInstance.ShowRecipeOnUI(test);

		}
		if (CheckIfCompletedRecipe(test) && canGetReward)
		{
			Debug.LogError("Collected all materials required: Instantiateing item: " + "ItemName");
			canGetReward = false;
			canGenerateNewRecipe = true;
			SpawnItemRecipe();
			DeleteBarrelsFromDropZone();
		}
		else
		{
			Debug.LogError("Didn't collected all required materials");
		}


	}


	public void DeleteBarrelsFromDropZone()
	{
		for (int i = 0; i < currentRecipe.amountRequired.Count; i++)
		{
			for (int k = 0; k < itemInside.Count; k++)
			{
				if (currentRecipe.recipe[i] == itemInside[k].contain)
				{
					PhotonNetwork.Destroy(itemInside[k].photonView);
				}
			}
		}
		for (int i = 0; i < itemInside.Count; i++)
		{
			if (itemInside[i] == null)
				itemInside.RemoveAt(i);
		}


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
		GameObject reward = PhotonNetwork.Instantiate("Pickable", rewardSpawnPosition.position, new Quaternion());
		reward.GetComponent<PickableItem>().ShowItemOnFloor(currentRecipe.recipeReward);
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