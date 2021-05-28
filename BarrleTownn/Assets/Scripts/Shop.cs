using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
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
            CheckDropSite();
            CheckIfRecipeCompleted();
        }
    }

    public void CheckDropSite()
	{
        itemInside = new List<RecipeItems>();
        Collider2D[] insideBarrels = Physics2D.OverlapBoxAll(dropSite.position, dropSiteRadius,0,barrelMask);
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
	public void CheckIfRecipeCompleted()
	{
        int correctAmountOfItems = 0;

		amountRequired = currentRecipe;
		int[] test = new int[amountRequired.amountRequired.Count];
		for (int i = 0; i < amountRequired.recipe.Count; i++)
		{
			for (int j = 0; j < itemInside.Count; j++)
			{
				if(amountRequired.recipe[i] == itemInside[j])
				{
					test[i] += 1;
					
				}


			}
			Debug.Log("Aquired: " + test[i]);
		}
		uiManager.shop.ShowRecipe(test);


  //      Recipe itemCache = new Recipe();
  //      itemCache.recipe = new List<RecipeItems>();

		//      for (int i = 0; i < itemInside.Count; i++)
		//{
		//          itemCache.recipe.Add(itemInside[i]);

		//      }



		//      int test = 0;


		//for (int i = 0; i < currentRecipe.recipe.Count; i++)
		//{
		//          test += currentRecipe.amountRequired[i];

		//          for (int j = 0; j < itemCache.recipe.Count; j++)
		//	{
		//              if(currentRecipe.recipe[i] == itemCache.recipe[j])
		//		{
		//                  correctAmountOfItems += 1;
		//                  itemCache.recipe.RemoveAt(j);
		//                  break;
		//              }
		//	}
		//}



		//      if(correctAmountOfItems == currentRecipe.recipe.Count)
		//{
		//          Debug.Log("Crafted item: " + " item name");
		//      }
		//else
		//{
		//          Debug.Log("not enough to craft item: " + " item name");
		//      }
		//      Debug.Log("Amount of Aquired item: " + correctAmountOfItems);



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