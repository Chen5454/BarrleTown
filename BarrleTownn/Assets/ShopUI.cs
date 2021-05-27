using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public Shop shopRef;

    [Header("Show Recipe")]
    public Image[] itemImage;
    public TextMeshProUGUI[] itemAmountText;
	public Color[] tempColor; //for now the item sprites will be barrels with different color, metal = gray, wood = brown, leather = orange;

	public void ShowRecipe()
	{
		for (int i = 0; i < shopRef.currentRecipe.recipe.Length; i++)
		{
			if(i < shopRef.currentRecipe.recipe.Length)
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
