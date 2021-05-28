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

	[Header("Recipe")]
	public ShopRecipe shopRecipe;
	public Recipe currentRecipe;
	public Recipe amountRequired;
	public List<RecipeItems> itemInside = new List<RecipeItems>();

	[Header("references")]
	public UIManager uiManager;
	//public SpriteRenderer[] recipeItemShow;
	//public Color[] tempColor; //for now the item sprites will be barrels with different color, metal = gray, wood = brown, leather = orange;
	//public Sprite[] amountSprites;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (PhotonNetwork.IsMasterClient)
			{
				CheckDropSite();
				photonView.RPC("RPC_CheckIfRecipeCompleted", RpcTarget.AllBufferedViaServer);
			}
		}
	}

	public void CheckDropSite()
	{
		itemInside = new List<RecipeItems>();
		Collider2D[] insideBarrels = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius, 0, barrelMask);
		for (int i = 0; i < insideBarrels.Length; i++)
		{
			itemInside.Add(insideBarrels[i].GetComponent<InteractItem>().contain);
		}


	}


	//   public void ShowRecipeOnShop()
	//{
	//	for (int i = 0; i < recipeItemShow.Length; i++)
	//	{
	//           if(i < currentRecipe.recipe.Length)
	//		{
	//               recipeItemShow[i].gameObject.SetActive(true);
	//               recipeItemShow[i].color = GetRecipeItemSprite(currentRecipe.recipe[i]);
	//               recipeItemShow[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetAmountOfItemSprite(currentRecipe.amountRequired[i]);
	//               //change sprite to fit the recipe

	//           }
	//		else
	//		{
	//               recipeItemShow[i].gameObject.SetActive(false);
	//           }
	//	}
	//}


	//   public Color GetRecipeItemSprite(RecipeItems _item)
	//{
	//	switch (_item)
	//	{
	//		case RecipeItems.Empty:
	//			break;
	//		case RecipeItems.Wood:
	//               return tempColor[0];
	//		case RecipeItems.Leather:
	//               return tempColor[1];
	//		case RecipeItems.Metal:
	//               return tempColor[2];
	//           default:
	//			break;
	//	}
	//       return tempColor[2];
	//   }

	//   public Sprite GetAmountOfItemSprite(int amount)
	//{
	//      return amountSprites[amount];
	//   }


	[PunRPC]
	void RPC_CheckIfRecipeCompleted()
	{
		CheckIfRecipeCompleted();
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
				if (amountRequired.recipe[i] == itemInside[j])
				{
					test[i] += 1;

				}


			}
			Debug.Log("Aquired: " + test[i]);
		}
		if (PhotonNetwork.IsMasterClient)
			photonView.RPC("RPC_ShowRecipeOnUI", RpcTarget.AllBufferedViaServer, test);
		if (CheckIfCompletedRecipe(test))
			Debug.LogError("Collected all materials required: Instantiateing item: " + "ItemName");
		else
		{
			Debug.LogError("Didn't collected all required materials");
		}


	}

	[PunRPC]
	void RPC_ShowRecipeOnUI(int[] _array)
	{
		uiManager.shop.ShowRecipe(_array);
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



	//item 
}