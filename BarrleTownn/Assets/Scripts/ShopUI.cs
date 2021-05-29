using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
	public Shop shopRef;

	[Header("Show Recipe")]
	public Image[] itemImage;
	public TextMeshProUGUI[] itemAmountText;
	public Color[] tempColor; //for now the item sprites will be barrels with different color, metal = gray, wood = brown, leather = orange;
	public GameObject recipeUI;
	public void ShowRecipe(int[] amountAquired)
	{
		for (int i = 0; i < itemImage.Length; i++)
		{
			if (i < shopRef.currentRecipe.recipe.Count)
			{
				itemImage[i].gameObject.SetActive(true);
				itemImage[i].color = GetColorByItemType(shopRef.currentRecipe.recipe[i]);
				itemAmountText[i].text = shopRef.currentRecipe.amountRequired[i].ToString();
			}
			else
			{
				itemImage[i].gameObject.SetActive(false);
			}
		}
		ShowNeededAmountOfRecipe(amountAquired);
	}



	public void ShowRecipePanel(bool _show)
	{
		if (shopRef.canGetReward)
		{
			recipeUI.SetActive(_show);
		}
		else
		{
			recipeUI.SetActive(false);
		}
		
	}


	void ShowNeededAmountOfRecipe(int[] amountAquired)
	{

		for (int i = 0; i < shopRef.currentRecipe.recipe.Count; i++)
		{
			int required = shopRef.currentRecipe.amountRequired[i] - amountAquired[i];
			if (required < 0)
				required = 0;

			if (i < shopRef.currentRecipe.recipe.Count)
			{
				itemAmountText[i].text = required.ToString();
			}
		}

	}
	public Color GetColorByItemType(RecipeItems _item)
	{
		switch (_item)
		{
			case RecipeItems.Empty:
				break;
			case RecipeItems.Wood:
				return tempColor[0];
			case RecipeItems.Leather:
				return tempColor[1];
			case RecipeItems.Metal:
				return tempColor[2];
			default:
				break;
		}
		return tempColor[2];
	}

	



}
