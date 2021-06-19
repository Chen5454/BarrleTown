using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PickableItem : MonoBehaviourPunCallbacks
{
	public ItemBankSO itemBank;


	public ItemSO pickableItem;
	[Header("References")]
	public SpriteRenderer spriteRenderer;


	public void ShowItemOnFloor(ItemSO _item)
	{
		gameObject.SetActive(true);
		pickableItem = _item;
		spriteRenderer.sprite = _item.itemSprite;
	}




	public void PickUpItem()
	{
		VanishFromWorld();

	}

	void VanishFromWorld()
	{
		this.gameObject.SetActive(false);
		//make the item vanish from everyone's perspective
	}
}
