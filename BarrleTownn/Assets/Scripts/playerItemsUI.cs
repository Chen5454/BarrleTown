using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class playerItemsUI : MonoBehaviour
{
	public Image[] equippedItems;//0 shoes, 1 armor, 2 crossbow
	public TextMeshProUGUI crossbowAmount;
	public GameObject xButton;
	public GameObject clawImage;

	private void Awake()
	{

	}

	public void OrgenizeUI()
	{
		if (GameManager.getInstance.player.isWerewolf)
		{
			equippedItems[0].gameObject.transform.parent.gameObject.SetActive(false);
			equippedItems[1].gameObject.transform.parent.gameObject.SetActive(false);
			clawImage.SetActive(true);
		}
	}



	public void UpdateItemsUI(ShoeSO shoe, ArmorSO armor, GunSO gun)
	{
		SetActiveItemImage(shoe, 0);
		SetActiveItemImage(armor, 1);
		SetActiveItemImage(gun, 2);
	}

	public void UpdatePlayerItemUI(ItemSO item)
	{
		SetActiveItemImage(item, ReturnIndexByItemType(item));
	}

	int ReturnIndexByItemType(ItemSO item)
	{
		if (item as ShoeSO)
		{
			return 0;
		}
		else if (item as ArmorSO)
		{
			return 1;
		}
		else if (item as GunSO)
		{
			return 2;
		}
		return -1;
	}

	public void SetActiveItemImage(ItemSO item, int index)
	{
		if (item == null && index != -1)
		{
			equippedItems[index].color = new Color(1, 1, 1, 0);
			Debug.Log("UI ITEM!");

		}
		else if (index != -1)
		{

			equippedItems[index].color = new Color(1, 1, 1, 1);


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
	public void UnEquipUI(int index)
	{
		if (index == 0)
		{
			equippedItems[0].color = new Color(1, 1, 1, 0);
		}
		if (index == 1)
		{
			equippedItems[1].color = new Color(1, 1, 1, 0);
		}
		if (index == 2)
		{
			equippedItems[2].color = new Color(1, 1, 1, 0);
		}
	}


	public void SetNightUI()
	{
		if (GameManager.getInstance.player.isWerewolf)
		{
			xButton.SetActive(true);
		}
		else
		{
			if (GameManager.getInstance.player.getPlayerItems.getGun != null)
			{
				xButton.SetActive(true);
				crossbowAmount.transform.parent.gameObject.SetActive(true);
				crossbowAmount.text = GameManager.getInstance.player.getPlayerItems.ammo.ToString();
			}

		}
	}
	public void UpdateAmmoText()
	{
		crossbowAmount.text = GameManager.getInstance.player.getPlayerItems.ammo.ToString();
		if (GameManager.getInstance.player.getPlayerItems.ammo <= 0)
		{
			crossbowAmount.transform.parent.gameObject.SetActive(false);
			xButton.SetActive(false);
			crossbowAmount.text = "";
		}
	}
	public void SetDayUI()
	{
		if (GameManager.getInstance.player.isWerewolf)
		{
			xButton.SetActive(false);
		}
		else
		{
			xButton.SetActive(false);
			if (GameManager.getInstance.player.getPlayerItems.getGun != null)
			{
				crossbowAmount.transform.parent.gameObject.SetActive(false);
				crossbowAmount.text = GameManager.getInstance.player.getPlayerItems.ammo.ToString();
			}

		}
	}
}
