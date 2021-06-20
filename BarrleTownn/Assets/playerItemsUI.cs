using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerItemsUI : MonoBehaviour
{
	public Image[] equippedItems;//0 shoes, 1 armor, 2 crossbow




	public void UpdateItemsUI(ShoeSO shoe, ArmorSO armor,GunSO gun)
	{
		SetActiveItemImage(shoe);
		SetActiveItemImage(armor);
		SetActiveItemImage(gun);
	}

	public void UpdatePlayerItemUI(ItemSO item)
	{
		SetActiveItemImage(item);
	}


	public void SetActiveItemImage(ItemSO item)
	{
		if(item == null)
		{
			if (item as ShoeSO)
				equippedItems[0].color = new Color(1,1,1,0);
			else if(item as ArmorSO)
				equippedItems[1].color = new Color(1, 1, 1, 0);
			else if(item as GunSO)
				equippedItems[2].color = new Color(1, 1, 1, 0);
		}
		else
		{
			if (item as ShoeSO)
				equippedItems[0].color = new Color(1, 1, 1, 1);
			else if (item as ArmorSO)
				equippedItems[1].color = new Color(1, 1, 1, 1);
			else if (item as GunSO)
				equippedItems[2].color = new Color(1, 1, 1, 1);

			UpdateItemSpriteUI(item);
		}
	}

	public void UpdateItemSpriteUI(ItemSO item)
	{
		if (item != null)
		{
			if (item as ShoeSO)
				equippedItems[0].sprite = item.itemSprite;
			else if (item as ArmorSO)
				equippedItems[1].sprite = item.itemSprite;
			else if (item as GunSO)
				equippedItems[2].sprite = item.itemSprite;
		}
	}


}
